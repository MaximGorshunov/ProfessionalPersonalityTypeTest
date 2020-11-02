using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using ProfessionalPersonalityTypeTest.Models;
using System.Collections.Generic;
using System.Linq;
using ProfessionalPersonalityTypeTest.Helpers;
using Models;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await userService.GetById(id);
                UserGet _user = new UserGet();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                ApiResponse<UserGet> response = new ApiResponse<UserGet>();

                response.Data = _user;
                
                return Json(response);
            }
            catch(Exception ex)
            {
                ApiResponse<Object> response = new ApiResponse<Object>();
                response.ErrorMessage = "Couldn't get user : " + ex.Message;
                return Json(response);
            }
        }

        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await userService.GetAll();
                var _users = users.Select(u => new UserGet { Id = u.Id,
                                                             IsAdmin = u.IsAdmin,
                                                             Login = u.Login,
                                                             Email = u.Email,
                                                             Birthdate = u.Birthdate,
                                                             IsMan = u.IsMan }).ToList();
                
                ApiResponse<List<UserGet>> response = new ApiResponse<List<UserGet>>();
                
                response.Data = _users;
                
                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<Object> response = new ApiResponse<Object>();
                response.ErrorMessage = "Couldn't get users : " + ex.Message;
                return Json(response);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreate userCreate)
        {
            try
            {
                var user = await userService.Create(userCreate.IsAdmin, userCreate.Login, userCreate.Email, userCreate.Birthdate, userCreate.IsMan, userCreate.Password);
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();

                if (user == null)
                {
                    response.ErrorMessage = "User with such login or email already exists";
                    return Json(response);
                }

                UserGet _user = new UserGet();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                response.Data = _user;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<Object> response = new ApiResponse<Object>();
                response.ErrorMessage = "Couldn't create user : " + ex.Message;
                return Json(response);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdate userUpdate)
        {
            try
            {
                var user = await userService.Update(userUpdate.Id, userUpdate.IsAdmin, userUpdate.Login, userUpdate.Email, userUpdate.Birthdate, userUpdate.IsMan, userUpdate.Password);
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();

                if (user == null)
                {
                    response.ErrorMessage = "User with such login or email already exists";
                    return Json(response);
                }

                UserGet _user = new UserGet();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                response.Data = _user;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<Object> response = new ApiResponse<Object>();
                response.ErrorMessage = "Couldn't update user : " + ex.Message;
                return Json(response);
            }
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ApiResponse<int> response = new ApiResponse<int>();
                response.Data = await userService.Delete(id);
                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<Object> response = new ApiResponse<Object>();
                response.ErrorMessage = "Couldn't delete user : " + ex.Message;
                return Json(response);
            }
        }
    }
}
