using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.SQL.Database.Model;

namespace BLL.Services;

public class CustomerService(ICustomerRepo repo) : ICustomerService
{
    private readonly ICustomerRepo _repo = repo;

    public async Task<string> CreateOrUpdateAsync(CustomerDto customer)
    {
        if(customer.Id is null) return await _repo.CreateAsync(new ApiUser{
            UserName = customer.Nickname,
            LastName = customer.LastName!,
            FirstName = customer.FirstName!,
            PasswordHash = customer.Password,
            Email = customer.Email,
        });
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

    public async Task<CustomerDto> RetrieveAsync(string Id)
    {
        if(string.IsNullOrWhiteSpace(Id) || !Guid.TryParse(Id, out _))
        {
            throw new ArgumentException("UserId cannot be null or empty and must be a valid Guid!", nameof(Id));
        }


        var user = await _repo.RetrieveAsync(Id);
        return new CustomerDto{
            Id = user?.Id,
            FirstName = user?.FirstName,
            LastName = user?.LastName,
            Email = user?.Email,
            Address = user?.Address,
            Nickname = user?.UserName,
        };
    }

    public async Task<bool> UpdateNicknameAsync(int Id, string Nickname)
    {
        return await _repo.UpdateNicknameAsync(Id, Nickname) > 0;
    }
}