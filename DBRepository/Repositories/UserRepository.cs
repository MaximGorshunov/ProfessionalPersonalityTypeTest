using System;
using Models;
using System.Threading.Tasks;
using DBRepository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DBRepository.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                var newUser = dbSet.Add(user);
                await repositoryContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<int> DeleteUser(int id)
        {
            try
            {
                var entityToDelete = dbSet.Find(id);
                return await DeleteUser(entityToDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUser(User user)
        {
            try
            {
                if (repositoryContext.Entry(user).State == EntityState.Detached)
                {
                    dbSet.Attach(user);
                }
                dbSet.Remove(user);
                await repositoryContext.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var newUser = dbSet.Update(user);
                await repositoryContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
    }
}
