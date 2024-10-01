using Microsoft.AspNetCore.Mvc;
using WebShop.src.BLL.Services;
using WebShop.src.Domain.DTOs;
using WebShop.src.Domain.Repository.Interfaces;
using WebShop.src.Domain.Services;

namespace WebShop.src.API.Controllers.Commands;

[ApiController]
[Route("[controller]/[action]")]
public class ImagesController(
    IAvatarImageRepo imagesRepo,
    IProductImgService productImgService
    ) : ControllerBase
{
    private readonly IAvatarImageRepo _imagesRepo = imagesRepo;
    private readonly IProductImgService _productsImgService = productImgService;

    [HttpPost]
    [Route("/upload-avatar")] 
    public async Task<IResult> UploadAvatar([FromForm] AvatarImageDTO images, string userId)
    {
        return Results.Ok(await _imagesRepo.UploadImagesAsync(images.Files[0], userId));
    }

    [HttpPost]
    [Route("/upload-images")]
    [Consumes("multipart/form-data")]
    public async Task<IResult> UploadImages(List<IFormFile> files, int productId)
    {
        return await _productsImgService.UploadAsync(files, productId) ? 
                Results.Ok("Image(s) are uploaded.") : Results.BadRequest("Not all images are uploaded or it is none.");
    }
}