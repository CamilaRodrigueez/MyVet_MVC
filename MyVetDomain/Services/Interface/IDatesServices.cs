using MyVetDomain.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyVetDomain.Services.Interface
{
    public interface IDatesServices
    {
        List<DatesDto> GetAllMyDates(int idUser);
        List<ServicesDto> GetAllServices();
        Task<bool> InsertDatesAsync(DatesDto dates);
        Task<ResponseDto> DeleteDatesAsync(int idDates);
        Task<bool> UpdateDatesAsync(DatesDto dates);
        Task<bool> CancelDatesAsync(int idDates, int? idUserVet);
        List<DatesDto> GetAllDates(int idUser);
        Task<bool> UpdateDatesVetAsync(DatesDto dates);


    }
}
