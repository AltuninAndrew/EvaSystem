using Microsoft.AspNetCore.Identity;

namespace EvaSystem.Models
{
    public class UserModel:IdentityUser
    {
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
    }
}
