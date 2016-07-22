using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using System.Web.Security;

namespace Knowledge_Management.Controllers
{
    public class EmployeeProfileController : Controller
    {
        // GET: EmployeeProfile
        public ActionResult Index()
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);

            ViewBag.dataentry = Boolean.Parse(emp_prop[2]);
            ViewBag.dataview = Boolean.Parse(emp_prop[3]);

            return View();
        }
    }
}