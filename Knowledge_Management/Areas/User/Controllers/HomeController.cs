using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using Knowledge_Management.Models;
using System.Web.Security;

namespace Knowledge_Management.Areas.User.Controllers
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

                if(DAL.login(user.username, user.pass))
                {
                    //ViewBag.Fail = "false";
                    //FormsAuthentication.SetAuthCookie(user.username, false);

                    List<string> emp_prop = DAL.get_Employee_prop(user.username);
                    string fullname = "";
                    if(emp_prop!=null)
                        fullname = emp_prop[4];

                    var authTicket = new FormsAuthenticationTicket(1, //version
                               user.username, // user name
                               DateTime.Now,             //creation
                               DateTime.Now.AddMinutes(30), //Expiration
                               false, //Persistent
                               fullname);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    string[] roles = Roles.GetRolesForUser(user.username);
                    if (roles.Contains("Admin"))
                        return Json(new { url = "/Admin/Strategy/Index" });
                    else
                    {
                        if (roles.Contains("DataEntry"))
                            return Json(new { url = "/InsertInfo/Index" });
                        else if (roles.Contains("DataView"))
                            return Json(new { url = "/SearchInfo/SearchAll" });
                        else if (roles.Contains("Public"))
                            return Json(new { url = "/EmployeeProfile/Index" });
                    }
                }

               // ViewBag.Fail = "true";
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