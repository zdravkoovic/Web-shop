namespace WebShop.src.Domain.Repository.Interfaces;

public interface IAvatarImageRepo
{
    Task<int> UploadImagesAsync(IFormFile file, string userId);
    Task<byte[]?> GetPictureByIdAsync(int Id);
    Task<byte[]?> GetPictureByUserIdAsync(string userId);
}