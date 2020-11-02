﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ProfessionalPersonalityTypeTest.Models;
using Service.IServices;
using System.Linq;
using Models;
using System.Threading.Tasks;
using System.Text;
using ProfessionalPersonalityTypeTest.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService userService;
        private readonly AppSettings appSettings;

        public AuthenticationController(IUserService _userService, IOptions<AppSettings> _appSettings)
        {
            userService = _userService;
            appSettings = _appSettings.Value;
        }

        private async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await userService.GetAll();
            var checkUser = user.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();

            // return null if user not found
            if (checkUser == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(checkUser);

            return new AuthenticateResponse(checkUser, token);
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authentication(AuthenticateRequest model)
        {
            try
            {
                ApiResponse<AuthenticateResponse> response = new ApiResponse<AuthenticateResponse>();
                response.Data = await Authenticate(model);

                if (response.Success)
                    return Json(response);

                response.ErrorMessage = "Username or password is incorrect";
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                ApiResponse<AuthenticateResponse> response = new ApiResponse<AuthenticateResponse>();
                response.ErrorMessage = "Aunthentication error : " + ex.Message;
                return Json(response);
            }
        }
    }
}