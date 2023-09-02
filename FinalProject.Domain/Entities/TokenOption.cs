using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Entities
{
    public class TokenOption
    {
        public string Issuer { get; set; } 
        public string Audience { get; set; }
        public int Expiration { get; set; }
        public string SecretKey { get; set; }   
    }
}
