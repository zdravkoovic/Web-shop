namespace WebShop.src.BLL.Services;

public interface IProductImgService
{
    Task<bool> UploadAsync(List<IFormFile> files, int productId);
}