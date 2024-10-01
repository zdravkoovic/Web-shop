using Domain.Repositories;
using Domain.Services;

namespace BLL.Services;

public class AdminService(IAdminRepo adminRepo) : IAdminService
{
    private readonly IAdminRepo _adminRepo = adminRepo;
    public async Task<bool> AddUserToRolesAsync(string userName, string[] roleNames)
    {
        return await _adminRepo.AddUserToRolesAsync(userName, roleNames);
    }
}