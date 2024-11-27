using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.DTOs;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebShop.src.Domain.DTOs;

namespace API.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AccountController(
    Context context,
    IConfiguration configuration,
    UserManager<ApiUser> userManager,
    RoleManager<IdentityRole> roleManager,
    SignInManager<ApiUser> signInManager) : ControllerBase
{
    private readonly Context _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<ApiUser> _userManager = userManager;
    private readonly SignInManager<ApiUser> _signInManager = signInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpPost]
    public async Task<ActionResult> Login(LoginDTO input)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(input.Username!);
            if(user == null
                || !await _userManager.CheckPasswordAsync(user, input.Password!))
                throw new Exception("Invalid login attempt.");
            else
            {
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            _configuration["JWT:SigningKey"]!)),
                            SecurityAlgorithms.HmacSha256);

                var header = new JwtHeader(signingCredentials)
                {
                    ["kid"] = _configuration["JWT:Kid"]
                };

                var IdClaims = new List<Claim>{
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Sub, user.Id),
                    new("oid", Guid.NewGuid().ToString()),
                    new("preferred_username", user.UserName!),
                    new("email", user.Email!),
                    new("name", user.FirstName),
                    new("lastname", user.LastName)
                };

                var accessClaims = new List<Claim>((await _userManager.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));


                var idJwt = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    header: header,
                    new JwtPayload(_configuration["JWT:Issuer"]!,
                                 _configuration["JWT:Audience"]!, 
                                 IdClaims, DateTime.UtcNow, 
                                 DateTime.UtcNow.AddMinutes(
                                    _configuration.GetValue<int>("JWT:IdToken")
                                ))
                ));

                var accessJwt = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    header: header,
                    new JwtPayload(
                        _configuration["JWT:Issuer"]!,
                        _configuration["JWT:Audience"]!, 
                        accessClaims, DateTime.UtcNow, 
                        DateTime.UtcNow.AddMinutes(
                        _configuration.GetValue<int>("JWT:AccessToken")
                    ))
                ));

                return StatusCode(StatusCodes.Status200OK, new {
                    IdToken = idJwt,
                    AccessToken = accessJwt
                });
            }
        }
        catch (Exception e)
        {
            var details = new ProblemDetails
            {
                Detail = e.Message,
                Status = StatusCodes.Status401Unauthorized
            };
            return StatusCode(StatusCodes.Status401Unauthorized, details);
        }
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterDTO input)
    {
        try
        {
            if(ModelState.IsValid)
            {
                var newUser = new ApiUser
                {
                    UserName = input.Username,
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName
                };

                var result = await _userManager.CreateAsync(newUser, input.Password!);

                if(result.Succeeded)
                {
                    return StatusCode(201, $"User '{newUser.UserName} has been created.'");
                }
                else{
                    throw new Exception(
                        string.Format("Error {0}", string.Join("", 
                            result.Errors.Select(e => e.Description))));
                }
            }
            else
            {
                throw new Exception("Problem is model state.");
            }
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails
            {
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddRoles(string[] roles){

        IdentityResult roleResult = new();

        foreach(var role in roles){
            if(!await _roleManager.RoleExistsAsync(role))
                roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
        }

        return Ok(roleResult);
    }

    [HttpPost]
    public async Task<IResult> Logout([FromBody] object empty)
    {
        try
        {
            if(empty == null) return Results.Unauthorized();
            await _signInManager.SignOutAsync();            
        }
        catch (Exception ex)
        {
            throw new Exception("User didn't logout!\n" + ex.Message);
        }
        return Results.Ok("User logged out!");
    }
}