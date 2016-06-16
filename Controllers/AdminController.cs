using System;
using System.Collections.Generic;
using System.Linq;
using MVCtutorial.Admin.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name.ToString(), "Admin")) {
                Session["IsAdmin"] = "yes";
            }
            return View();
        }

        public ActionResult AddUser()
        {
            ViewBag.roles = Roles.GetAllRoles();
            if (Session["tempforview"] != null){
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }            
            return View();
        }

        [HttpPost]
        public ActionResult AddUserToRole(AdminAddUserModel model) {
            try
            {
                int id;
                if (model.Role.Contains("AllDest"))
                {
                    string[] RolesArray = Roles.GetAllRoles();
                    foreach (String role in RolesArray)
                    {
                        if (Int32.TryParse(role, out id) == true)
                        {
                            if (!(Roles.IsUserInRole(model.User, role)))
                            {
                                Roles.AddUserToRole(model.User, role);
                            }
                        }

                    }
                    if (Roles.IsUserInRole(model.User, "AllDest"))
                    {
                        Roles.RemoveUserFromRole(model.User, "AllDest");
                    }
                    Roles.AddUserToRole(model.User, "AllDest");
                }
                else
                {
                    Roles.AddUserToRole(model.User, model.Role);
                }
                Session["tempforview"] = "User: " + model.User + " has been successfully added to role: " + model.Role + ".";

                return RedirectToAction("AddUser", "Admin");
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                throw msg;
            }
        }
        public ActionResult RemoveUser()
        {
            ViewBag.roles = Roles.GetAllRoles();
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult RemoveUserFromRole(AdminAddUserModel model)
        {
            try
            {
                int id;
                if (model.Role.Contains("AllDest"))
                {
                    string[] RolesArray = Roles.GetAllRoles();
                    foreach (String role in RolesArray)
                    {
                        if (Int32.TryParse(role, out id) == true)
                        {
                            if (Roles.IsUserInRole(model.User, role))
                            {
                                Roles.RemoveUserFromRole(model.User, role);
                            }
                        }
                    }
                }
                else
                {
                    Roles.RemoveUserFromRole(model.User, model.Role);
                }
                Session["tempforview"] = "User: " + model.User + " has been successfully removed from role: " + model.Role + ".";
                return RedirectToAction("RemoveUser", "Admin");
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                throw msg;
            }
        }

        public ActionResult AddRole()
        {
            ViewBag.roles = Roles.GetAllRoles();
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddRoleForm(AdminAddUserModel model)
        {
            try
            {
                int id;
                if (Roles.RoleExists(model.Role))
                {
                    Session["tempforview"] = "Role: " + model.Role + " already exist look up!";
                }
                else
                {
                    Roles.CreateRole(model.Role);
                    if (Int32.TryParse(model.Role, out id) == true) {
                        string[] users  = Roles.GetUsersInRole("AllDest");
                        Roles.AddUsersToRole(users, model.Role);
                    }
                    
                    Session["tempforview"] = "Role: " + model.Role + " has been successfully added.";
                }

                return RedirectToAction("AddRole", "Admin");
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                throw msg;
            }
        }

        public ActionResult RemoveRole()
        {
            ViewBag.roles = Roles.GetAllRoles();
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            return View();
        }
        [HttpPost]
        public ActionResult RemoveRoleForm(AdminAddUserModel model)
        {
            try
            {
                int id;
                if (!(Roles.RoleExists(model.Role)))
                {
                    Session["tempforview"] = "Role: " + model.Role + " already has been deleted look up!";
                }
                else
                {
                    if (Int32.TryParse(model.Role, out id) == true)
                    {
                        string[] users = Roles.GetUsersInRole(model.Role);
                        Roles.RemoveUsersFromRole(users,model.Role);
                    }
                    Roles.DeleteRole(model.Role);
                    Session["tempforview"] = "Role: " + model.Role + " has been successfully deleted.";
                }

                return RedirectToAction("AddRole", "Admin");
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                throw msg;
            }
        }
    }
}