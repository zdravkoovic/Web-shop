using Domain.Services;
using Infrastructure.SQL.Database.Model;
using Infrastructure.SQL.Database.Model.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace API.Commands;

[Authorize(Roles = RoleNames.Administrator)]
[Route("[controller]/[action]")]
[ApiController]
public class AdminController(UserManager<ApiUser> userManager, RoleManager<IdentityRole> roleManager, IAdminService adminService) : ControllerBase
{
    private readonly UserManager<ApiUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IAdminService _adminService = adminService;

    [HttpPut]
    public async Task<IActionResult> AddUserToRoleAsync(string userName, string[] roleNames)
    {
        if(await _adminService.AddUserToRolesAsync(userName, roleNames)) return Ok("Roles have been successfully added!");
        return BadRequest("Roles have not been successfully added!");
    }
}