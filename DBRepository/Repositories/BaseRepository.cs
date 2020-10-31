using System;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBRepository.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IModel
    {
        protected readonly RepositoryContext repositoryContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(RepositoryContext _repositoryContext)
        {
            repositoryContext = _repositoryContext;
            dbSet = repositoryContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> GetById(int id)
        {
            try
            {
                return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrive entitie: {ex.Message}");
            }
        }
    }
}
