﻿using Common.Utils.Utils;
using Infraestructure.Entity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyVet.Handlers;
using MyVetDomain.Dto;
using MyVetDomain.Dto.RestServices;
using MyVetDomain.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Common.Utils.Constant.Const;

namespace MyVet.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class AuthController : Controller
    {
        private readonly IUserServices _userServices;

        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }
            [HttpGet]
            public IActionResult Login()
            {
                return View(new UserDto());
            }

            [HttpPost]
            public async Task<IActionResult> Login(UserDto user)
            {
                ResponseDto response = await _userServices.Login(user);

                if (!response.IsSuccess) 
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(user);
                }
                else
                {
                //    UserEntity userEntity = (UserEntity)response.Result;

                //    var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, userEntity.FullName),
                //    new Claim(TypeClaims.IdUser, userEntity.IdUser.ToString()),
                //    new Claim(TypeClaims.UserName, userEntity.Email),
                //    new Claim(TypeClaims.IdRol, string.Join(",",userEntity.RolUserEntities.Select(x=>x.IdRol))),
                //};

                TokenDto token = JsonConvert.DeserializeObject<TokenDto>(response.Result.ToString());
                string idRoles = Utils.GetClaimValue(token.Token, TypeClaims.IdRol);
                string idUser = Utils.GetClaimValue(token.Token, TypeClaims.IdUser);
                var claims = new List<Claim>
                {
                    new Claim("Token", token.Token),
                    new Claim("Expiration", token.Expiration.ToString()),
                    new Claim(TypeClaims.IdRol,idRoles),
                    new Claim(TypeClaims.IdUser,idUser),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes((token.Expiration / 60)-10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = false,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                  new ClaimsPrincipal(claimsIdentity),
                                                  authProperties);

                    return Redirect("/Home/Index");
                }

            }


            [HttpGet]
            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(Login));
            }


            [HttpGet]
            public IActionResult Register()
            {
                return View(new UserDto());
            }

            [HttpPost]
            public async Task<IActionResult> Register(UserDto user)
            {
                IActionResult response;

                var result = await _userServices.Register(user);
                if (!result.IsSuccess)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(user);
                }
                else
                {
                    response = RedirectToAction(nameof(Login));
                }
                return response;
            }
        }
}
