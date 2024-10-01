using Domain.DTOs;

namespace Domain.Services;

public interface ICustomerService
{
    Task<bool> DeleteAsync(int Id);
    Task<List<CustomerDto>> GetAllAsync();
    Task<CustomerDto> RetrieveAsync(int Id);
    Task<string> CreateOrUpdateAsync(CustomerDto customer);
    Task<bool> UpdateNicknameAsync(int Id, string Nickname);
}