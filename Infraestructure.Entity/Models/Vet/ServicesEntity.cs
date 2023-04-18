using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Models.Vet
{

    [Table("Services", Schema = "Vet")]
    public class ServicesEntity
    {
        [Key]
        public int Id { get; set; }
        public string Services { get; set; }
        public string Description { get; set; }

    }
}
