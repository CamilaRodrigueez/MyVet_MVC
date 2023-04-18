using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyVetDomain.Dto
{
    public class DatesDto
    {
        public int IdDates { get; set; }

        public DateTime Date { get; set; }

        public int IdPet { get; set; }

        public int IdServives { get; set; }

        public int? IdUserVet { get; set; }

        public int IdState { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string Name { get; set; }
        public string Services { get; set; }
        public string Estado { get; set; }

        [MaxLength(300)]
        public string Description { get; set; } 
        [MaxLength(300)]
        public string Observation { get; set; }

        [MaxLength(100)]
        public string Contact { get; set; }
        public string StrClosingDate { get; set; }
        public string StrDate { get; set; }
    }
}
