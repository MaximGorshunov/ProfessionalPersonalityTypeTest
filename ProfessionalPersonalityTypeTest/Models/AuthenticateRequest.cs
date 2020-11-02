﻿using System.ComponentModel.DataAnnotations;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}