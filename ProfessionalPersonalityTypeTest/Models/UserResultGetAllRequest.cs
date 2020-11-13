using System;
using Service.Helpers;

namespace ProfessionalPersonalityTypeTest.Models
{
    /// <summary>
    /// Request model for getting list of user's results
    /// </summary>
    public class UserResultGetAllRequest
    {
        /// <summary>
        /// Male = 1
        /// Female = 2
        /// None = null
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// Filter by user's login
        /// </summary>
        public string LoginFilter { get; set; }
        /// <summary>
        /// Filter by user's min age
        /// </summary>
        public int? AgeMin { get; set; }
        /// <summary>
        /// Filter by user's max age
        /// </summary>
        public int? AgeMax { get; set; }
        /// <summary>
        /// Filter by time period
        /// </summary>
        public DateTime? DataMin { get; set; }
        /// <summary>
        /// Filter by time period
        /// </summary>
        public DateTime? DataMax { get; set; }
        /// <summary>
        /// If true get only actual results
        /// </summary>
        public bool Actual { get; set; }
    }
}
