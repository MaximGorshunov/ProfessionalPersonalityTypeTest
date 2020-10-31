using System.Collections;

namespace ProfessionalPersonalityTypeTest.Models
{
    public class ApiResponce<T>
    {
        public T Data { get; set; }

        public string ErrorMessage { get; set; }

        public bool Success => Data != null;
    }
}
