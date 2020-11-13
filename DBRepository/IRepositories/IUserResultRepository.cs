using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBRepository.IRepositories
{
    public interface IUserResultRepository : IBaseRepository<UserResult>
    {
        Task<List<UserResult>> GetByFilters(DateTime? dataMin, DateTime? dataMax, int? ageMin, int? ageMax, bool? isMan, string loginFilter, bool actual);
        Task<UserResult> CreateUserResult(UserResult userResult);
        Task<int> DeleteUserResult(int userResultId);
        Task<int> DeleteUserResult(UserResult userResult);
        Task<UserResult> UpdateUserResult(UserResult userResult);
    }
}
