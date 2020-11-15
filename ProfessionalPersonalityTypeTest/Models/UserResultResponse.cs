using System;
using System.Collections.Generic;
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
        [JsonIgnore]
        public int R { get; set; }
        [JsonIgnore]
        public int I { get; set; }
        [JsonIgnore]
        public int A { get; set; }
        [JsonIgnore]
        public int S { get; set; }
        [JsonIgnore]
        public int E { get; set; }
        [JsonIgnore]
        public int C { get; set; }
        public List<PType> Results { get; set; }
    }
}
