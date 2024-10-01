using Domain.DTOs;

namespace Domain.Repositories
{
    public interface ICustomerRepo
    {
       Task<CustomerDto> RetrieveAsync(int Id);
       Task<List<CustomerDto>> GetAllAsync();
       Task<string> CreateAsync(CustomerDto customer);
       Task<int> UpdateAsync(CustomerDto customer);
       Task<int> UpdateNicknameAsync(int Id, string Nickname);
       Task<int> DeleteAsync(int Id);
    }
}