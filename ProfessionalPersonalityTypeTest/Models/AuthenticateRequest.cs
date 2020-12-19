using System.ComponentModel.DataAnnotations;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class AuthenticateRequest
    {
        /// <summary>
        /// User's login name
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
