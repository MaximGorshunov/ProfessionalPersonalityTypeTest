using ProfessionalPersonalityTypeTest.Helpers;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class PType
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Power { get; set; }

        public PType(string name, int value)
        {
            Name = name;
            Value = value;

            if (value >= 2 && value <= 4) Power = PTypePowers.Low;
            if (value >= 5 && value <= 7) Power = PTypePowers.Middle;
            if (value >= 8 && value <= 10) Power = PTypePowers.High;
        }
    }
}
