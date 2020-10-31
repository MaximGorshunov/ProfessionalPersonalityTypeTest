using System;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> GetById(int id);
        Task<int> Delete(int id);
        Task<User> Create(bool isAdmin, string login, string email, DateTime birthdate, bool isMan, string password);
        Task<User> Update(int id, bool isAdmin, string login, string email, DateTime birthdate, bool isMan, string password);
    }
}
