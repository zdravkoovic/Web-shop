using Infrastructure.SQL.Database.Model;

namespace Domain.Repositories
{
    public interface IAdminRepo
    {
        Task<IResult> CreateAdmin(ApiUser admin);
        ApiUser GetAdmin(int Id);
        ICollection<ApiUser> GetAllAdmins();
        bool UpdateAdmin(int Id);
        bool DeleteAdmin(int Id);
        Task<bool> AddUserToRolesAsync(string userName, string[] roleNames);
    }
}