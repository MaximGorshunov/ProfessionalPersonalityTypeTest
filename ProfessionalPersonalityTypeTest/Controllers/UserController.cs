using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using ProfessionalPersonalityTypeTest.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Models;
using System.Net;

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
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();

                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage =  $"You do not have permission to get user with id {id}.";
                    return Json(response);
                }

                var user = await userService.GetById(id);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Wrong user identity key.";
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
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return Json(response);
            }
            catch
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get user";
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
                ApiResponse<List<UserResponse>> response = new ApiResponse<List<UserResponse>>();

                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return Json(response);
                }

                var users = await userService.GetAll(request.Gender, request.LoginFilter, request.EmailFilter, request.AgeMin, request.AgeMax, request.Role);
                
                if(!users.Any())
                {
                    HttpContext.Response.StatusCode = 404;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Users not found.";
                    return Json(response);
                } 

                var _users = users.Select(u => new UserResponse { Id = u.Id,
                                                             IsAdmin = u.IsAdmin,
                                                             Login = u.Login,
                                                             Email = u.Email,
                                                             Birthdate = u.Birthdate,
                                                             IsMan = u.IsMan }).ToList();
                
                response.Data = _users;
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return Json(response);
            }
            catch
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = "Couldn't get users.";
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
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return Json(response);
                }

                var user = await userService.Create(userCreate.IsAdmin, userCreate.Login, userCreate.Email, userCreate.Birthdate, userCreate.IsMan, userCreate.Password);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage =  "User with such login or email already exists";
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
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                return Json(response);
            }
            catch
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
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
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();

                if (!ModelState.IsValid)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "Invalid request model.";
                    return Json(response);
                }

                var currentUserId = int.Parse(User.Identity.Name);
                
                if (userUpdate.Id != currentUserId && !User.IsInRole(Roles.Admin))
                {
                    HttpContext.Response.StatusCode = 403;
                    response.Status = HttpContext.Response.StatusCode;
                    response.ErrorMessage = "You do not have permission to update this user.";
                    return Json(response);
                }

                if (User.IsInRole(Roles.User))
                    userUpdate.IsAdmin = false;

                var user = await userService.Update(userUpdate.Id, userUpdate.IsAdmin, userUpdate.Login, userUpdate.Email, userUpdate.Birthdate, userUpdate.IsMan, userUpdate.Password);

                if (user == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    response.Status = HttpContext.Response.StatusCode;
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

                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = _user;
                return Json(response);
            }
            catch
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
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
                HttpContext.Response.StatusCode = 200;
                response.Status = HttpContext.Response.StatusCode;
                response.Data = await userService.Delete(id);
                return Json(response);
            }
            catch
            {
                ApiResponse<UserResponse> response = new ApiResponse<UserResponse>();
                HttpContext.Response.StatusCode = 409;
                response.Status = HttpContext.Response.StatusCode;
                response.ErrorMessage = $"Couldn't delete user";
                return Json(response);
            }
        }
    }
}
