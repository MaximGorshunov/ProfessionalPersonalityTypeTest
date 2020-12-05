using Service.Helpers;

namespace ProfessionalPersonalityTypeTest.Models
{
    /// <summary>
    /// Request model for getting list of users
    /// </summary>
    public class UserGetAllRequest
    {
        /// <summary>
        /// Male = 1
        /// Female = 2
        /// None = Null
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// Filter by user's login
        /// </summary>
        public string LoginFilter { get; set; }
        /// <summary>
        /// Filter by user's email
        /// </summary>
        public string EmailFilter { get; set; }
        /// <summary>
        /// Filter by user's min age
        /// </summary>
        public int? AgeMin { get; set; }
        /// <summary>
        /// Filter by user's max age
        /// </summary>
        public int? AgeMax { get; set; }
        /// <summary>
        /// User = 1
        /// Admin = 2
        /// None = Null
        /// </summary>
        public Role? Role { get; set; }
    }
}
