using System;
using System.Threading.Tasks;
using Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using ProfessionalPersonalityTypeTest.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {

		private readonly IUserService userService;
		private IConfiguration _config;

		public IdentityController(IUserService _userService, IConfiguration config)
		{
			userService = _userService;
			_config = config;
		}

		[HttpPost]
		[Route("token")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] IdentityModel model)
		{
			IActionResult response = Unauthorized();
			var users = await userService.GetAll();
			var user = users.Where(u => u.Login == model.Login).FirstOrDefault();

			if (user != null)
			{
				var tokenString = GenerateJSONWebToken(user);
				response = Ok(new { token = tokenString });
			}

			return response;
		}

		private string GenerateJSONWebToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Jwt:Issuer"],
			  _config["Jwt:Issuer"],
			  null,
			  expires: DateTime.Now.AddMinutes(120),
			  signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		//[HttpPost]
		//[Route("token")]
		//[AllowAnonymous]
		//public async Task<IActionResult> Token([FromBody] IdentityModel model)
		//{
		//	var identity = await GetIdentity(model.Login, model.Password);
		//	if (identity == null)
		//	{
		//		return Unauthorized();
		//	}

		//	var now = DateTime.UtcNow;
		//	var jwt = new JwtSecurityToken(
		//			issuer: AuthOptions.ISSUER,
		//			audience: AuthOptions.AUDIENCE,
		//			notBefore: now,
		//			claims: identity,
		//			expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
		//			signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
		//	var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

		//	return Ok(encodedJwt);
		//}

		//private async Task<IReadOnlyCollection<Claim>> GetIdentity(string login, string password)
		//{
		//	List<Claim> claims = null;
		//	var users = await userService.GetAll();
		//	var user = users.Where(u => u.Login == login).FirstOrDefault();

		//	if (user != null)
		//	{
		//		var sha256 = new SHA256Managed();
		//		var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
		//		if (passwordHash == user.Password)
		//		{
		//			claims = new List<Claim>
		//			{
		//				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
		//			};
		//		}
		//	}
		//	return claims;
		//}
	}
}
