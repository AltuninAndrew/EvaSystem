using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class ResponseUserModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public byte[] AvatarImage { get; set; }
        public string UserRole { get; set; }
        public string Username { get; set; }
    }
}
