using WebShop.src.BLL.Services;
using WebShop.src.Domain.Repository.Interfaces;

namespace WebShop.src.Domain.Services;

public class ProductImgService(IProductImageRepo imgRepo, IProductRepo productRepo) : IProductImgService
{
    private readonly IProductImageRepo _imgRepo = imgRepo;
    private readonly IProductRepo _productRepo = productRepo;
    public async Task<bool> UploadAsync(List<IFormFile> files, int productId)
    {
        return await _imgRepo.UploadAsync(files, productId);
    }
}