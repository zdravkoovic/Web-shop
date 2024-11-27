using Domain.DTOs;
using Infrastructure.SQL.Database.Model;

namespace Domain.Repositories
{
    public interface ICustomerRepo
    {
       Task<ApiUser?> RetrieveAsync(string Id);
       Task<List<CustomerDto>> GetAllAsync();
       Task<string> CreateAsync(ApiUser customer);
       Task<int> UpdateAsync(CustomerDto customer);
       Task<int> UpdateNicknameAsync(int Id, string Nickname);
       Task<int> DeleteAsync(int Id);
    }
}