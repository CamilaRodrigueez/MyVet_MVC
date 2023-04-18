using System;
using System.Collections.Generic;
using System.Text;

namespace MyVetDomain.Dto.RestServices
{
    public class TokenDto
    {
        public string Token { get; set; }
        public double Expiration { get; set; }
    }
}
