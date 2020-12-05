namespace ProfessionalPersonalityTypeTest.Helpers
{
    public static class PTypePowerConvertor
    {
        private const int LowFrom = 2;
        private const int LowTo = 4;
        private const int MiddleFrom = 5;
        private const int MiddleTo = 7;
        private const int HighFrom = 8;
        private const int HighTo = 10;
        
        public static string Convert(int value)
        {
            if (value >= LowFrom && value <= LowTo) return PTypePowers.Low.ToString();
            if (value >= MiddleFrom && value <= MiddleTo) return PTypePowers.Middle.ToString();
            if (value >= HighFrom && value <= HighTo) return PTypePowers.High.ToString();

            return null;
        }
    }
}
