using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IProfessionService
    {
        Task<List<Profession>> GetAll();
        Task<Profession> GetById(int id);
    }
}
