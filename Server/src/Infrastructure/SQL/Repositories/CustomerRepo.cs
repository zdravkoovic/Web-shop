using Infrastructure.SQL.Database.Model;
using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SQL.Repositories;
public class CustomerRepo(Context context, UserManager<ApiUser> userManager) : ICustomerRepo
{
    private readonly Context _context = context;
    private readonly UserManager<ApiUser> _userManager = userManager;
    public async Task<string> CreateAsync(CustomerDto customer)
    {
        var customerEntity = new ApiUser
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            UserName = customer.Nickname,
            Email = customer.Email,
            Address = customer.Address
        };
        try
        {
            var result = await _userManager.CreateAsync(customerEntity, customer.Password!);
            if(result.Succeeded){
                return customerEntity.Id;
            }
            // await _context.SaveChangesAsync();
            else{
                throw new Exception(
                    string.Format("Error {0}", string.Join("", 
                        result.Errors.Select(e => e.Description))));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Doslo je do greske kod kreiranja kupca\n" + ex.Message);
        }

    }

    public async Task<int> DeleteAsync(int Id)
    {
        return await _context.Accounts.Where(x => int.Parse(x.Id) == Id).ExecuteDeleteAsync();
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        return await _context.Accounts
            .AsNoTracking()
            .Select(x => new CustomerDto{
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Nickname = x.UserName,
                Email = x.Email,
                Address = x.Address
            })
            .ToListAsync();
    }

    public async Task<CustomerDto> RetrieveAsync(int Id)
    {
        var customer = await _context.Accounts
            .AsNoTracking()
            .Where(x => int.Parse(x.Id) == Id)
            .FirstOrDefaultAsync();
        if(customer != null)
            return new CustomerDto{
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Nickname = customer.UserName,
                Email = customer.Email,
                Address = customer.Address
            };
        return new CustomerDto{
            FirstName = "",
            LastName = ""
        };
    }

    public async Task<int> UpdateAsync(CustomerDto customer)
    {
        return await _context.Accounts
            .Where(x => x.Id == customer.Id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(p => p.UserName, customer.Nickname)
            .SetProperty(p => p.FirstName, customer.FirstName)
            .SetProperty(p => p.LastName, customer.LastName)
            .SetProperty(p => p.Email, customer.Email)
            .SetProperty(p => p.Address, customer.Address));
    }

    public async Task<int> UpdateNicknameAsync(int Id, string Nickname)
    {
        return await _context.Accounts
            .Where(x => x.Id == Id.ToString())
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.UserName, Nickname));
    }
}