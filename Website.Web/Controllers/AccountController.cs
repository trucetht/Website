using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Account;
using Website.Services;

namespace Website.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // manage token
        private readonly ITokenService _tokenService;
        // manage users
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        // manage sign in
        private readonly SignInManager<ApplicationUserIdentity> _signInManager;

        // initialize the dependencies
        public AccountController(
            ITokenService tokenService,
            UserManager<ApplicationUserIdentity> userManager,
            SignInManager<ApplicationUserIdentity> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // registering the user, if succeeded then will sign them in
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register(ApplicationUserCreate applicationUserCreate)
        {
            var applicationUserIdentity = new ApplicationUserIdentity
            {
                Username = applicationUserCreate.Username,
                Email = applicationUserCreate.Email,
                Fullname = applicationUserCreate.Fullname
            };

            // can take the username and password seperately so that you can hash it
            var result = await _userManager.CreateAsync(applicationUserIdentity, applicationUserCreate.Password);

            if (result.Succeeded)
            {
                applicationUserIdentity = await _userManager.FindByNameAsync(applicationUserCreate.Username);

                // send the token so that it is secure
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                    Username = applicationUserIdentity.Username,
                    Email = applicationUserIdentity.Email,
                    Fullname = applicationUserIdentity.Fullname,
                    Token = _tokenService.CreateToken(applicationUserIdentity)
                };

                return Ok(applicationUser);
            }

            // display the error to the user
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> Login(ApplicationUserLogin applicationUserLogin)
        {
            var applicationUserIdentity = await _userManager.FindByNameAsync(applicationUserLogin.Username);

            // if cannot find the user
            if (applicationUserIdentity != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(
                    applicationUserIdentity,
                    applicationUserLogin.Password, false);

                // if we find the user in the table
                if (result.Succeeded)
                {
                    ApplicationUser applicationUser = new ApplicationUser
                    {
                        ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                        Username = applicationUserIdentity.Username,
                        Email = applicationUserIdentity.Email,
                        Fullname = applicationUserIdentity.Fullname,
                        Token = _tokenService.CreateToken(applicationUserIdentity)
                    };

                    return Ok(applicationUser);
                }
            }
            
            // display the error to the user
            return BadRequest("Invalid login attempt.");
        }
    }
}
