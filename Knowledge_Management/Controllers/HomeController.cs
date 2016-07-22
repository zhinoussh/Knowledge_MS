using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using Knowledge_Management.Models;
using System.Web.Security;

namespace Knowledge_Management.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(tbl_login user)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                string role = DAL.login(user.username, user.pass);
                if (role != null && role != "" )
                {
                    ViewBag.Fail = "false";
                    FormsAuthentication.SetAuthCookie(user.username, false);
                    if (role == "1")
                        return Json(new { url = "/Strategy/Index" });
                    else
                    {
                        List<string> emp_prop = DAL.get_Employee_prop(user.username);

                        bool dtentry = Boolean.Parse(emp_prop[2]);
                        bool dtview = Boolean.Parse(emp_prop[3]);

                        if(dtentry)
                            return Json(new { url = "/InsertInfo/Index" });
                        else if (dtview)
                            return Json(new { url = "/SearchInfo/SearchAll" });
                        else
                            return Json(new { url = "/EmployeeProfile/Index" });
                    }
                }

                ViewBag.Fail = "true";
                return PartialView("_LoginPartial",user);
            }
            else
            {
                ModelState.AddModelError("loginErr", "login data is invalid");
                return View(user);

            }

        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
           
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
    }
}