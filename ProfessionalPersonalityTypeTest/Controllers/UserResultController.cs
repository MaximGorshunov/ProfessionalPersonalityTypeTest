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

        public UserResultController(IUserResultService _userResultService)
        {
            userResultService = _userResultService;
        }

        /// <summary>
        /// Find user's test's result by identity key.
        /// Admin can find any result.
        /// User can find only his result.
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
                response.ErrorMessage = "Couldn't get user result : " + ex.Message;
                return Json(response);
            }
        }

        /// <summary>
        /// Get all user's test's results.
        /// Only admin is allowed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
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
                response.ErrorMessage = "Couldn't get users results : " + ex.Message;
                return Json(response);
            }
        }

        /// <summary>
        /// Add new user's test result.
        /// Admin can add test's result for any user.
        /// User can add test's result only for himself.
        /// </summary>
        /// <param name="userResultCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] UserResultCreate userResultCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var currentUserId = int.Parse(User.Identity.Name);
                if (userResultCreate.UserId != currentUserId && !User.IsInRole(Roles.Admin))
                    return Forbid();

                var userResult = await userResultService.Create(userResultCreate.UserId, 
                                                          userResultCreate.R, userResultCreate.I, userResultCreate.A, userResultCreate.S, userResultCreate.E, userResultCreate.C);

                ApiResponse<UserResultGet> response = new ApiResponse<UserResultGet>();

                if (userResult == null)
                {
                    response.ErrorMessage = "Invalid user identity public key";
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
                response.ErrorMessage = "Couldn't create user result : " + ex.Message;
                return Json(response);
            }
        }

        /// <summary>
        /// Update user's test result.
        /// Admin can update any test's result.
        /// User can update only his test's result.
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
                response.ErrorMessage = "Couldn't update user result : " + ex.Message;
                return Json(response);
            }
        }

        /// <summary>
        /// Delete user's test's result.
        /// Admin can delete any test's result.
        /// User can delete only his test's result.
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
                response.ErrorMessage = "Couldn't delete user result : " + ex.Message;
                return Json(response);
            }
        }
    }
}
