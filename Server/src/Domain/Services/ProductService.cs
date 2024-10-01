using System.Text;
using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebShop.src.BLL.Services;
using WebShop.src.Domain.DTOs;
using WebShop.src.Domain.Repository.Interfaces;

namespace WebShop.src.Domain.Services;

public class ProductService(
    IProductRepo _productRepo,
    UserManager<ApiUser> _userManager,
    IAvatarImageRepo _avatarImageRepo
) : IProductService
{
    private readonly IProductRepo productRepo = _productRepo;
    private readonly IAvatarImageRepo avatarImageRepo = _avatarImageRepo;
    private readonly UserManager<ApiUser> userManager = _userManager;

    public async Task<int> CreateAsync(ProductDTO productDTO)
    {
        return await productRepo.CreateAsync(new Product{
            Category = productDTO.Category!,
            Comments = [],
            Images = [],
            Name = productDTO.Name!,
            OrderItems = [],
            Price = productDTO.Price!,
            StockQuantity = productDTO.StockQuantity!
        });
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await productRepo.GetAllAsync();
    }

    public Task<Product?> GetByIdWithImagesAsync(int id)
    {
        return productRepo.GetByIdWithImagesAsync(id);
    }

    public async Task<(IEnumerable<CommentResponseDTO>?, float, CommentResponseDTO?)> GetCommentsAsync(int id, ApiUser? user)
    {
        if(id < 0) throw new Exception("Id must not be negative!");

        var comments = await productRepo.GetCommentsAsync(id);
        if(comments == null) return (null, 0, null);

        comments = [.. comments.OrderByDescending(c => c.Timestamp)];
        
        float rating = comments.Aggregate(0, (acc, x) => acc + x.Rate);
        rating /= comments.Count();

        if(user != null){

            var username = user.FirstName + " " + user.LastName;
            var comment = comments.Where(x => x.Username == username).FirstOrDefault();
            return (comments, rating, comment);
        
        }

        return (comments, rating, null);
    }

    public async Task<string?> GetDescriptionAsync(int id)
    {
        var product = await productRepo.GetByIdAsync(id);
        if(product == null) return null;
        return product.Description;
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        return await productRepo.GetByIdAsync(id);
    }

    public async Task<CommentHubDTO> LeaveCommentAsync(ApiUser user, Product product, CommentRequestDTO commentDTO)
    {
        var comment =  await productRepo.SaveCommentAsync(new Comment{
            User = user,
            UserId = user.Id,
            Product = product,
            ProductId = product.Id,
            Text = commentDTO.Comment,
            Timestamp = DateTime.UtcNow,
            Rate = commentDTO.Rate,
        });

        // var avatarImage = await this.avatarImageRepo.GetPictureByUserIdAsync(user.Id);

        return new CommentHubDTO{
            Rate = comment.Rate,
            Text = comment.Text,
            Timestamp = comment.Timestamp,
            Username = user.FirstName + " " + user.LastName,
        };
    }
}