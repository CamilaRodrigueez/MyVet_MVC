﻿using Microsoft.AspNetCore.Authorization;
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
    public class PetController : Controller
    {
        #region Attribute
        private readonly IPetServices _petServices;
        #endregion

        #region Buider
        public PetController(IPetServices petServices)
        {
            _petServices = petServices;
        }
        #endregion

        #region Methods

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMyPets()
        {
            var user = HttpContext.User; 
            string token = user.Claims.FirstOrDefault(x => x.Type == TypeClaims.Token).Value;

            //List<PetDto> list =await _petServices.GetAllMyPets(token);
            ResponseDto response = await _petServices.GetAllMyPets(token);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePet(int idPet)
        {
            ResponseDto response = await _petServices.DeletePetAsync(idPet);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAllSexs()
        {
            List<SexDto> response = _petServices.GetAllSexs();
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAllTypePet()
        {
            List<TypePetDto> response = _petServices.GetAllTypePet();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPet(PetDto pet)
        {
            var user = HttpContext.User;
            string idUser = user.Claims.FirstOrDefault(x => x.Type == TypeClaims.IdUser).Value;
            pet.IdUser = Convert.ToInt32(idUser);

            bool response = await _petServices.InsertPetAsync(pet);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePet(PetDto pet)
        {
            bool response = await _petServices.UpdatePetAsync(pet);
            return Ok(response);
        }
        #endregion
    }
}