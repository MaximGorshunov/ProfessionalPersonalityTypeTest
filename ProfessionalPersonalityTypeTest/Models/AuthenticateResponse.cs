using Models;
using System;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsMan { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            IsAdmin = user.IsAdmin;
            Login = user.Login;
            Email = user.Email;
            Birthdate = user.Birthdate;
            IsMan = user.IsMan;
            Token = token;
        }
    }
}
