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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();
                response.ErrorMessage = $"Couldn't get user : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Get all users from DB.
        /// Only Admin is allowed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [Authorize(Roles = Roles.Admin)]
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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();
                response.ErrorMessage = "Couldn't get users";
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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();

                if (!ModelState.IsValid)
                {
                    response.ErrorMessage = "Model State not valid";
                    return BadRequest(response);
                }

                var user = await userService.Create(userCreate.IsAdmin, userCreate.Login, userCreate.Email, userCreate.Birthdate, userCreate.IsMan, userCreate.Password);

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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();
                response.ErrorMessage = "Couldn't create user";
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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();
                
                if (!ModelState.IsValid)
                {
                    response.ErrorMessage = "Model State not valid";
                    return BadRequest(response);
                }

                var currentUserId = int.Parse(User.Identity.Name);
                if (userUpdate.Id != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

                var user = await userService.Update(userUpdate.Id, userUpdate.IsAdmin, userUpdate.Login, userUpdate.Email, userUpdate.Birthdate, userUpdate.IsMan, userUpdate.Password);

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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();
                response.ErrorMessage = "Couldn't update user";
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
                ApiResponse<UserGet> response = new ApiResponse<UserGet>();
                response.ErrorMessage = "Couldn't delete user";
                return Json(response);
            }
        }
    }
}
