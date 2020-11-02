using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class QuestionGet
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public ICollection<ProfessionGet> professions { get; set; }
    }
}
