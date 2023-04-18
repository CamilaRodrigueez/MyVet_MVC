﻿using Common.Utils.RestServices.Interface;
using Common.Utils.Utils;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Models;
using Microsoft.Extensions.Configuration;
using MyVetDomain.Dto;
using MyVetDomain.Dto.RestServices;
using MyVetDomain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Utils.Enums.Enums;

namespace MyVetDomain.Services
{
    public class UserServices : IUserServices
    {
        #region Attribute
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestService _restService;
        private readonly IConfiguration _config;
        #endregion

        #region Builder
        public UserServices(IUnitOfWork unitOfWork, IRestService restService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _restService= restService;
            _config = config;
        }
        #endregion




        #region authentication
        public async Task<ResponseDto> Login(UserDto user)
        {

            string urlBase = _config.GetSection("ApiMyVet").GetSection("UrlBase").Value;
            string controller = _config.GetSection("ApiMyVet").GetSection("ControlerAuthentication").Value;
            string method = _config.GetSection("ApiMyVet").GetSection("MethodLogin").Value;

            LoginDto parameters = new LoginDto()
            {
                Password = user.Password,
                UserName = user.UserName,
            };
            Dictionary<string, string> headers = new Dictionary<string, string>();
            ResponseDto resultToken = await _restService.PostRestServiceAsync<ResponseDto>(urlBase, controller, method, parameters, headers);

            return resultToken;

            //ResponseDto response = new ResponseDto();
            //UserEntity result = _unitOfWork.UserRepository.FirstOrDefault(x => x.Email == user.UserName
            //                                                                && x.Password == user.Password,
            //                                                               r => r.RolUserEntities);
            //if (result == null)
            //{
            //    response.Message = "Usuario o contraseña inválida!";
            //    response.IsSuccess = false;
            //}
            //else
            //{
            //    response.Result = result;
            //    response.IsSuccess = true;
            //    response.Message = "Usuario autenticado!";
            //}

        }
        #endregion

        #region Methods Crud
        public List<UserEntity> GetAll()
        {
            return _unitOfWork.UserRepository.GetAll().ToList();
        }

        public UserEntity GetUser(int idUser)
        {
            return _unitOfWork.UserRepository.FirstOrDefault(x => x.IdUser == idUser);
        }

        public async Task<bool> UpdateUser(UserEntity user)
        {
            UserEntity _user = GetUser(user.IdUser);

            _user.Name = user.Name;
            _user.LastName = user.LastName;
            _unitOfWork.UserRepository.Update(_user);

            return await _unitOfWork.Save() > 0;

        }
        public async Task<bool> DeleteUser(int idUser)
        {
            _unitOfWork.UserRepository.Delete(idUser);

            return await _unitOfWork.Save() > 0;
        }

        public async Task<ResponseDto> CreateUser(UserEntity data)
        {
            ResponseDto result = new ResponseDto();

            if (Utils.ValidateEmail(data.Email))
            {
                if (_unitOfWork.UserRepository.FirstOrDefault(x => x.Email == data.Email) == null)
                {
                    int idRol = data.IdUser;
                    data.Password = "123456";
                    data.IdUser = 0;

                    RolUserEntity rolUser = new RolUserEntity()
                    {
                        IdRol = idRol,
                        UserEntity = data
                    };

                    _unitOfWork.RolUserRepository.Insert(rolUser);
                    result.IsSuccess = await _unitOfWork.Save() > 0;
                }
                else
                    result.Message = "Email ya se encuestra registrado, utilizar otro!";
            }
            else
                result.Message = "Usuario  con Email Inválido";

            return result;
        }//Pendiente para eliminar
        public async Task<ResponseDto> Register(UserDto data)
        {
            ResponseDto result = new ResponseDto();

            if (Utils.ValidateEmail(data.UserName))
            {
                if (_unitOfWork.UserRepository.FirstOrDefault(x => x.Email == data.UserName) == null)
                {

                    RolUserEntity rolUser = new RolUserEntity()
                    {
                        IdRol = RolUser.Estandar.GetHashCode(),
                        UserEntity = new UserEntity()
                        {
                            Email = data.UserName,
                            LastName = data.LastName,
                            Name = data.Name,
                            Password = data.Password
                        }
                    };

                    _unitOfWork.RolUserRepository.Insert(rolUser);
                    result.IsSuccess = await _unitOfWork.Save() > 0;
                }
                else
                    result.Message = "Email ya se encuestra registrado, utilizar otro!";
            }
            else
                result.Message = "Usuario con Email Inválido";

            return result;
        }
        #endregion
    }
}
