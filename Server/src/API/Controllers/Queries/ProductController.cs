using System.Security.Claims;
using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.src.BLL.Services;

namespace WebShop.src.API.Controllers.Queries;

[Route("[controller]/[action]")]
[ApiController]
public class ProductController(
    IProductService _service,
    UserManager<ApiUser> _userManager
) : ControllerBase
{
    private readonly IProductService service = _service;
    private readonly UserManager<ApiUser> userManager = _userManager;

    [HttpGet]
    [Route("/products")]
    public async Task<IResult> GetAllAsync()
    {
        return Results.Ok(await service.GetAllAsync());
    }

    [HttpGet]
    [Route("/product-image")]
    public IActionResult GetImage(string path)
    {
        return File(System.IO.File.ReadAllBytes(path), "image/jpeg", System.IO.Path.GetFileName(path));
    }

    [HttpGet]
    [Route("/product/{id}")]
    public async Task<IResult> GetProduct(int id)
    {
        var product = await service.GetProductAsync(id);
        if(product == null) return Results.BadRequest(null);
        return Results.Ok(await service.GetByIdWithImagesAsync(id));
    }

    [HttpGet]
    [Route("/product/comments/{productId}")]
    public async Task<ActionResult> GetCommnets(int productId)
    {
        if(await service.GetProductAsync(productId) == null) return StatusCode(StatusCodes.Status404NotFound, "The product doesn't exist!");

        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        var user = string.IsNullOrEmpty(userId) ? null : await userManager.FindByIdAsync(userId);

        var (comments, rating, comment) = await service.GetCommentsAsync(productId, user);
        if(comments == null) return StatusCode(StatusCodes.Status204NoContent);
        return Ok(new
        {
            Rating = rating,
            Comment = comment,
            Comments = comments
        });
    }

    [HttpGet]
    [Route("/product-description/{Id}")]
    public async Task<ActionResult> ProductDetails(int Id)
    {
        var product = await service.GetProductAsync(Id);
        if(product == null) return BadRequest("The product does not exist!");

        var desc = await service.GetDescriptionAsync(Id);

        if(desc == null) return StatusCode(StatusCodes.Status204NoContent);

        return Ok(desc);
    }
}