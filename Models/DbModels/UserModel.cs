using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaSystem.Models
{
    public class UserModel:IdentityUser
    {
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public int PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public PositionModel Position { get; set; }
    }
}
