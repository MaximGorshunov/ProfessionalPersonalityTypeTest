namespace ProfessionalPersonalityTypeTest.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        public int Status { get; set; }

        public string ErrorMessage { get; set; }

        public bool Success => Data != null;
    }
}
