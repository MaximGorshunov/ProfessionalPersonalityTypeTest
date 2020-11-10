using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
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
        private readonly IQuestionService questionService;
        private readonly IProfessionService professionService;

        public UserResultController(IUserResultService _userResultService, IQuestionService _questionService, IProfessionService _professionService)
        {
            userResultService = _userResultService;
            questionService = _questionService;
            professionService = _professionService;
        }

        /// <summary>
        /// Find test's result by identity key.
        /// Admin can find any result.
        /// User can find only his own result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userResult = await userResultService.GetById(id);

                var currentUserId = int.Parse(User.Identity.Name);
                if (userResult.UserId != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

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

                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't get user's result {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Get all test's results.
        /// Only admin is allowed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = Roles.Admin)]
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

                ApiResponse<List<UserResultGet>> response = new ApiResponse<List<UserResultGet>>();

                response.Data = _userResults;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't get user's results : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Get all test's results of current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall-current-user")]
        [Authorize]
        public async Task<IActionResult> GetAllCurrentUser()
        {
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);

                var userResults = await userResultService.GetAllForUser(currentUserId);

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

                ApiResponse<List<UserResultGet>> response = new ApiResponse<List<UserResultGet>>();

                response.Data = _userResults;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't get user's results : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Get all actual test's results.
        /// Only admin is allowed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall-actual")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllActual()
        {
            try
            {
                var userResults = await userResultService.GetAllActual();
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

                ApiResponse<List<UserResultGet>> response = new ApiResponse<List<UserResultGet>>();

                response.Data = _userResults;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't get actual user's results : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Generate new test result after completing.
        /// </summary>
        /// <param name="answers">
        /// List of profession's id that were chosen.
        /// </param>
        /// <returns></returns>
        [HttpPost("generate")]
        [AllowAnonymous]
        public async Task<IActionResult> Generate([FromBody] List<int> answers)
        {
            try
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();

                var questions = await questionService.GetAll();

                if(answers.Count() != questions.Count())
                {
                    response.ErrorMessage = "Not all questions are answered";
                    return Json(response);
                }

                if(answers.Distinct().Count() != answers.Count())
                {
                    response.ErrorMessage = "Some answers repeated";
                    return Json(response);
                }

                List<Profession> professions = new List<Profession>();

                foreach(var a in answers)
                {
                    var p = await professionService.GetById(a);

                    if (p == null)
                    {
                        response.ErrorMessage = "Answer with id " + a.ToString() + " not found";
                        return Json(response);
                    }

                    professions.Add(p);
                }

                int? UserId = null;
                bool isAuthenticated = User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    UserId = int.Parse(User.Identity.Name);
                }

                var userResult = await userResultService.Generate(UserId, professions);

                if (userResult == null)
                {
                    response.ErrorMessage = "Causes problems while generating result";
                    return Json(response);
                }

                UserResultGet _userResult = new UserResultGet();

                if (isAuthenticated)
                {
                    _userResult.Id = userResult.Id;
                    _userResult.UserId = userResult.UserId;
                }
                _userResult.Date = userResult.Date;
                _userResult.R = userResult.R;
                _userResult.I = userResult.I;
                _userResult.A = userResult.A;
                _userResult.S = userResult.S;
                _userResult.E = userResult.E;
                _userResult.C = userResult.C;

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't generate result : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Add new user's test result in DB.
        /// Only admin is allowed.
        /// </summary>
        /// <param name="userResultCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] UserResultCreate userResultCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var userResult = await userResultService.Create(userResultCreate.UserId,
                                                          userResultCreate.R, userResultCreate.I, userResultCreate.A, userResultCreate.S, userResultCreate.E, userResultCreate.C);
                
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                
                if (userResult == null)
                {
                    response.ErrorMessage = "Causes problems while creating result";
                    return Json(response);
                }

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
                response.Data = _userResult;
                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't create user's result : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Update test's result.
        /// Admin can update any test's result.
        /// User can update only his own test's result.
        /// </summary>
        /// <param name="userResultUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserResultUpdate userResultUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                        return BadRequest();

                var currentUserId = int.Parse(User.Identity.Name);
                if (userResultUpdate.UserId != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

                var userResult = await userResultService.Update(userResultUpdate.Id,
                                                        userResultUpdate.R, userResultUpdate.I, userResultUpdate.A, userResultUpdate.S, userResultUpdate.E, userResultUpdate.C);
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();

                if (userResult == null)
                {
                    response.ErrorMessage = "Invalid identity public key";
                    return Json(response);
                }

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

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't update user's result : {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Delete test's result.
        /// Admin can delete any test's result.
        /// User can delete only his own test's result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userResult = await userResultService.GetById(id);
                var currentUserId = int.Parse(User.Identity.Name);
                if (userResult.UserId != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

                ApiResponse<int> response = new ApiResponse<int>();
                response.Data = await userResultService.Delete(id);
                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();
                response.ErrorMessage = $"Couldn't delete user's result : {ex.Message}";
                return Json(response);
            }
        }
    }
}
