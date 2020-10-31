using System;
using System.Collections.Generic;

namespace Models
{
    public class Profession : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProfType ProfType { get; set; }

        public ICollection<Question> QuestionFirst { get; set; }
        public ICollection<Question> QuestionSecond { get; set; }
    }

    public enum ProfType
    {
        R = 1,
        I = 2,
        A = 3,
        S = 4,
        E = 5,
        C = 6
    }
}
