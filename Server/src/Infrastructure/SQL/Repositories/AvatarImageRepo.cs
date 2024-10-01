using Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;
using WebShop.src.Domain.Repository.Interfaces;
using WebShop.src.Infrastructure.SQL.Database.Model;

namespace WebShop.src.Infrastructure.SQL.Repositories;

public class AvatarImageRepo(Context context) : IAvatarImageRepo
{
    private readonly Context _context = context;

    public async Task<byte[]?> GetPictureByIdAsync(int Id)
    {
        var image =  await _context.Avatars.FindAsync(Id);
        if(image != null){
            return image.Content;
        }
        return null;
    }

    public async Task<byte[]?> GetPictureByUserIdAsync(string userId)
    {
        var avatar = await _context.Avatars
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

        if(avatar != null) return avatar.Content;
                
        return null;
    }

    public async Task<int> UploadImagesAsync(IFormFile file, string userId)
    {
        var id = -1;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);

            if(memoryStream.Length < 2097152 && memoryStream.Length > 0)
            {
                var images = new AvatarImage
                {
                    Content = memoryStream.ToArray(),
                    UserId = userId
                };

                _context.Avatars.Add(images);
                await _context.SaveChangesAsync();
                id = images.Id;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Slika je ili nepostojeca ili prevelika!");
                Console.WriteLine("Slika je ili nepostojeca ili prevelika!");
            }
        }
        return id;
    }
}