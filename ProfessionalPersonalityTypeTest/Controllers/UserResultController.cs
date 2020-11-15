﻿using Microsoft.AspNetCore.Authorization;
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
                _userResult.UserId = userResult.UserId;
                _userResult.User = _user;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: "Realistic", value: userResult.R));
                _userResult.Results.Add(new PType(name: "Investigative", value: userResult.I));
                _userResult.Results.Add(new PType(name: "Artistic", value: userResult.A));
                _userResult.Results.Add(new PType(name: "Social", value: userResult.S));
                _userResult.Results.Add(new PType(name: "Enterprising", value: userResult.E));
                _userResult.Results.Add(new PType(name: "Conventional", value: userResult.C));

                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = $"Couldn't get user's result {ex.Message}";
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

                var _userResults = userResults.Select(u => new UserResultResponse
                {
                    Id = u.Id,
                    Date = u.Date,
                    UserId = u.UserId,
                    R = u.R,
                    I = u.I,
                    A = u.A,
                    S = u.S,
                    E = u.E,
                    C = u.C
                }).ToList();

                foreach(var ur in _userResults)
                {
                    var user = await userService.GetById((int)ur.UserId);

                    UserResponse _user = new UserResponse();

                    _user.Id = user.Id;
                    _user.IsAdmin = user.IsAdmin;
                    _user.Login = user.Login;
                    _user.Email = user.Email;
                    _user.Birthdate = user.Birthdate;
                    _user.IsMan = user.IsMan;

                    ur.User = _user;

                    ur.Results = new List<PType>();

                    ur.Results.Add(new PType(name: "Realistic", value: ur.R));
                    ur.Results.Add(new PType(name: "Investigative", value: ur.I));
                    ur.Results.Add(new PType(name: "Artistic", value: ur.A));
                    ur.Results.Add(new PType(name: "Social", value: ur.S));
                    ur.Results.Add(new PType(name: "Enterprising", value: ur.E));
                    ur.Results.Add(new PType(name: "Conventional", value: ur.C));
                }

                ApiResponse<List<UserResultResponse>> response = new ApiResponse<List<UserResultResponse>>();

                response.Data = _userResults;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = $"Couldn't get user's results : {ex.Message}";
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
                    _userResult.UserId = userResult.UserId;
                    _userResult.User = _user;
                }
                _userResult.Date = userResult.Date;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: "Realistic", value: userResult.R));
                _userResult.Results.Add(new PType(name: "Investigative", value: userResult.I));
                _userResult.Results.Add(new PType(name: "Artistic", value: userResult.A));
                _userResult.Results.Add(new PType(name: "Social", value: userResult.S));
                _userResult.Results.Add(new PType(name: "Enterprising", value: userResult.E));
                _userResult.Results.Add(new PType(name: "Conventional", value: userResult.C));

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
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
                _userResult.UserId = userResult.UserId;
                _userResult.User = _user;
                _userResult.Date = userResult.Date;
                
                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: "Realistic", value: userResult.R));
                _userResult.Results.Add(new PType(name: "Investigative", value: userResult.I));
                _userResult.Results.Add(new PType(name: "Artistic", value: userResult.A));
                _userResult.Results.Add(new PType(name: "Social", value: userResult.S));
                _userResult.Results.Add(new PType(name: "Enterprising", value: userResult.E));
                _userResult.Results.Add(new PType(name: "Conventional", value: userResult.C));

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
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
                _userResult.UserId = userResult.UserId;
                _userResult.User = _user;
                _userResult.Date = userResult.Date;

                _userResult.Results = new List<PType>();

                _userResult.Results.Add(new PType(name: "Realistic", value: userResult.R));
                _userResult.Results.Add(new PType(name: "Investigative", value: userResult.I));
                _userResult.Results.Add(new PType(name: "Artistic", value: userResult.A));
                _userResult.Results.Add(new PType(name: "Social", value: userResult.S));
                _userResult.Results.Add(new PType(name: "Enterprising", value: userResult.E));
                _userResult.Results.Add(new PType(name: "Conventional", value: userResult.C));

                response.Data = _userResult;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
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
                ApiResponse<UserResultResponse> response = new ApiResponse<UserResultResponse>();
                response.ErrorMessage = $"Couldn't delete user's result : {ex.Message}";
                return Json(response);
            }
        }
    }
}
