using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using ProfessionalPersonalityTypeTest.Helpers;
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
        private readonly IUserService userService;

        public UserResultController(IUserResultService _userResultService, IQuestionService _questionService, IProfessionService _professionService, IUserService _userService)
        {
            userResultService = _userResultService;
            questionService = _questionService;
            professionService = _professionService;
            userService = _userService;
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

                var user = await userService.GetById(userResult.UserId);

                UserResponse _user = new UserResponse();

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                UserResultResponse _userResult = new UserResultResponse();

                _userResult.Id = userResult.Id;
                _userResult.Date = userResult.Date;
                _userResult.User = _user;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: PTypeNames.Realistic.ToString(), value: userResult.R, power: PTypePowerConvertor.Convert(userResult.R)));
                _userResult.Results.Add(new PType(name: PTypeNames.Investigative.ToString(), value: userResult.I, power: PTypePowerConvertor.Convert(userResult.I)));
                _userResult.Results.Add(new PType(name: PTypeNames.Artistic.ToString(), value: userResult.A, power: PTypePowerConvertor.Convert(userResult.A)));
                _userResult.Results.Add(new PType(name: PTypeNames.Social.ToString(), value: userResult.S, power: PTypePowerConvertor.Convert(userResult.S)));
                _userResult.Results.Add(new PType(name: PTypeNames.Enterprising.ToString(), value: userResult.E, power: PTypePowerConvertor.Convert(userResult.E)));
                _userResult.Results.Add(new PType(name: PTypeNames.Conventional.ToString(), value: userResult.C, power: PTypePowerConvertor.Convert(userResult.C)));

                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();

                response.Data = _userResult;

                return Json(response);
            }
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't get user's result";
                return Json(response);
            }
        }

        /// <summary>
        /// Get list of result's by filters.
        /// If in role "User" get list of his own results.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromBody] UserResultGetAllRequest request)
        {
            try
            {
                if (!User.IsInRole(Roles.Admin)) 
                {
                    var currentUserId = int.Parse(User.Identity.Name);
                    var user = await userService.GetById(currentUserId);
                    request.LoginFilter = user.Login;
                    request.Gender = null;
                    request.AgeMin = null;
                    request.AgeMax = null;
                }

                List<UserResult> userResults = new List<UserResult>();

                userResults = await userResultService.GetByFilters(request.DataMin, request.DataMax, request.AgeMin, request.AgeMax, request.Gender, request.LoginFilter, request.Actual);

                var _userResults = userResults.Select(u => (u.UserId, new UserResultResponse
                {
                    Id = u.Id,
                    Date = u.Date,
                    
                    Results = new List<PType>()
                    {
                        new PType(name: PTypeNames.Realistic.ToString(), value: u.R, power: PTypePowerConvertor.Convert(u.R)),
                        new PType(name: PTypeNames.Investigative.ToString(), value: u.I, power: PTypePowerConvertor.Convert(u.I)),
                        new PType(name: PTypeNames.Artistic.ToString(), value: u.A, power: PTypePowerConvertor.Convert(u.A)),
                        new PType(name: PTypeNames.Social.ToString(), value: u.S, power: PTypePowerConvertor.Convert(u.S)),
                        new PType(name: PTypeNames.Enterprising.ToString(), value: u.E, power: PTypePowerConvertor.Convert(u.E)),
                        new PType(name: PTypeNames.Conventional.ToString(), value: u.C, power: PTypePowerConvertor.Convert(u.C))
                    }

                })).ToList();

                foreach(var (userId, ur) in _userResults)
                {
                    var user = await userService.GetById(userId);

                    UserResponse _user = new UserResponse();

                    _user.Id = user.Id;
                    _user.IsAdmin = user.IsAdmin;
                    _user.Login = user.Login;
                    _user.Email = user.Email;
                    _user.Birthdate = user.Birthdate;
                    _user.IsMan = user.IsMan;

                    ur.User = _user;
                }

                ApiResponse<List<UserResultResponse>> response = new ApiResponse<List<UserResultResponse>>();

                response.Data = _userResults.Select(u => u.Item2).ToList();

                return Json(response);
            }
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't get user's results";
                return Json(response);
            }
        }

        /// <summary>
        /// Returns statistic.
        /// Only admin is allowed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("statistic")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Statistic([FromBody] UserResultGetAllRequest request)
        {
            try
            {
                List<UserResult> userResults = new List<UserResult>();

                userResults = await userResultService.GetByFilters(request.DataMin, request.DataMax, request.AgeMin, request.AgeMax, request.Gender, request.LoginFilter, request.Actual);

                UserResultStatistic statistic = new UserResultStatistic();
                statistic.High = new Statistic();
                statistic.Middle = new Statistic();
                statistic.Low = new Statistic();

                statistic.High.Realistic = Math.Round(((double) userResults.Count(x => PTypePowerConvertor.Convert(x.R) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Investigative = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.I) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Artistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.A) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Social = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.S) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Enterprising = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.E) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);
                statistic.High.Conventional = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.C) == PTypePowers.High.ToString()) / userResults.Count() * 100), 1);

                statistic.Middle.Realistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.R) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Investigative = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.I) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Artistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.A) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Social = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.S) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Enterprising = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.E) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);
                statistic.Middle.Conventional = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.C) == PTypePowers.Middle.ToString()) / userResults.Count() * 100), 1);

                statistic.Low.Realistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.R) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Investigative = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.I) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Artistic = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.A) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Social = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.S) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Enterprising = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.E) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);
                statistic.Low.Conventional = Math.Round(((double)userResults.Count(x => PTypePowerConvertor.Convert(x.C) == PTypePowers.Low.ToString()) / userResults.Count() * 100), 1);

                ApiResponse<UserResultStatistic> response = new ApiResponse<UserResultStatistic>();

                response.Data = statistic;

                return Json(response);
            }
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't get statistic";
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
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();

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

                UserResultResponse _userResult = new UserResultResponse();

                if (isAuthenticated)
                {
                    UserResponse _user = new UserResponse();
                    var user = await userService.GetById((int)UserId);

                    _user.Id = user.Id;
                    _user.IsAdmin = user.IsAdmin;
                    _user.Login = user.Login;
                    _user.Email = user.Email;
                    _user.Birthdate = user.Birthdate;
                    _user.IsMan = user.IsMan;

                    _userResult.Id = userResult.Id;
                    _userResult.User = _user;
                }
                
                _userResult.Date = userResult.Date;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: PTypeNames.Realistic.ToString(), value: userResult.R, power: PTypePowerConvertor.Convert(userResult.R)));
                _userResult.Results.Add(new PType(name: PTypeNames.Investigative.ToString(), value: userResult.I, power: PTypePowerConvertor.Convert(userResult.I)));
                _userResult.Results.Add(new PType(name: PTypeNames.Artistic.ToString(), value: userResult.A, power: PTypePowerConvertor.Convert(userResult.A)));
                _userResult.Results.Add(new PType(name: PTypeNames.Social.ToString(), value: userResult.S, power: PTypePowerConvertor.Convert(userResult.S)));
                _userResult.Results.Add(new PType(name: PTypeNames.Enterprising.ToString(), value: userResult.E, power: PTypePowerConvertor.Convert(userResult.E)));
                _userResult.Results.Add(new PType(name: PTypeNames.Conventional.ToString(), value: userResult.C, power: PTypePowerConvertor.Convert(userResult.C)));

                response.Data = _userResult;

                return Json(response);
            }
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't generate result";
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
                
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                
                if (userResult == null)
                {
                    response.ErrorMessage = "Causes problems while creating result";
                    return Json(response);
                }

                UserResponse _user = new UserResponse();
                var user = await userService.GetById(userResult.UserId);

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                UserResultResponse _userResult = new UserResultResponse();
                
                _userResult.Id = userResult.Id;
                _userResult.User = _user;
                _userResult.Date = userResult.Date;
                
                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: PTypeNames.Realistic.ToString(), value: userResult.R, power: PTypePowerConvertor.Convert(userResult.R)));
                _userResult.Results.Add(new PType(name: PTypeNames.Investigative.ToString(), value: userResult.I, power: PTypePowerConvertor.Convert(userResult.I)));
                _userResult.Results.Add(new PType(name: PTypeNames.Artistic.ToString(), value: userResult.A, power: PTypePowerConvertor.Convert(userResult.A)));
                _userResult.Results.Add(new PType(name: PTypeNames.Social.ToString(), value: userResult.S, power: PTypePowerConvertor.Convert(userResult.S)));
                _userResult.Results.Add(new PType(name: PTypeNames.Enterprising.ToString(), value: userResult.E, power: PTypePowerConvertor.Convert(userResult.E)));
                _userResult.Results.Add(new PType(name: PTypeNames.Conventional.ToString(), value: userResult.C, power: PTypePowerConvertor.Convert(userResult.C)));

                response.Data = _userResult;

                return Json(response);
            }
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't create user's result";
                return Json(response);
            }
        }

        /// <summary>
        /// Update test's result.
        /// Only admin is allowed
        /// </summary>
        /// <param name="userResultUpdate"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update([FromBody] UserResultUpdate userResultUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                        return BadRequest();

                var userResult = await userResultService.Update(userResultUpdate.Id,
                                                        userResultUpdate.R, userResultUpdate.I, userResultUpdate.A, userResultUpdate.S, userResultUpdate.E, userResultUpdate.C);
                
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();

                if (userResult == null)
                {
                    response.ErrorMessage = "Invalid identity public key";
                    return Json(response);
                }

                UserResponse _user = new UserResponse();
                var user = await userService.GetById(userResult.UserId);

                _user.Id = user.Id;
                _user.IsAdmin = user.IsAdmin;
                _user.Login = user.Login;
                _user.Email = user.Email;
                _user.Birthdate = user.Birthdate;
                _user.IsMan = user.IsMan;

                UserResultResponse _userResult = new UserResultResponse();

                _userResult.Id = userResult.Id;
                _userResult.User = _user;
                _userResult.Date = userResult.Date;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: PTypeNames.Realistic.ToString(), value: userResult.R, power: PTypePowerConvertor.Convert(userResult.R)));
                _userResult.Results.Add(new PType(name: PTypeNames.Investigative.ToString(), value: userResult.I, power: PTypePowerConvertor.Convert(userResult.I)));
                _userResult.Results.Add(new PType(name: PTypeNames.Artistic.ToString(), value: userResult.A, power: PTypePowerConvertor.Convert(userResult.A)));
                _userResult.Results.Add(new PType(name: PTypeNames.Social.ToString(), value: userResult.S, power: PTypePowerConvertor.Convert(userResult.S)));
                _userResult.Results.Add(new PType(name: PTypeNames.Enterprising.ToString(), value: userResult.E, power: PTypePowerConvertor.Convert(userResult.E)));
                _userResult.Results.Add(new PType(name: PTypeNames.Conventional.ToString(), value: userResult.C, power: PTypePowerConvertor.Convert(userResult.C)));

                response.Data = _userResult;

                return Json(response);
            }
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't update user's result";
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
            catch
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = "Couldn't delete user's result";
                return Json(response);
            }
        }
    }
}
