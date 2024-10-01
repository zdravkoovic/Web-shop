using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Model;
using WebShop.src.Domain.Repository.Interfaces;
using WebShop.src.Infrastructure.SQL.Database.Model;

namespace WebShop.src.Infrastructure.SQL.Repositories;

public class ProductImageRepo(
    IConfiguration config,
    Context context,
    IProductRepo productRepo
    ) : IProductImageRepo
{
    private readonly IConfiguration _config = config;
    private readonly Context _context = context;
    private readonly IProductRepo _productRepo = productRepo;

    public async Task<bool> UploadAsync(List<IFormFile> files, int productId)
    {
        var product = await _productRepo.GetByIdAsync(productId);
        if(product == null) return false;
        foreach(var formFile in files){
            
            if(formFile.Length > 0){
                
                var filePath = Path.Combine(_config["StoredFilesPath"]!, Path.GetRandomFileName());
                using var stream = File.Create(filePath);
                await formFile.CopyToAsync(stream);

                try
                {
                    _context.Images.Add(new ProductImage {
                        Path = filePath,
                        Product = product
                    });
                    await _context.SaveChangesAsync();
                   

                }
                catch (Exception err)
                {
                    throw new Exception("Slika nije sacuvana u bazi podataka. " + err.Message);
                }
            }
            else return false;
        }
        return true;
    }
}