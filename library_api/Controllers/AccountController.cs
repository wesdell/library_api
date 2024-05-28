using library_api.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/account")]
	public class AccountController : ControllerBase
	{
		private UserManager<IdentityUser> _userManager;
		private SignInManager<IdentityUser> _signInManager;
		private IConfiguration _configuration;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._configuration = configuration;
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthenticationResponse>> LogIn(UserCredentials userCredentials)
		{
			Microsoft.AspNetCore.Identity.SignInResult account = await this._signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
			if (!account.Succeeded)
			{
				return BadRequest("Incorrect email or password.");
			}
			return this.SetUserToken(userCredentials);
		}

		[HttpPost("signup")]
		public async Task<ActionResult<AuthenticationResponse>> SignUp(UserCredentials userCredentials)
		{
			IdentityUser user = new IdentityUser() { UserName = userCredentials.Email, Email = userCredentials.Email };
			IdentityResult account = await this._userManager.CreateAsync(user, userCredentials.Password);

			if (!account.Succeeded)
			{
				return BadRequest(account.Errors);
			}

			return this.SetUserToken(userCredentials);
		}

		private AuthenticationResponse SetUserToken(UserCredentials userCredentials)
		{
			List<Claim> claims = new List<Claim>() {
				new Claim("name", userCredentials.Name)
			};

			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JWT_SECRET"]));
			SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			DateTime expires = DateTime.Now.AddMonths(1);
			JwtSecurityToken securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

			return new AuthenticationResponse() { Token = new JwtSecurityTokenHandler().WriteToken(securityToken), Expires = expires };
		}
	}
}