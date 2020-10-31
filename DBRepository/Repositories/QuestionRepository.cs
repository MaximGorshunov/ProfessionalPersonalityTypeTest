using Models;
using DBRepository;
using DBRepository.IRepositories;

namespace DBRepository.Repositories
{
    public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
    }
}
