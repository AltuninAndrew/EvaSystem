using System.ComponentModel.DataAnnotations;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangeEmailRequest
    {
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
