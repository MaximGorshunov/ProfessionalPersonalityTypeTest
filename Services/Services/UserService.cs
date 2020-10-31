using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.IServices;
using Models;
using DBRepository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<List<User>> GetAll()
        {
            return await userRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await userRepository.GetById(id);
        }
        
        public async Task<int> Delete(int id)
        {
            return await userRepository.DeleteUser(id);
        }

        public async Task<User> Create(bool isAdmin, string login, string email, DateTime birthdate, bool isMan, string password)
        {
            User newUser = new User();
            var _user = await userRepository.GetAll()
                                       .Where(u => u.Login == login || u.Email == email)
                                       .AnyAsync();
            if(!_user)
            {
                newUser.IsAdmin = isAdmin;
                newUser.Login = login;
                newUser.Email = email;
                newUser.IsMan = isAdmin;
                newUser.Birthdate = birthdate;
                newUser.Password = password;

                newUser = await userRepository.CreateUser(newUser);
            }
            return newUser;
        }

        public async Task<User> Update(int id, bool isAdmin, string login, string email, DateTime birthdate, bool isMan, string password)
        {
            User updatedUser = await userRepository.GetById(id);
            var loginCheck = false;
            var emailCheck = false;

            if(updatedUser.Login != login)
            {
                loginCheck = await userRepository.GetAll()
                                       .Where(u => u.Login == login)
                                       .AnyAsync();
            }

            if(updatedUser.Email != email)
            {
                emailCheck = await userRepository.GetAll()
                                       .Where(u => u.Email == email)
                                       .AnyAsync();
            }

            if (!loginCheck && !emailCheck && updatedUser != null)
            {

                updatedUser.Id = id;
                updatedUser.IsAdmin = isAdmin;
                updatedUser.Login = login;
                updatedUser.Email = email;
                updatedUser.IsMan = isMan;
                updatedUser.Birthdate = birthdate;
                updatedUser.Password = password;

                updatedUser = await userRepository.UpdateUser(updatedUser);
            }
            return updatedUser;
        }
    }
}
