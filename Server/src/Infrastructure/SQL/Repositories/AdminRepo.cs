using Domain.Repositories;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Model;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SQL.Repositories;
public class AdminRepo(Context context, UserManager<ApiUser> userManager, RoleManager<IdentityRole> roleManager) : IAdminRepo
{
    private readonly Context _context = context;
    private readonly UserManager<ApiUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public Task<IResult> CreateAdmin(ApiUser admin)
    {
        throw new NotImplementedException();
    }

    public bool DeleteAdmin(int Id)
    {
        throw new NotImplementedException();
    }

    public ApiUser GetAdmin(int Id)
    {
        throw new NotImplementedException();
    }

    public ICollection<ApiUser> GetAllAdmins()
    {
        throw new NotImplementedException();
    }

    public bool UpdateAdmin(int Id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddUserToRolesAsync(string userName, string[] roleNames)
    {
        var user = await _userManager.FindByNameAsync(userName) ?? throw new Exception("User doesn't exist!");

        foreach (string role in roleNames){
            if(!await _roleManager.RoleExistsAsync(role)) return false;
        }

        foreach(string role in roleNames){
            if(!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        return true;
    }
}