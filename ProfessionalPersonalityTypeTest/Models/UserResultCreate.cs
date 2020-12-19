using System;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPersonalityTypeTest.Models
{
    /// <summary>
    /// Request model for creating user's test result
    /// </summary>
    public class UserResultCreate
    {
        private int r;
        private int i;
        private int a;
        private int s;
        private int e;
        private int c;

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
        public int R
        {
            get
            {
                return r;
            }
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentException("Value cannot be more then 10 or less then 0", nameof(value));
                r = value;
            }
        }
        /// <summary>
        /// Investigative type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int I
        {
            get
            {
                return i;
            }
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentException("Value cannot be more then 10 or less then 0", nameof(value));
                i = value;
            }
        }
        /// <summary>
        /// Artistic type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int A
        {
            get
            {
                return a;
            }
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentException("Value cannot be more then 10 or less then 0", nameof(value));
                a = value;
            }
        }
        /// <summary>
        /// Social type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int S
        {
            get
            {
                return s;
            }
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentException("Value cannot be more then 10 or less then 0", nameof(value));
                s = value;
            }
        }
        /// <summary>
        /// Enterprising type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int E
        {
            get
            {
                return e;
            }
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentException("Value cannot be more then 10 or less then 0", nameof(value));
                e = value;
            }
        }
        /// <summary>
        /// Conventional type
        /// Сontains the sum of the selected professions of the given type
        /// </summary>
        [Required]
        public int C
        {
            get
            {
                return c;
            }
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentException("Value cannot be more then 10 or less then 0", nameof(value));
                c = value;
            }
        }
    }
}