using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAll();
        Task<Question> GetById(int id);
    }
}
