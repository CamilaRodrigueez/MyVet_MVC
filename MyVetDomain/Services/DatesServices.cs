using Common.Utils.Enums;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Models.Master;
using Infraestructure.Entity.Models.Vet;
using MyVetDomain.Dto;
using MyVetDomain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVetDomain.Services
{
    public class DatesServices : IDatesServices

    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion


        #region Builder
        public DatesServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods
        public List<DatesDto> GetAllDates(int idUser)
        {
            var dates = _unitOfWork.DatesRepository.FindAll(x=>(x.IdUserVet==idUser || x.IdUserVet== null)
                                                            , d => d.PetEntity.UserPetEntity
                                                            , d => d.PetEntity.TypePetEntity
                                                            , d => d.ServicesEntity
                                                            , d => d.StateEntity).ToList();
            var datesDeleteList = dates.Where(x => (x.IdState == (int)Enums.State.CitaCancelada && x.IdUserVet == null)).ToList();


            var datesSelect = (from t in dates
                               where !datesDeleteList.Any(x => x.Id == t.Id)
                               select t).ToList();


            List<DatesDto> list = datesSelect.Select(x => new DatesDto
            {
                IdDates = x.Id,
                Name = $"{x.PetEntity.Name}  [{x.PetEntity.TypePetEntity.TypePet}]",
                Contact = x.Contact,
                Date = x.Date,
                IdServives = x.IdServives,
                IdPet = x.IdPet,
                IdUserVet = x.IdUserVet,
                IdState = x.IdState,
                Estado = x.StateEntity.State,
                ClosingDate = x.ClosingDate,
                StrClosingDate = x.ClosingDate == null ? "No disponible" : x.ClosingDate.Value.ToString("yyyy-MM-dd"),
                StrDate = x.Date.ToString("yyyy-MM-dd"),
                Services = x.ServicesEntity.Services,
                Description = x.Description,

            }).OrderByDescending(f => f.Date).ToList();


            return list;
        } 
        public List<DatesDto> GetAllMyDates(int idUser)
        {
            var dates = _unitOfWork.DatesRepository.FindAll(p => p.PetEntity.UserPetEntity.IdUser == idUser
                                                            , d => d.PetEntity.UserPetEntity
                                                            , d => d.PetEntity.TypePetEntity
                                                            , d => d.ServicesEntity
                                                            , d => d.StateEntity).ToList();


            List<DatesDto> list = dates.Select(x => new DatesDto
            {
                IdDates = x.Id,
                Name = $"{x.PetEntity.Name}  [{x.PetEntity.TypePetEntity.TypePet}]",
                Contact = x.Contact,
                Date = x.Date,
                IdServives = x.IdServives,
                IdPet = x.IdPet,
                IdUserVet = x.IdUserVet,
                IdState = x.IdState,
                Estado = x.StateEntity.State,
                ClosingDate = x.ClosingDate,
                StrClosingDate = x.ClosingDate == null ? "No disponible" : x.ClosingDate.Value.ToString("yyyy-MM-dd"),
                StrDate = x.Date.ToString("yyyy-MM-dd"),
                Services = x.ServicesEntity.Services,
                Description = x.Description,

            }).OrderByDescending(f => f.Date).ToList();


            return list;
        }

        public List<ServicesDto> GetAllServices()
        {
            List<ServicesEntity> services = _unitOfWork.ServicesRepository.GetAll().ToList();

            List<ServicesDto> list = services.Select(x => new ServicesDto
            {
                IdServices = x.Id,
                Services = x.Services
            }).ToList();

            return list;
        }

        public async Task<bool> InsertDatesAsync(DatesDto data)
        {
            DatesEntity dates = new DatesEntity()
            {
                Contact = data.Contact,
                Date = data.Date,
                Description = data.Description,
                IdPet = data.IdPet,
                IdServives = data.IdServives,
                IdState = (int)Enums.State.CitaActiva,
            };
            _unitOfWork.DatesRepository.Insert(dates);

            return await _unitOfWork.Save() > 0;
        }

        public async Task<ResponseDto> DeleteDatesAsync(int idDates)
        {
            ResponseDto response = new ResponseDto();

            _unitOfWork.DatesRepository.Delete(idDates);
            response.IsSuccess = await _unitOfWork.Save() > 0;
            if (response.IsSuccess)
                response.Message = "Se elminnó correctamente la cita";
            else
                response.Message = "Hubo un error al eliminar la cita, por favor vuelva a intentalo";

            return response;
        }
       
        public async Task<bool> UpdateDatesAsync(DatesDto dates)
        {
            bool result = false;

            DatesEntity datesEntity = _unitOfWork.DatesRepository.FirstOrDefault(x => x.Id == dates.IdDates);
            if (datesEntity != null)
            {

                datesEntity.Contact = dates.Contact;
                datesEntity.Date = dates.Date;
                datesEntity.Description = dates.Description;
                datesEntity.IdPet = dates.IdPet;
                datesEntity.IdServives = dates.IdServives;
                datesEntity.IdState = datesEntity.IdState;

                _unitOfWork.DatesRepository.Update(datesEntity);
                result = await _unitOfWork.Save() > 0;
            }

            return result;
        } 
        public async Task<bool> UpdateDatesVetAsync(DatesDto dates)
        {
            bool result = false;

            DatesEntity datesEntity = _unitOfWork.DatesRepository.FirstOrDefault(x => x.Id == dates.IdDates);
            if (datesEntity != null)
            {

                datesEntity.Contact = dates.Contact;
                datesEntity.Date = dates.Date;
                datesEntity.Description = dates.Description;
                datesEntity.IdPet = dates.IdPet;
                datesEntity.IdServives = dates.IdServives;
                datesEntity.IdState = (int)Enums.State.CitaFinalizada;
                datesEntity.IdUserVet=dates.IdUserVet;
                datesEntity.ClosingDate = DateTime.Now;
                datesEntity.Observation = dates.Observation;

                _unitOfWork.DatesRepository.Update(datesEntity);
                result = await _unitOfWork.Save() > 0;
            }

            return result;
        }
        public async Task<bool> CancelDatesAsync(int idDates, int? idUserVet)
        {
            bool result = false;

            DatesEntity dates = _unitOfWork.DatesRepository.FirstOrDefault(x => x.Id == idDates);
            if (dates != null)
            {
                dates.IdState = (int)Enums.State.CitaCancelada;
                dates.ClosingDate = DateTime.Now;
                dates.IdUserVet = idUserVet ?? null;
                _unitOfWork.DatesRepository.Update(dates);
                result = await _unitOfWork.Save() > 0;
            }

            return result;
        }

        #endregion
    }
}
