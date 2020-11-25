namespace ProfessionalPersonalityTypeTest.Helpers
{
    public class PType
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Power { get; set; }

        public PType(string name, int value, string power)
        {
            Name = name;
            Value = value;
            Power = power;
        }
    }
}
