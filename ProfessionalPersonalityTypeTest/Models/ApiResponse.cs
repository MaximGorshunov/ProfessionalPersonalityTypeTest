namespace ProfessionalPersonalityTypeTest.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        public string ErrorMessage { get; set; }

        public bool Success => Data != null;
    }
}
