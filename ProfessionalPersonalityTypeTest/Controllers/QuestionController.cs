using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPersonalityTypeTest.Models;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPersonalityTypeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IProfessionService professionService;

        public QuestionController(IQuestionService _questionService, IProfessionService _professionService)
        {
            questionService = _questionService;
            professionService = _professionService;
        }

        /// <summary>
        /// Find question by identity key.
        /// All allowed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                ApiResponse<QuestionResponse> response = new ApiResponse<QuestionResponse>();

                var question = await questionService.GetById(id);
                var professionFirst = await professionService.GetById(question.ProfessionIdFirst);
                var professionSecond = await professionService.GetById(question.ProfessionIdSecond);

                ProfessionResponse professionGetFirst = new ProfessionResponse();

                professionGetFirst.Id = professionFirst.Id;
                professionGetFirst.Name = professionFirst.Name;
                professionGetFirst.Type = professionFirst.ProfType.ToString();

                ProfessionResponse professionGetSecond = new ProfessionResponse();

                professionGetSecond.Id = professionSecond.Id;
                professionGetSecond.Name = professionSecond.Name;
                professionGetSecond.Type = professionSecond.ProfType.ToString();

                QuestionResponse questionGet = new QuestionResponse();

                questionGet.Id = question.Id;
                questionGet.Number = question.Number;

                questionGet.professions = new Collection<ProfessionResponse>();

                questionGet.professions.Add(professionGetFirst);
                questionGet.professions.Add(professionGetSecond);

                response.Data = questionGet;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<QuestionResponse> response = new ApiResponse<QuestionResponse>();
                response.ErrorMessage = $"Couldn't get question {ex.Message}";
                return Json(response);
            }
        }

        /// <summary>
        /// Get all questions.
        /// All allowed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ApiResponse<List<QuestionResponse>> responce = new ApiResponse<List<QuestionResponse>>();
                List<QuestionResponse> questions = new List<QuestionResponse>();
                
                var _questions = await questionService.GetAll();
                
                foreach(var q in _questions)
                {
                    var professionFirst = await professionService.GetById(q.ProfessionIdFirst);
                    var professionSecond = await professionService.GetById(q.ProfessionIdSecond);

                    ProfessionResponse professionGetFirst = new ProfessionResponse();

                    professionGetFirst.Id = professionFirst.Id;
                    professionGetFirst.Name = professionFirst.Name;
                    professionGetFirst.Type = professionFirst.ProfType.ToString();

                    ProfessionResponse professionGetSecond = new ProfessionResponse();

                    professionGetSecond.Id = professionSecond.Id;
                    professionGetSecond.Name = professionSecond.Name;
                    professionGetSecond.Type = professionSecond.ProfType.ToString();

                    QuestionResponse questionGet = new QuestionResponse();

                    questionGet.Id = q.Id;
                    questionGet.Number = q.Number;

                    questionGet.professions = new Collection<ProfessionResponse>();

                    questionGet.professions.Add(professionGetFirst);
                    questionGet.professions.Add(professionGetSecond);

                    questions.Add(questionGet);
                }

                responce.Data = questions;

                return Json(responce);
            }
            catch (Exception ex) 
            {
                ApiResponse<QuestionResponse> response = new ApiResponse<QuestionResponse>();
                response.ErrorMessage = $"Couldn't get questions  : {ex.Message}";
                return Json(response);
            }
        }
    }
}
