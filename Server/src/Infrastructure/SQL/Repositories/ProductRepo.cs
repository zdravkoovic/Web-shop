using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebShop.src.Domain.DTOs;
using WebShop.src.Domain.Repository.Interfaces;

namespace WebShop.src.Infrastructure.SQL.Repositories;
public class ProductRepo(Context context) : IProductRepo
{
    private readonly Context _context = context;

    public async Task<int> CreateAsync(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Doslo je do greske kod kreiranja proizvoda. " + ex.Message);
        }
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(x => x.Images)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int Id)
    {
        var product = await _context.Products
            .FindAsync(Id);
        
        if(product != null){
            return product;
        }
        return null;
    }

    public Task<Product?> GetByIdWithImagesAsync(int id)
    {
        // mozda neka validacija

        return _context.Products
            .AsNoTracking()
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CommentResponseDTO>?> GetCommentsAsync(int id)
    {
        var comments = await _context.Comments
                .Where(x => x.ProductId == id)
                .GroupJoin(
                    _context.Avatars,
                    comment => comment.UserId,
                    avatar => avatar.UserId,
                    (comment, avatar) => new {comment, avatar = avatar.FirstOrDefault()}
                )
                .Select(x => new CommentResponseDTO {
                    Rate = x.comment.Rate,
                    Text = x.comment.Text!,
                    Timestamp = x.comment.Timestamp,
                    AvatarPicture = x.avatar!.Content!, // ako user nema sliku, problem
                    Username = x.comment.User.FirstName! + " " + x.comment.User.LastName
                })
                .ToListAsync();
        if(comments.IsNullOrEmpty()) return null;
        return comments;
    }

    public async Task<Comment> SaveCommentAsync(Comment comment)
    {
        try
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment; 
        }
        catch (Exception e)
        {
            throw new Exception("Problem was occured during database process!\n" + e.Message);
        }
    }
}