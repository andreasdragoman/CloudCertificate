using IdentityModule;
using IdentityModule.Models;
using IdentityModule.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;

namespace CloudCertificate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        //private readonly IEmailSender _sender;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var returnUrl = Url.Content("~/");

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, false);
            var user = await _userManager.FindByNameAsync(request.Username);
            if (result.Succeeded)
            {
                //return LocalRedirect(returnUrl);
                return Ok(new
                {
                    Token = await CreateJwtToken(user)
                });
            }
            return NotFound();
        }

        [HttpPost(template: "register")]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var returnUrl = Url.Content("~/");
            var user = new AppUser { UserName = request.Username, Email = request.Email, Name = request.Name };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { area = "Identity", userId = user.Id, code = code }, protocol: Request.Scheme);
                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    //return RedirectToPage("RegisterConfirmation", new { email = request.Email });
                    return Ok();
                }
                else
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost(template: "logout")]
        public async Task<IActionResult> Logout()
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var returnUrl = Url.Content("~/");
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        private async Task<string> CreateJwtToken(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecretveryverysecretveryverysecret");

            var currentUserRole = User.Claims?.Where(c => c.Type == ClaimTypes.Role)?.Select(c => c.Value)?.FirstOrDefault();
            if (currentUserRole == null)
            {
                currentUserRole = "Test";
            }

            var currentUserRoleFromManagerList = await _userManager.GetRolesAsync(user);
            var currentUserRoleFromManager = currentUserRoleFromManagerList?.FirstOrDefault();

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, currentUserRole),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.GivenName, user.Name)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                return jwtTokenHandler.WriteToken(token);
            }
            catch(Exception ex)
            {
                var x = ex.Message;
                throw;
            }
        }
    }
}
