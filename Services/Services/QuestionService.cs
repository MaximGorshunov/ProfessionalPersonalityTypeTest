using DBRepository.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository questionRepository;

        public QuestionService(IQuestionRepository _questionRepository)
        {
            questionRepository = _questionRepository;
        }

        public async Task<Question> GetById(int id)
        {
            return await questionRepository.GetById(id);
        }

        public async Task<List<Question>> GetAll()
        {
            return await questionRepository.GetAll().ToListAsync();
        }
    }
}
