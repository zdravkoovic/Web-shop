using Infrastructure.SQL.Database.Model;
using WebShop.src.Domain.DTOs;

namespace WebShop.src.Domain.Repository.Interfaces;
public interface IProductRepo
{
    Task<int> CreateAsync(Product product);
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int Id);
    Task<Product?> GetByIdWithImagesAsync(int id);
    Task<IEnumerable<CommentResponseDTO>?> GetCommentsAsync(int id);
    Task<Comment> SaveCommentAsync(Comment comment);
}