using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagementApp.Models;
using UserManagementApp.Service;
using UserManagementApp.Service.Implementation;

namespace UserManagementApp.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserManagement _userManagement;

        public UserController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }
        // GET: User
        public ActionResult LinkUser()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                IEnumerable<UserModel> userlist = _userManagement.GetAllUsers();
                ViewBag.GroupList = ToSelectList(_userManagement.GetAllGroups(), "Value", "Text");
                return View(userlist);
            }
            return RedirectToAction("DashBoard", "Home");
        }

        public ActionResult Index()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                IEnumerable<UserModel> userlist = _userManagement.GetAllUsers();
                ViewBag.GroupList = ToSelectList(_userManagement.GetAllGroups(), "Value", "Text");
                return View(userlist);
            }
            return RedirectToAction("DashBoard", "Home");
        }

        // GET: User/Create
        public ActionResult Create()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                UserModel userModel = new UserModel();
                ViewBag.GroupList = ToSelectList(_userManagement.GetAllGroups(), "Value", "Text");
                return View(userModel);
            }
            return RedirectToAction("DashBoard", "Home");
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserModel userModel)
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _userManagement.SubmitUser(userModel);
                        return RedirectToAction("Index");
                    }
                    return View("Create", userModel);
                }
                return RedirectToAction("DashBoard", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult LinkUserGroup(int id)
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                UserModel userModel = _userManagement.GetUserDetails(id);
                ViewBag.GroupList = ToSelectList(_userManagement.GetAllGroups(), "Value", "Text");
                return View(userModel);
            }
            return RedirectToAction("DashBoard", "Home");
        }

        [HttpPost]
        public ActionResult LinkUserGroup(int id, UserModel userModel)
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _userManagement.SubmitUser(userModel);
                        return RedirectToAction("LinkUser");
                    }
                    return View("LinkUserGroup", userModel);
                }
                return RedirectToAction("DashBoard", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult LinkGroup(int id, UserModel userModel)
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _userManagement.SubmitUser(userModel);
                        return RedirectToAction("Index");
                    }
                    return View("LinkGroup", userModel);
                }
                return RedirectToAction("DashBoard", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                UserModel userModel = _userManagement.GetUserDetails(id);
                ViewBag.GroupList = ToSelectList(_userManagement.GetAllGroups(), "Value", "Text");
                return View(userModel);
            }
            return RedirectToAction("DashBoard", "Home");
        }
        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserModel userModel)
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _userManagement.SubmitUser(userModel);
                        return RedirectToAction("Index");
                    }
                    return View("Edit", userModel);
                }
                return RedirectToAction("DashBoard", "Home");
            }
            catch
            {
                return View();
            }
        }


        [NonAction]
        public SelectList ToSelectList(IEnumerable<GroupModel> groupList, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var group in groupList)
            {
                list.Add(new SelectListItem()
                {
                    Text = group.GroupName.ToString(),
                    Value = group.GroupID.ToString()
                });
            }

            return new SelectList(list, valueField, textField);
        }
    }
}
