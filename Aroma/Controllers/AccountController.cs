﻿using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.User;
using AutoMapper;
using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static Tensorflow.SummaryMetadata.Types;


namespace Lab_TW.Controllers
{
    public class AccountController : BaseController
    {
        private   ISession _session;

        public AccountController() 
        {
            var logicBL = new BussinesLogic();
            _session = logicBL.GetSessionBL();

        }

        public ActionResult Logout()
        {
            StatusSessionCheck();
            // Вызываем метод выхода из системы
            ResponseLogout logoutResponse = _session.UserLogout();

            if (logoutResponse.Status)
            {
                // Если выход успешен, можно очистить какие-либо данные сессии или выполнить другие действия
                // Например, удалить куки сессии (если они используются)
                if (Response.Cookies["X-KEY"] != null)
                {
                    var IsUserLoggedIn = false; // Статус аутентификации сохраняется между запросами   
                    var admin = false;
                    var moderator = false;
                    var userId = 0;
                    Session["UserId"] = userId;
                    Session["admin"] = admin.ToString(); // Сохраняем роль в сессии
                    Session["moderator"] = moderator.ToString(); // Сохраняем роль в сессии
                    Session["IsUserLoggedIn"] = IsUserLoggedIn.ToString(); // Сохраняем роль в сессии

                    var cookie = new HttpCookie("X-KEY")
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Add(cookie);
                }

                // Перенаправляем пользователя на страницу входа
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Обработка ошибки выхода, если потребуется
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: login
        public ActionResult login()
        {
            StatusSessionCheck();
            return View();
        }
        public ActionResult EditProfile(LoginData data)
        {
            StatusSessionCheck();

            int UserId = (int)Convert.ToUInt32(Session["UserId"]);
            /*    GetUserId();*/

            Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());

            var updateProfile = Mapper.Map<ULoginData>(data);
                {
                updateProfile.credential = data.Username;
                updateProfile.Id = UserId;
            }


            // Вызов метода бизнес-логики для обновления продукта
            ResponseToEditProfile response = _session.ProfileUpdateAction(updateProfile);

                if (response.Status)
                {
                return RedirectToAction("UProfile", "Account");
                  
                }
                else
                {
                    // Если при обновлении произошла ошибка, отображаем сообщение об ошибке
                    ViewBag.ErrorMessage = response.MessageError;
                    return View("Error"); // Предполагается, что у вас есть представление Error.cshtml для отображения ошибок
                }


            

        }

      
        [HttpPost]
        public ActionResult ChangePassword(LoginData user,string newPassword, string confirmPassword)
        {
            StatusSessionCheck();
            // Логика обновления пароля
            int UserId = (int)Convert.ToUInt32(Session["UserId"]);
            if (newPassword == confirmPassword)
            {
                ULoginData NewPassword = new ULoginData()
                {
                    password = newPassword,
                    Id = UserId

                };

                ResponseEditPassword newPass = _session.EditUserPass(NewPassword, newPassword);


                // Возвращаем результат (например, перенаправление на профиль пользователя)
                return RedirectToAction("UProfile");
            }
            else
            {
                // Обработка ошибки, если пароли не совпадают
                ModelState.AddModelError("", "Пароли не совпадают");
                return View("UProfile"); // Перенаправляем обратно на профиль пользователя с ошибкой
            }
        }

        [HttpGet]
        public ActionResult UProfile()
        {
            StatusSessionCheck();


            int UserId = (int)Convert.ToUInt32(Session["UserId"]);
            if (UserId == 0)
            {
                GetUserId();
                if (UserId == -1)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserId = (int)Convert.ToUInt32(Session["UserId"]);
            }


            // Используем сервис для получения данных о пользователе по идентификатору
            ResponseViewProfile userProfile = _session.ViewProfile(UserId);

            LoginData data = new LoginData()
            {
                Username = userProfile.Username,
                Email = userProfile.Email,
                Balance = userProfile.Balance

            };



         
                        // Возвращаем представление с информацией о пользователе
                        return View(data);
                
                
               
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginData data)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());

                var Data = Mapper.Map<ULoginData>(data); // Исправлено здесь
                {
                    Data.IP = Request.UserHostAddress;
                    Data.FirstLoginTime = DateTime.Now;
                    Data.credential = data.Username;
                };

                RResponseData response = _session.UserLoginAction(Data);

                if (response != null && response.Status)
                {
                    HttpCookie cookie = _session.GenCookie(Data.credential, data.RememberMe);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    ViewBag.UserName = Data.credential;
                    Session["IsUserLoggedIn"] = System.Web.HttpContext.Current.Session["LoginStatus"];

                    if (response.AdminMod || response.ModeratorMod)
                    {
                        ViewBag.UserName = Data.credential;
                        return RedirectToAction("ProductsAdminPanel", "Product");
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (response.ResponseMessage != null)
                {
                    ViewBag.ErrorMessage = response.ResponseMessage;
                }
                // Возврат на страницу входа с сообщением об ошибке
                return View("~/Views/Account/Login.cshtml");
            }
            // Возврат на страницу входа, если модель не валидна
            return View();
        }


        // GET: Register
        public ActionResult Register()
            {


                return View();
            }
            [HttpPost]

            public ActionResult Register(RegisterData RegData)
            {

                var uRegData = new URegisterData()
                {
                    Name = RegData.Username,
                    Password = RegData.Password,
                    Email = RegData.Email,
                    IP = Request.UserHostAddress,
                    RegDate = DateTime.Now,
                    AcceptPassword = RegData.AcceptPassword

                };

                URegisterResp uRegisterResp = _session.UserRegisterAction(uRegData);
                if (uRegisterResp != null && uRegisterResp.Status)
                {

                    return RedirectToAction("Login", "Account");

                }
                if (uRegisterResp.ResponseMessage != null)
                {
                    ViewBag.ErrorPassword = uRegisterResp.ResponseMessage;
                }

            return View("~/Views/Account/Register.cshtml");


        }
        }
}

