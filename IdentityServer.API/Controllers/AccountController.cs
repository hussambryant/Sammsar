using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("UserRegistration")]
        public async Task<IActionResult> RegisterUser(UserRegistrationDto model)
        {
            var user = new User
            {
                UserType = UserType.User,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
                var claims = new List<Claim>()
                {
                    new Claim("ClientProfileId", Guid.NewGuid().ToString()),
                    new Claim("FirstName", model.FirstName),
                    new Claim("LastName", model.LastName)
                };
                await _userManager.AddClaimsAsync(user, claims);
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetUser()
        {
            var body = new
            {
                User.Identity.Name
            };

            return Ok(body);
        }


    }

    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
