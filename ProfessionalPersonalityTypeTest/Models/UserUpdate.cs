using System;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class UserUpdate
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsMan { get; set; }
        public string Password { get; set; }
    }
}
