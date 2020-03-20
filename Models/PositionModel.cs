using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class PositionModel
    {
        [Key]
        public int PositionId { get; set; }
            
        public string PositionName { get; set; }

        public ICollection<UserModel> Users { get; set; }
    }
}
