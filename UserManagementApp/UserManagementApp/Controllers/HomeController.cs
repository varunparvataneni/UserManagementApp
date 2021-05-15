using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagementApp.Models;
using UserManagementApp.Service;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private IUserManagement _userManagement;
        public HomeController(IUserManagement userManagement)
        {
            this._userManagement = userManagement;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel LoginRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userdetails = _userManagement.IsValidUser(LoginRequest.LoginName, LoginRequest.Password);
                    if (userdetails!=null)
                    {
                        HttpContext.Session["Userdetails"] = userdetails;
                        var UserMenuList =_userManagement.GetGroupDetails(userdetails.Group_ID).MenuList;
                        foreach (var menu in UserMenuList)
                        {
                            HttpContext.Session[menu.MenuName] = menu.IsSelected;
                        }
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ViewBag.NotValidUser = "Invalid User Details";
                    }
                }            
                return View("Index", LoginRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult About()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {

                ViewBag.Message = "Argility- User Management Application";

                return View();
            }
            return RedirectToAction("Index");
        }


        public ActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();//clear session
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Logout"));
            }
        }

        public ActionResult DashBoard()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}