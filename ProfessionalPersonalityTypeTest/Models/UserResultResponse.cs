using ProfessionalPersonalityTypeTest.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class UserResultResponse
    {
        public int? Id { get; set; }
        public UserResponse User { get; set; }
        public DateTime Date { get; set; }
        public List<PType> Results { get; set; }
    }
}
