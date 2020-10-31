using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using ProfessionalPersonalityTypeTest.Models;
using System.Collections.Generic;
using System.Linq;

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

                ApiResponce<UserGet> responce = new ApiResponce<UserGet>();

                responce.Data = _user;
                
                return Json(responce);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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
                
                ApiResponce<List<UserGet>> responce = new ApiResponce<List<UserGet>>();
                
                responce.Data = _users;
                
                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreate userCreate)
        {
            try
            {
                var user = await userService.Create(userCreate.IsAdmin, userCreate.Login, userCreate.Email, userCreate.Birthdate, userCreate.IsMan, userCreate.Password);
                UserGet _user = new UserGet();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                ApiResponce<UserGet> responce = new ApiResponce<UserGet>();

                responce.Data = _user;

                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdate userUpdate)
        {
            try
            {
                var user = await userService.Update(userUpdate.Id, userUpdate.IsAdmin, userUpdate.Login, userUpdate.Email, userUpdate.Birthdate, userUpdate.IsMan, userUpdate.Password);
                UserGet _user = new UserGet();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                ApiResponce<UserGet> responce = new ApiResponce<UserGet>();

                responce.Data = _user;

                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ApiResponce<int> responce = new ApiResponce<int>();
                responce.Data = await userService.Delete(id);
                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
