using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Infrastructure.SQL.Database.Model;
using Infrastructure.SQL.Database.Model.Constants;

namespace API.Commands
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerController(
        ICustomerService service, 
        RoleManager<IdentityRole> roleManager,
        UserManager<ApiUser> userManager ) : ControllerBase
    {
        private readonly ICustomerService _service = service;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<ApiUser> _userManager = userManager;

        [Authorize(Roles = RoleNames.Moderator)]
        [HttpPost]
        [Route("/customers")]
        public async Task<string> CreateCustomerAsync([FromBody] CustomerDto customer)
        {
            return "Uspesno dodat kupac sa ID-jem " + await _service.CreateOrUpdateAsync(customer);
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpDelete]
        [Route("/customers/{Id}")]
        public async Task<string> DeleteCustomer(int Id)
        {
            if(await _service.DeleteAsync(Id))
                return "Uspesno uklonjen kupac!";
            return "Kupac nije uklonjen!";
        }

        [Authorize(Roles = RoleNames.Moderator)]
        [HttpPut]
        [Route("/customers/{Id}/{Nickname}")]
        public async Task<string> UpdateNickname(int Id, string Nickname)
        {
            if(await _service.UpdateNicknameAsync(Id, Nickname))
                return "Uspesno promenjen nickname!";
            return "Nickname nije promenjen!";
        }

        [Authorize(Roles = RoleNames.Moderator)]
        [HttpPost]
        public async Task<string> CreateOrUpdateCustomer([FromBody] CustomerDto customer)
        {
            string Id = await _service.CreateOrUpdateAsync(customer);
            if(Id != "-1") return $"Uspesno izvrseno! ID je {Id}";
            return "Loso resenje!";
        }

        [HttpPost]
        public async Task<IActionResult> AuthData()
        {
            int rolesCreated = 0;
            int usersAddedToRoles = 0;

            if(!await _roleManager.RoleExistsAsync(RoleNames.Moderator))
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.Moderator));
                rolesCreated++;
            }
            if(!await _roleManager.RoleExistsAsync(RoleNames.Administrator))
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.Administrator));
                rolesCreated++;
            }

            var testModerator = await _userManager.FindByNameAsync("TestModerator");
            if(testModerator != null
                && !await _userManager.IsInRoleAsync(
                    testModerator, RoleNames.Moderator
                ))
            {
                await _userManager.AddToRoleAsync(testModerator, RoleNames.Moderator);
                usersAddedToRoles++;
            }

            var testAdministrator = await _userManager.FindByNameAsync("TestAdministrator");
            if(testAdministrator != null
                && !await _userManager.IsInRoleAsync(testAdministrator, RoleNames.Administrator))
            {
                await _userManager.AddToRoleAsync(testAdministrator, RoleNames.Moderator);
                await _userManager.AddToRoleAsync(testAdministrator, RoleNames.Administrator);
                usersAddedToRoles++;
            }

            return new JsonResult(new 
            {
                RolesCreated = rolesCreated,
                UsersAddedToRoles = usersAddedToRoles
            });
        }

    }
}