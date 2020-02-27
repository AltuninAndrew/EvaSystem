﻿using System;
using System.Collections.Generic;
using System.Text;
using EvaSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvaSystem.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

    }
}
