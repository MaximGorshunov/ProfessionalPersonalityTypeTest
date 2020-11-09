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

        public async Task<UserResult> Create(int? userId, List<Profession> professions) 
        {
            UserResult newUserResult = new UserResult();

            int r = 0; int i = 0; int a = 0; int s = 0; int e = 0; int c = 0;

            foreach (var p in professions)
            {
                switch (p.ProfType)
                {
                    case ProfType.R:
                        r++;
                        break;

                    case ProfType.I:
                        i++;
                        break;

                    case ProfType.A:
                        a++;
                        break;

                    case ProfType.S:
                        s++;
                        break;

                    case ProfType.E:
                        e++;
                        break;

                    case ProfType.C:
                        c++;
                        break;

                    default:
                        break;
                }
            }

            if (userId != null)
            {
                var _userResult = await userResultRepository.GetAll()
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (_userResult == null)
                {
                    newUserResult.UserId = (int)userId;
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
                else
                {
                    return await Update(_userResult.Id, r, i, a, s, e, c);
                }
            }
            else
            {
                newUserResult.Date = DateTime.UtcNow;
                newUserResult.R = r;
                newUserResult.I = i;
                newUserResult.A = a;
                newUserResult.S = s;
                newUserResult.E = e;
                newUserResult.C = c;

                return newUserResult;
            }
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
