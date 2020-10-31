using DBRepository.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProfessionService : IProfessionService
    {
        private readonly IProfessionRepository professionRepository;

        public ProfessionService(IProfessionRepository _professionRepository)
        {
            professionRepository = _professionRepository;
        }

        public async Task<Profession> GetById(int id)
        {
            return await professionRepository.GetById(id);
        }

        public async Task<List<Profession>> GetAll()
        {
            return await professionRepository.GetAll().ToListAsync();
        }
    }
}
