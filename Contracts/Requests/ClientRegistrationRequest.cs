using System.ComponentModel.DataAnnotations;

namespace EvaSystem.Contracts.Requests
{
    public class ClientRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MinLength(2,ErrorMessage ="First name lenght should be more then 1 chars")]
        [MaxLength(50,ErrorMessage = "Firs name lenght should be less then 50 char")]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "Middle name lenght should be more then 1 chars")]
        [MaxLength(50, ErrorMessage = "Middle name lenght should be less then 50 char")]
        public string MiddleName { get; set; }

        [MinLength(2, ErrorMessage = "Last name lenght should be more then 1 chars")]
        [MaxLength(50, ErrorMessage = "Last name lenght should be less then 50 char")]
        public string LastName { get; set; }

        [MinLength(2, ErrorMessage ="Position length should be more then 1 chars")]
        public string Position { get; set; }
    }
}
