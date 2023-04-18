using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Models.Vet
{
    [Table("UserPet", Schema = "Vet")]

    public class UserPetEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PetEntity")]
        public int IdPet { get; set; }
        public PetEntity PetEntity { get; set; }

        [ForeignKey("UserEntity")]
        public int IdUser { get; set; }
        public UserEntity UserEntity { get; set; }
    }
}
