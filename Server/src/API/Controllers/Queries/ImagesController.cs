using System.Security.Claims;
using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.src.Domain.Repository.Interfaces;

namespace WebShop.src.API.Controllers.Queries;

[Route("[controller]/[action]")]
[ApiController]
[Authorize]
public class ImagesController(
    IAvatarImageRepo _imagesRepo,
    UserManager<ApiUser> _userManager) : ControllerBase
{
    private readonly IAvatarImageRepo imagesRepo = _imagesRepo;
    private readonly UserManager<ApiUser> userManager = _userManager;

    [AllowAnonymous]
    [HttpGet]
    [Route("/avatar-image/{id}")]
    public async Task<IResult> GetImage(int id)
    {
        var bytes = await imagesRepo.GetPictureByIdAsync(id);
        if (bytes == null) return Results.BadRequest("Lose");
        return Results.File(bytes, "image/jpeg");  
    }

    [HttpGet]
    public async Task<IResult> GetAvatar()
    {
        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;

        if(string.IsNullOrEmpty(userId)) return Results.Unauthorized();
        var bytes = await imagesRepo.GetPictureByUserIdAsync(userId);
        if (bytes == null) return Results.BadRequest("Nije nadjena avatar slicka");
        return Results.File(bytes, "image/jpeg");
    }
}