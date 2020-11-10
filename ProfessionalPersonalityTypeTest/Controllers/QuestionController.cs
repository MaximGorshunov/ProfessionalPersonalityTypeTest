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
                ApiResponse<QuestionGet> response = new ApiResponse<QuestionGet>();

                var question = await questionService.GetById(id);
                var professionFirst = await professionService.GetById(question.ProfessionIdFirst);
                var professionSecond = await professionService.GetById(question.ProfessionIdSecond);

                ProfessionGet professionGetFirst = new ProfessionGet();

                professionGetFirst.Id = professionFirst.Id;
                professionGetFirst.Name = professionFirst.Name;
                professionGetFirst.Type = professionFirst.ProfType.ToString();

                ProfessionGet professionGetSecond = new ProfessionGet();

                professionGetSecond.Id = professionSecond.Id;
                professionGetSecond.Name = professionSecond.Name;
                professionGetSecond.Type = professionSecond.ProfType.ToString();

                QuestionGet questionGet = new QuestionGet();

                questionGet.Id = question.Id;
                questionGet.Number = question.Number;

                questionGet.professions = new Collection<ProfessionGet>();

                questionGet.professions.Add(professionGetFirst);
                questionGet.professions.Add(professionGetSecond);

                response.Data = questionGet;

                return Json(response);
            }
            catch (Exception ex)
            {
                ApiResponse<QuestionGet> response = new ApiResponse<QuestionGet>();
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
                ApiResponse<List<QuestionGet>> responce = new ApiResponse<List<QuestionGet>>();
                List<QuestionGet> questions = new List<QuestionGet>();
                
                var _questions = await questionService.GetAll();
                
                foreach(var q in _questions)
                {
                    var professionFirst = await professionService.GetById(q.ProfessionIdFirst);
                    var professionSecond = await professionService.GetById(q.ProfessionIdSecond);

                    ProfessionGet professionGetFirst = new ProfessionGet();

                    professionGetFirst.Id = professionFirst.Id;
                    professionGetFirst.Name = professionFirst.Name;
                    professionGetFirst.Type = professionFirst.ProfType.ToString();

                    ProfessionGet professionGetSecond = new ProfessionGet();

                    professionGetSecond.Id = professionSecond.Id;
                    professionGetSecond.Name = professionSecond.Name;
                    professionGetSecond.Type = professionSecond.ProfType.ToString();

                    QuestionGet questionGet = new QuestionGet();

                    questionGet.Id = q.Id;
                    questionGet.Number = q.Number;

                    questionGet.professions = new Collection<ProfessionGet>();

                    questionGet.professions.Add(professionGetFirst);
                    questionGet.professions.Add(professionGetSecond);

                    questions.Add(questionGet);
                }

                responce.Data = questions;

                return Json(responce);
            }
            catch (Exception ex) 
            {
                ApiResponse<QuestionGet> response = new ApiResponse<QuestionGet>();
                response.ErrorMessage = $"Couldn't get questions  : {ex.Message}";
                return Json(response);
            }
        }
    }
}
