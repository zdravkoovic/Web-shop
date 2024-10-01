using Infrastructure.SQL.Database.Model;
using WebShop.src.Domain.DTOs;

namespace WebShop.src.BLL.Services;

public interface IProductService 
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetProductAsync(int id);
    Task<string?> GetDescriptionAsync(int id);
    Task<int> CreateAsync(ProductDTO productDTO); 
    Task<Product?> GetByIdWithImagesAsync(int id);
    Task<(IEnumerable<CommentResponseDTO>?, float, CommentResponseDTO?)> GetCommentsAsync(int id, ApiUser? user);
    Task<CommentHubDTO> LeaveCommentAsync(ApiUser user, Product product, CommentRequestDTO commentDTO);
}