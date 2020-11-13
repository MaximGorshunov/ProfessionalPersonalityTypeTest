using Models;
using System.Linq;
using System.Threading.Tasks;

namespace DBRepository.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IModel 
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetById(int id);
    }
}
