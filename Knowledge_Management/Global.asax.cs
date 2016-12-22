using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Claims;
using Knowledge_Management.DAL;

namespace Knowledge_Management
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //initialise admin user
            RegisterAdminUser();
        }

        private void RegisterAdminUser()
        {
            KnowledgeMSDAL db = new KnowledgeMSDAL();
            if (!db.check_userinRole("admin", "Admin"))
            {
                string encrypt_pass = (new Encryption()).Encrypt("1");
                db.initialise_admin_user(encrypt_pass);
            }
        }

        void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                string encTicket = authCookie.Value;
                if (!String.IsNullOrEmpty(encTicket))
                {
                    
                    var ticket = FormsAuthentication.Decrypt(encTicket);
                    
                    var id = new UserIdentity(ticket);
                    id.Fullname = ticket.UserData;
                    var userRoles = Roles.GetRolesForUser(id.Name);
                    var prin = new GenericPrincipal(id, userRoles);
                    
                    HttpContext.Current.User = prin;
                }
            }
        }

        protected void Application_EndRequest()
        {//here breakpoint
            // under debug mode you can find the exceptions at code: this.Context.AllErrors
            Exception[] err_array = this.Context.AllErrors;
        }
    }
}
