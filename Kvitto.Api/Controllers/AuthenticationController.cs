using Kvitto.Data.Migrations;
using Kvitto.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kvitto.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthenticationController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            //var user = ValidateUserCredentials(
            //    authenticationRequestBody.UserName,
            //    authenticationRequestBody.Password);
            //    )

            //if(user == null)
            //{
            return Unauthorized();
            //}
        }

        private async Task<ApplicationUser> ValidateUserCredentials(string? userName, string? password)
        {
            //TODO do login and return User
            if(userName != null && password != null)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                }
                if (result.RequiresTwoFactor)
                {
                    //we do not support this
                }
                if (result.IsLockedOut)
                {
                    //TODO _logger.LogWarning("User account locked out.");
                }
                else
                {
                    //TODO reply
                    //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName
            };
        }
    }
}
