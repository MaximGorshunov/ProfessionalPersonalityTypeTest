namespace ProfessionalPersonalityTypeTest.Models
{
    public class UserResultStatistic
    {
        public Statistic High { get; set; }
        public Statistic Middle { get; set; }
        public Statistic Low { get; set; }
    }

    public class Statistic
    {
        public double Realistic { get; set; }
        public double Investigative { get; set; }
        public double Artistic { get; set; }
        public double Social { get; set; }
        public double Enterprising { get; set; }
        public double Conventional { get; set; }
    }
}
