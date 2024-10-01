namespace Domain.Services;

public interface IAdminService
{
    Task<bool> AddUserToRolesAsync(string userName, string[] roleNames);
}