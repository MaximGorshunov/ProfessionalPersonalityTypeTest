using Models;
using System.Threading.Tasks;

namespace DBRepository.IRepositories
{
    public interface IUserResultRepository : IBaseRepository<UserResult>
    {
        Task<UserResult> CreateUserResult(UserResult userResult);
        Task<int> DeleteUserResult(int userResultId);
        Task<int> DeleteUserResult(UserResult userResult);
        Task<UserResult> UpdateUserResult(UserResult userResult);
    }
}
