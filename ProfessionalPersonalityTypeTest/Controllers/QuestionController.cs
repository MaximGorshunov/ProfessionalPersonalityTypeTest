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

        [HttpGet("get")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                ApiResponce<QuestionGet> responce = new ApiResponce<QuestionGet>();

                var question = await questionService.GetById(id);
                var professionFirst = await professionService.GetById(question.ProfessionIdFirst);
                var professionSecond = await professionService.GetById(question.ProfessionIdSecond);

                ProfessionGet professionGetFirst = new ProfessionGet();

                professionGetFirst.Name = professionFirst.Name;
                professionGetFirst.Type = professionFirst.ProfType.ToString();

                ProfessionGet professionGetSecond = new ProfessionGet();

                professionGetSecond.Name = professionSecond.Name;
                professionGetSecond.Type = professionSecond.ProfType.ToString();

                QuestionGet questionGet = new QuestionGet();

                questionGet.Id = question.Id;

                questionGet.professions = new Collection<ProfessionGet>();

                questionGet.professions.Add(professionGetFirst);
                questionGet.professions.Add(professionGetSecond);

                responce.Data = questionGet;

                return Json(responce);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ApiResponce<List<QuestionGet>> responce = new ApiResponce<List<QuestionGet>>();
                List<QuestionGet> questions = new List<QuestionGet>();
                
                var _questions = await questionService.GetAll();
                
                foreach(var q in _questions)
                {
                    var professionFirst = await professionService.GetById(q.ProfessionIdFirst);
                    var professionSecond = await professionService.GetById(q.ProfessionIdSecond);

                    ProfessionGet professionGetFirst = new ProfessionGet();

                    professionGetFirst.Name = professionFirst.Name;
                    professionGetFirst.Type = professionFirst.ProfType.ToString();

                    ProfessionGet professionGetSecond = new ProfessionGet();

                    professionGetSecond.Name = professionSecond.Name;
                    professionGetSecond.Type = professionSecond.ProfType.ToString();

                    QuestionGet questionGet = new QuestionGet();

                    questionGet.Id = q.Id;

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
                throw new Exception(ex.Message);
            }
        }
    }
}
