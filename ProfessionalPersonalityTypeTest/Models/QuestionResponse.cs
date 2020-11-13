using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public ICollection<ProfessionResponse> professions { get; set; }
    }
}
