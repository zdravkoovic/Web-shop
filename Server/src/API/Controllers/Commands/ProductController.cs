using System.Security.Claims;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.src.BLL.Services;
using WebShop.src.Domain.DTOs;

namespace WebShop.src.API.Controllers.Commands;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class ProductController(
    IProductService _productService,
    UserManager<ApiUser> _userManager,
    Context _context
) : ControllerBase
{
    private readonly IProductService productService = _productService;
    private readonly UserManager<ApiUser> userManager = _userManager;
    private readonly Context context = _context;

    [HttpPost]
    [Route("/create")]
    public async Task<IResult> Create([FromBody] ProductDTO product)
    {
        return Results.Ok("Kreiran je proizvod - ID = " + await productService.CreateAsync(product));
    }

    [HttpPost]
    [Route("/leave-your-comment")]
    public async Task<IActionResult> LeaveYourComment([FromBody] CommentRequestDTO commentDTO){
        
        var product = await productService.GetProductAsync(commentDTO.ProductId);
        if(product == null) return StatusCode(StatusCodes.Status404NotFound, "The product doesn't exist.");
        
        var userId = User.FindFirst(ClaimTypes.Sid)!.Value;
        var user = await userManager.FindByIdAsync(userId);

        var comment = await productService.LeaveCommentAsync(user!, product, commentDTO);
        return Ok(comment);    
    }
} 