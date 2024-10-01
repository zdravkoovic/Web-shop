using Infrastructure.SQL.Database.Model;

namespace WebShop.src.Domain.Repository.Interfaces;

public interface IProductImageRepo
{
    Task<bool> UploadAsync(List<IFormFile> files, int productId); 
}