using System.ComponentModel.DataAnnotations;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangeEmailRequest
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
