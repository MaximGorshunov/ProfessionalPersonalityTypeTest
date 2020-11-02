using DBRepository.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserResultService : IUserResultService
    {
        private readonly IUserResultRepository userResultRepository;

        public UserResultService(IUserResultRepository _userResultRepository)
        {
            userResultRepository = _userResultRepository;
        }

        public async Task<List<UserResult>> GetAll()
        {
            return await userResultRepository.GetAll().ToListAsync(); ;
        }

        public async Task<UserResult> GetById(int id) 
        {
            return await userResultRepository.GetById(id);
        }

        public async Task<int> Delete(int id) 
        {
            return await userResultRepository.DeleteUserResult(id);
        }

        public async Task<UserResult> Create(int userId, int r, int i, int a, int s, int e, int c) 
        {
            UserResult newUserResult = new UserResult();
            var _userResult = await userResultRepository.GetAll()
                                            .Where(u => u.UserId == userId)
                                            .AnyAsync();

            if (!_userResult)
            {
                newUserResult.UserId = userId;
                newUserResult.Date = DateTime.UtcNow;
                newUserResult.R = r;
                newUserResult.I = i;
                newUserResult.A = a;
                newUserResult.S = s;
                newUserResult.E = e;
                newUserResult.C = c;

                newUserResult = await userResultRepository.CreateUserResult(newUserResult);

                return newUserResult;
            }

            return newUserResult = null;
        }

        public async Task<UserResult> Update(int id, int r, int i, int a, int s, int e, int c) 
        {
            var updatedUserResult = await userResultRepository.GetById(id);

            if (updatedUserResult != null)
            {
                updatedUserResult.Id = id;
                updatedUserResult.Date = DateTime.UtcNow;
                updatedUserResult.R = r;
                updatedUserResult.I = i;
                updatedUserResult.A = a;
                updatedUserResult.S = s;
                updatedUserResult.E = e;
                updatedUserResult.C = c;

                updatedUserResult = await userResultRepository.UpdateUserResult(updatedUserResult);

                return updatedUserResult;
            }

            return updatedUserResult = null;
        }
    }
}
