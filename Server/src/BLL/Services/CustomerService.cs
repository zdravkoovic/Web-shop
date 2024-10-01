using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;

namespace BLL.Services;

public class CustomerService(ICustomerRepo repo) : ICustomerService
{
    private readonly ICustomerRepo _repo = repo;

    public async Task<string> CreateOrUpdateAsync(CustomerDto customer)
    {
        if(customer.Id is null) return await _repo.CreateAsync(customer);
        if(await _repo.UpdateAsync(customer) > 0) return customer.Id;
        return "-1";
    }

    public async Task<bool> DeleteAsync(int Id)
    {
        return await _repo.DeleteAsync(Id) > 0;
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<CustomerDto> RetrieveAsync(int Id)
    {
        return await _repo.RetrieveAsync(Id);
    }

    public async Task<bool> UpdateNicknameAsync(int Id, string Nickname)
    {
        return await _repo.UpdateNicknameAsync(Id, Nickname) > 0;
    }
}