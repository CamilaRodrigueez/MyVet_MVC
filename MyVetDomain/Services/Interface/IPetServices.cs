using MyVetDomain.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyVetDomain.Services.Interface
{
    public interface IPetServices
    {
        Task<ResponseDto> GetAllMyPets(string token);
        List<TypePetDto> GetAllTypePet();
        List<SexDto> GetAllSexs(); 
        Task<ResponseDto> DeletePetAsync(int idPet);
        Task<bool> InsertPetAsync(PetDto pet);
        Task<bool> UpdatePetAsync(PetDto pet);
    }
}
