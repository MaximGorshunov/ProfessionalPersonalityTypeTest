using Microsoft.AspNetCore.Mvc;
using ProfessionalPersonalityTypeTest.Models;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserResultController : Controller
    {
        private readonly IUserResultService userResultService;

        public UserResultController(IUserResultService _userResultService)
        {
            userResultService = _userResultService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userResult = await userResultService.GetById(id);
                UserResultGet _userResult = new UserResultGet();

                _userResult.Id = userResult.Id;
                _userResult.UserId = userResult.UserId;
                _userResult.Date = userResult.Date;
                _userResult.R = userResult.R;
                _userResult.I = userResult.I;
                _userResult.A = userResult.A;
                _userResult.S = userResult.S;
                _userResult.E = userResult.E;
                _userResult.C = userResult.C;

                ApiResponce<UserResultGet> responce = new ApiResponce<UserResultGet>();

                responce.Data = _userResult;

                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userResults = await userResultService.GetAll();
                var _userResults = userResults.Select(u => new UserResultGet
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    Date = u.Date,
                    R = u.R,
                    I = u.I,
                    A = u.A,
                    S = u.S,
                    E = u.E,
                    C = u.C
                }).ToList();

                ApiResponce<List<UserResultGet>> responce = new ApiResponce<List<UserResultGet>>();

                responce.Data = _userResults;

                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserResultCreate userResultCreate)
        {
            try
            {
                var userResult = await userResultService.Create(userResultCreate.UserId, 
                                                          userResultCreate.R, userResultCreate.I, userResultCreate.A, userResultCreate.S, userResultCreate.E, userResultCreate.C);
                UserResultGet _userResult = new UserResultGet();

                _userResult.Id = userResult.Id;
                _userResult.UserId = userResult.UserId;
                _userResult.Date = userResult.Date;
                _userResult.R = userResult.R;
                _userResult.I = userResult.I;
                _userResult.A = userResult.A;
                _userResult.S = userResult.S;
                _userResult.E = userResult.E;
                _userResult.C = userResult.C;

                ApiResponce<UserResultGet> responce = new ApiResponce<UserResultGet>();

                responce.Data = _userResult;

                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UserResultUpdate userResultUpdate)
        {
            try
            {
                var userResult = await userResultService.Update(userResultUpdate.Id,
                                                        userResultUpdate.R, userResultUpdate.I, userResultUpdate.A, userResultUpdate.S, userResultUpdate.E, userResultUpdate.C);
                UserResultGet _userResult = new UserResultGet();

                _userResult.Id = userResult.Id;
                _userResult.UserId = userResult.UserId;
                _userResult.Date = userResult.Date;
                _userResult.R = userResult.R;
                _userResult.I = userResult.I;
                _userResult.A = userResult.A;
                _userResult.S = userResult.S;
                _userResult.E = userResult.E;
                _userResult.C = userResult.C;

                ApiResponce<UserResultGet> responce = new ApiResponce<UserResultGet>();

                responce.Data = _userResult;

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
                responce.Data = await userResultService.Delete(id);
                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
