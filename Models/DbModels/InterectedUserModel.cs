﻿using System.ComponentModel.DataAnnotations;

namespace EvaSystem.Models
{
    public class InterectedUserModel
    {
        [Key]
        public string EntryHash { get; set; } //костыль!

        public string UserName { get; set; }

        public string InterectedUserName { get; set; }

    }
}
