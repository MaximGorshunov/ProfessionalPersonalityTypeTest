using System.ComponentModel.DataAnnotations;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class AuthenticateRequest
    {
        /// <summary>
        /// User's login name
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
