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
        private static string maskTable = "mask_directory";
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult  AddMask()
        {
            ViewBag.roles = Roles.GetAllRoles();
            db db = new db();
            List<string> stringList = new List<string>();
            List<object> objectList = db.multipleItemSelect("maskFile", maskTable, "bakeryId='" + Session["id"] + "'");
            foreach (object o in objectList)
            {
                stringList.Add(o.ToString());
            }
            ViewBag.masks = stringList;
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            return View();
        }

        /*
         * @param AdminAddMaskModel model, return void 
         * Method to add mask of file(directory)
         */
        [HttpPost]
        public ActionResult AddMaskForm(AdminAddMaskModel model)
        {
            db db = new db();
            //string FileMask = db.singleItemSelect("maskFile", maskTable, "bakeryId='" + bakeryId + "' AND maskFile='" + model.Mask + "'").ToString();
            if (model.bakeryId !=null) {
                int bakeryId = int.Parse(model.bakeryId.ToString());                
                db.singleItemInsertAsync(maskTable, "bakeryId,maskFile,maskName", bakeryId + ",'" + model.Mask + "','" + model.maskName + "','" + model.maskRole + "'");
                Session["tempforview"] = "Mask: " + model.Mask + " has been successfully added to bakery: " + model.bakeryId + ".";
            }
            else
            {
                db.singleItemInsertAsync(maskTable, "bakeryId,maskFile,maskName,maskRole", Session["id"].ToString() + ",'" + model.Mask + "','" + model.maskName + "'" + "','" + model.maskRole + "'");
                Session["tempforview"] = "Mask: " + model.Mask + " has been successfully added to bakery: " + Session["id"].ToString() + ".";
            }
            
            return RedirectToAction("AddMask", "Admin");
        }


        public ActionResult RemoveMask()
        {
            db db = new db();
            List<string> stringList = new List<string>();
            List<object> objectList = db.multipleItemSelect("maskFile", maskTable, "bakeryId='" + Session["id"] + "'");
            foreach (object o in objectList) {
                stringList.Add(o.ToString());
            }
            ViewBag.masks = stringList;
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            return View();
        }

        /*
         * @param void, return void 
         * Method to remove mask of file(directory)
         */
        [HttpPost]
        public ActionResult RemoveMaskForm(AdminRemoveMaskModel model)
        {
            db db = new db();
            int bakeryId = Int32.Parse(model.bakeryId.ToString());
            db.singleItemDeleteAsync(maskTable, "bakeryId='" + bakeryId + "' AND maskFile='" + model.Mask + "'"); //TODO solve potential SQL injections
            Session["tempforview"] = "Mask: " + model.Mask + " has been successfully removed from bakery: " + model.bakeryId + ".";

            return RedirectToAction("RemoveMask", "Admin");
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

        /*
         * @param model, @return redirect
         * Method to Add User to role
         * redirect is to AddUser method
         */
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

        /*
         * @param model, @return redirect
         * Method to Remopve User from role
         * redirect is to RemoveUser method
         */
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

        /*
         * @param model, @return redirect
         * Method to Add role into the system
         * redirect is to AddRole method
         */
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

        /*
         * @param model, @return redirect
         * Method to Add role into the system
         * redirect is to RemoveRole method
         */
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

                return RedirectToAction("RemoveRole", "Admin");
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                throw msg;
            }
        }
    }
}