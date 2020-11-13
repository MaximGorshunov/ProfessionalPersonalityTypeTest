using System;
using System.Text.Json.Serialization;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class UserResultResponse
    {
        public int? Id { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        public UserResponse User { get; set; }
        public DateTime Date { get; set; }
        public int R { get; set; }
        public int I { get; set; }
        public int A { get; set; }
        public int S { get; set; }
        public int E { get; set; }
        public int C { get; set; }
    }
}
