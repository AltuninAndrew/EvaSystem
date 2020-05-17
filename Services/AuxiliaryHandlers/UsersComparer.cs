using EvaSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Services.AuxiliaryHandlers
{
    public class UsersComparer : IEqualityComparer<ResponseUserModel>
    {

        public bool Equals([AllowNull] ResponseUserModel x, [AllowNull] ResponseUserModel y)
        {
            return x.Username == y.Username;
        }

        public int GetHashCode([DisallowNull] ResponseUserModel obj)
        {
           return obj.Username.GetHashCode();
        }
    }
}
