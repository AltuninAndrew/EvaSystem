using System.ComponentModel.DataAnnotations;

namespace EvaSystem.Contracts.Requests
{
    public class ClientLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
