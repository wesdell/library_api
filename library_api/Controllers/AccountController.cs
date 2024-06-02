using library_api.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

		[HttpGet("renewtoken")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<AuthenticationResponse>> RenewToken()
		{
			Claim emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Email).FirstOrDefault();
			string email = emailClaim.Value;

			UserCredentials userCredentials = new UserCredentials()
			{
				Email = email
			};

			return await this.SetUserToken(userCredentials);
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthenticationResponse>> LogIn(UserCredentials userCredentials)
		{
			Microsoft.AspNetCore.Identity.SignInResult account = await this._signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
			if (!account.Succeeded)
			{
				return BadRequest("Incorrect email or password.");
			}
			return await this.SetUserToken(userCredentials);
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

			return await this.SetUserToken(userCredentials);
		}

		[HttpPost("setadmin")]
		public async Task<ActionResult> SetAdmin(UpdateAdminDTO updateAdminDTO)
		{
			IdentityUser user = await this._userManager.FindByEmailAsync(updateAdminDTO.Email);
			await this._userManager.AddClaimAsync(user, new Claim("Admin", "1"));
			return NoContent();
		}

		[HttpPost("removeadmin")]
		public async Task<ActionResult> RemoveAdmin(UpdateAdminDTO updateAdminDTO)
		{
			IdentityUser user = await this._userManager.FindByEmailAsync(updateAdminDTO.Email);
			await this._userManager.RemoveClaimAsync(user, new Claim("Admin", "1"));
			return NoContent();
		}

		private async Task<AuthenticationResponse> SetUserToken(UserCredentials userCredentials)
		{
			List<Claim> claims = new List<Claim>() {
				new Claim(ClaimTypes.Email, userCredentials.Email)
			};

			IdentityUser user = await this._userManager.FindByEmailAsync(userCredentials.Email);
			IList<Claim> userDBClaims = await this._userManager.GetClaimsAsync(user);
			claims.AddRange(userDBClaims);

			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JWT_SECRET"]));
			SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			DateTime expires = DateTime.Now.AddMonths(1);
			JwtSecurityToken securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

			return new AuthenticationResponse() { Token = new JwtSecurityTokenHandler().WriteToken(securityToken), Expires = expires };
		}
	}
}