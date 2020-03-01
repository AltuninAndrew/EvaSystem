using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EvaSystem.Options
{
    public class JwtSettings
    {
        public string Secret { get; private set; } 

        public JwtSettings()
        {

            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] data = new byte[32];
            rng.GetBytes(data);
            Secret = Convert.ToBase64String(data);

            //var hmac = new HMACSHA1();
            //Secret = Convert.ToBase64String(hmac.Key);
        }

    }
}
