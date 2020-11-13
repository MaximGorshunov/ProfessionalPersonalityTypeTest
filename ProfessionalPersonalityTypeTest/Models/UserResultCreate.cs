using System.ComponentModel.DataAnnotations;

namespace ProfessionalPersonalityTypeTest.Models
{
    /// <summary>
    /// Request model for creating user's test result
    /// </summary>
    public class UserResultCreate
    {
        /// <summary>
        /// User's id 
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Realistic type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int R { get; set; }
        /// <summary>
        /// Investigative type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int I { get; set; }
        /// <summary>
        /// Artistic type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int A { get; set; }
        /// <summary>
        /// Social type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int S { get; set; }
        /// <summary>
        /// Enterprising type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int E { get; set; }
        /// <summary>
        /// Conventional type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int C { get; set; }
    }
}