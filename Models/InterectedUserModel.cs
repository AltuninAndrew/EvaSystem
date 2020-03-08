using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class InterectedUserModel
    {
        [Key]
        public string UserName { get; set; }

        [ForeignKey("UserName")]
        public string InterectedUserName { get; set; }


    }
}
