using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts
{
    public static class ApiRoutes
    {

        public static class Test
        {
            public const string Value = "api/test";
        }

        public static class Identity
        {
            public const string Login = "api/identity/login";
            public const string RegisterAdmin = "api/identity/admin_register";
            public const string RegisterClient = "api/identity/client_register";
           
        }

        public static class ClientData
        {
            public const string ChangePassword = "api/ClientData/change_password/{username}";
        }
    
    }
}
