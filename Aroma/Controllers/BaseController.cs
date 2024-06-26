﻿using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_TW.Extension;
using Aroma.Domain.Entities.User;
using Aroma.BussinesLogic.DBModel.Seed;
namespace Lab_TW.Controllers
{
    public class BaseController : Controller
    {
        private readonly ISession _session;

        public BaseController()
        {
            var bl = new BussinesLogic();
            _session = bl.GetSessionBL();
        }

        public bool ChechEmail(string email)
        {
            using (var db = new UserContext()) 
            {
                
                    var user = db.Users.FirstOrDefault(x => x.EmailAccess == true);
                if (user != null)
                {
                    return true;

                }
                else
                    db.Users.Remove(user);
                db.SaveChanges();
                return false;
            }
        }

        public int GetUserId()
        {
            var cook = Request.Cookies["X-KEY"]?.Value;

            var user = _session.GetUserByCookie(cook);
            if (user != null)
            {
                var userId = user.Id;

                Session["UserId"] = userId; // где userId - это идентификатор пользователя
                return userId;
            }

            // Возвращаем специальное значение, чтобы показать, что пользователь не был найден
            return -1;
        }

        public void SessionStatus()
        {
            var apiCookie = Request.Cookies["X-KEY"];
            if (apiCookie != null)
            {
                var profile = _session.GetUserByCookie(apiCookie.Value);
                if (profile != null)
                {
                    System.Web.HttpContext.Current.SetMySessionObject(profile);
                    System.Web.HttpContext.Current.Session["LoginStatus"] = "login";
                    string 
                        s = profile.Level.ToString();


                    System.Web.HttpContext.Current.Session["Permission"] = s;
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Clear();
                    if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("X-KEY"))
                    {
                        var cookie = ControllerContext.HttpContext.Request.Cookies["X-KEY"];
                        if (cookie != null)
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        }
                    }

                    System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";
            }
        }

      
    }
}