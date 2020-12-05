using System.Collections.Generic;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public ICollection<ProfessionResponse> professions { get; set; }
    }
}
