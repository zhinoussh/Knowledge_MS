using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Knowledge_Management.DAL
{
    public class KnowledgeMsSL  : IKnowledgeMSSL
    {
        private IKnowledgeMSDAL _dataLayer;

        public IKnowledgeMSDAL DataLayer
        {
            get
            {
                if (_dataLayer == null)
                    _dataLayer = new KnowledgeMSDAL(new KnowledgeMsDB());

                return _dataLayer;
            }
            set {
                _dataLayer = value;
            }
        }
        public string Post_Login(string userName,string passWord,Controller ctrl)
        {
            string returnUrl = "";

            if (DataLayer.login(userName, passWord))
            {

                List<string> emp_prop = DataLayer.get_Employee_prop(userName);
                string fullname = "";
                if (emp_prop != null)
                    fullname = emp_prop[4];

                var authTicket = new FormsAuthenticationTicket(1, //version
                           userName, // user name
                           DateTime.Now,             //creation
                           DateTime.Now.AddMinutes(30), //Expiration
                           false, //Persistent
                           fullname);

                var encTicket = FormsAuthentication.Encrypt(authTicket);
                ctrl.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                string[] roles = Roles.GetRolesForUser(userName);


                if (roles.Contains("Admin"))
                    returnUrl="/Admin/Strategy/Index";
                else
                {
                    if (roles.Contains("DataEntry"))
                        returnUrl= "/User/InsertInfo/Index";
                    else if (roles.Contains("DataView"))
                        returnUrl = "/User/SearchInfo/SearchAll";
                    else if (roles.Contains("Public"))
                        returnUrl= "/User/EmployeeProfile/Index";
                }
            }

            return returnUrl;
        }
    }
}