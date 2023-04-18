using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Handlers;
using MyVetDomain.Dto;
using MyVetDomain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Utils.Constant.Const;

namespace MyVet.Controllers
{
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class DatesController : Controller
    {
        #region Attribute
        private readonly IDatesServices _datesServices;
        #endregion

        #region Buider
        public DatesController(IDatesServices datesServices)
        {
            _datesServices = datesServices;
        }
        #endregion
        #region Views
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DatesVet()
        {
            return View();
        }
        #endregion
        #region Methods
        [HttpGet]
        public IActionResult GetAllDates()
        {
            var user = HttpContext.User;
            string idUser = user.Claims.FirstOrDefault(x => x.Type == TypeClaims.IdUser).Value;

            List<DatesDto> list = _datesServices.GetAllDates(Convert.ToInt32(idUser));
            return Ok(list);
        }
        [HttpGet]
        public IActionResult GetAllMyDates()
        {
            var user = HttpContext.User;
            string idUser = user.Claims.FirstOrDefault(x => x.Type == TypeClaims.IdUser).Value;

            List<DatesDto> list = _datesServices.GetAllMyDates(Convert.ToInt32(idUser));
            return Ok(list);
        }
        [HttpGet]
        public IActionResult GetAllServices()
        {
            List<ServicesDto> response = _datesServices.GetAllServices();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertDates(DatesDto dates)
        {
            bool response = await _datesServices.InsertDatesAsync(dates);
            return Ok(response);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteDates(int idDates)
        {
            ResponseDto response = await _datesServices.DeleteDatesAsync(idDates);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDates(DatesDto dates)
        {
            bool response = await _datesServices.UpdateDatesAsync(dates);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDatesVet(DatesDto dates)
        {
            var user = HttpContext.User;
            string idUser = user.Claims.FirstOrDefault(x => x.Type == TypeClaims.IdUser).Value;
            dates.IdUserVet = Convert.ToInt32(idUser);

            bool result = await _datesServices.UpdateDatesVetAsync(dates);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> CancelDates(int idDates)
        {
            bool result = await _datesServices.CancelDatesAsync(idDates, idUserVet: null);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> CancelDatesVet(int idDates)
        {
            var user = HttpContext.User;
            string idUser = user.Claims.FirstOrDefault(x => x.Type == TypeClaims.IdUser).Value;

            bool result = await _datesServices.CancelDatesAsync(idDates, Convert.ToInt32(idUser));
            return Ok(result);
        }





        #endregion
    }
}
