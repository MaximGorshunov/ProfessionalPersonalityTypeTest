using Models;
using DBRepository;
using DBRepository.IRepositories;

namespace DBRepository.Repositories
{
    public class ProfessionRepository : BaseRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
    }
}
