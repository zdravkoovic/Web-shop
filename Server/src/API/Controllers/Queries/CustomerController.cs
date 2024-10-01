using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.src.API.Controllers.Queries;

[ApiController]
[Route("[controller]/[action]")]
public class CustomerController(ICustomerService service) : ControllerBase
{
    private readonly ICustomerService _service = service;

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    [HttpGet]
    public async Task<IResult> GetAllCustomers()
    {
        return Results.Ok(await _service.GetAllAsync());
    }
    [HttpGet]
    [Route("/customers/{Id}")]
    public async Task<IResult> GetCustomer(int Id)
    {
        return Results.Ok(await _service.RetrieveAsync(Id));
    }
}