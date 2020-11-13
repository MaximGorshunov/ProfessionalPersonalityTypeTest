using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using ProfessionalPersonalityTypeTest.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Find user by his identity key.
        /// Admin can find any user.
        /// User can find only himself.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

                var user = await userService.GetById(id);
                UserResponse _user = new UserResponse();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();

                response.Data = _user;
                
                return Json(response);
            }
            catch(Exception ex)
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                response.ErrorMessage = $"Couldn't get user : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Get all users from DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll([FromBody] UserGetAllRequest request)
        {
            try
            {
                var users = await userService.GetAll(request.Gender, request.LoginFilter, request.EmailFilter, request.AgeMin, request.AgeMax, request.Role);
                
                var _users = users.Select(u => new UserResponse { Id = u.Id,
                                                             IsAdmin = u.IsAdmin,
                                                             Login = u.Login,
                                                             Email = u.Email,
                                                             Birthdate = u.Birthdate,
                                                             IsMan = u.IsMan }).ToList();
                
                ApiResponse<List<UserResponse>> response = new ApiResponse<List<UserResponse>>();
                
                response.Data = _users;
                
                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                response.ErrorMessage = $"Couldn't get users  : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary> 
        /// Add new user in DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <param name="userCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] UserCreate userCreate)
        {
            try
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();

                if (!ModelState.IsValid)
                        return BadRequest();

                var user = await userService.Create(userCreate.IsAdmin, userCreate.Login, userCreate.Email, userCreate.Birthdate, userCreate.IsMan, userCreate.Password);

                if (user == null)
                {
                    response.ErrorMessage = "User with such login or email already exists";
                    return Json(response);
                }

                UserResponse _user = new UserResponse();

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
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                response.ErrorMessage = $"Couldn't create user : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Updade user.
        /// Admin can update any user.
        /// User can update only himself.
        /// </summary>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdate userUpdate)
        {
            try
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                
                if (!ModelState.IsValid)
                        return BadRequest();

                var currentUserId = int.Parse(User.Identity.Name);
                if (userUpdate.Id != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

                var user = await userService.Update(userUpdate.Id, userUpdate.IsAdmin, userUpdate.Login, userUpdate.Email, userUpdate.Birthdate, userUpdate.IsMan, userUpdate.Password);

                if (user == null)
                {
                    response.ErrorMessage = "User with such login or email already exists";
                    return Json(response);
                }

                UserResponse _user = new UserResponse();

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
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                response.ErrorMessage = $"Couldn't update user : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Delete user from DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete")]
        [Authorize(Roles = Roles.Admin)]
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
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                response.ErrorMessage = $"Couldn't delete user : {ex.Message}";
                return Json(response);
            }
        }
    }
}
