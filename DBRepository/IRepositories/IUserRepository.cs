using Models;
using System.Threading.Tasks;

namespace DBRepository.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> CreateUser(User user);
        Task<int> DeleteUser(int userId);
        Task<int> DeleteUser(User user);
        Task<User> UpdateUser(User user);
    }
}
