using System.Collections.Generic;

namespace EvaSystem.Models
{
    public class ChangedUserAvatarResulModel
    {
        public bool Success { get; set; }
        public byte[] NewAvatarImage { get; set; }
        public IEnumerable<string> ErrorsMessages { get; set; }
    }
}
