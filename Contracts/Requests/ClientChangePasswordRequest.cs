using System.ComponentModel.DataAnnotations;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangePasswordRequest
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
