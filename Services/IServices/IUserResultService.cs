using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IUserResultService
    {
        Task<List<UserResult>> GetAll();
        Task<UserResult> GetById(int id);
        Task<int> Delete(int id);
        Task<UserResult> Create(int userId, int r, int i, int a, int s, int e, int c);
        Task<UserResult> Update(int id, int r, int i, int a, int s, int e, int c);
    }
}
