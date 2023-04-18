using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyVetDomain.Dto
{
    public class ServicesDto
    {

        public int IdServices { get; set; }
        public string Services { get; set; }
        public string Description { get; set; }
    }
}
