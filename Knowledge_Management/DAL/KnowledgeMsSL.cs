using Knowledge_Management.Areas.Admin.ViewModels;
using Knowledge_Management.Models;
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

        #region Home
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

        #endregion Home

        #region Department
        public void Post_Add_Edit_Department(DepartmentViewModel vm)
        {
            DataLayer.InsertDepartment(vm.dep_id, vm.dep_name);
        }

        public DepartmentViewModel Get_Delete_Department(int departmentId)
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            vm.dep_id = departmentId;

            return vm;
        }

        public void Post_Delete_Department(DepartmentViewModel vm)
        {
            DataLayer.DeleteDepartment(vm.dep_id);
        }

        public Tuple<List<tbl_department>, int> Get_DepartmentTableContent(string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<tbl_department> all_items = DataLayer.get_all_Departments();

            //filtering 
            List<tbl_department> filtered = new List<tbl_department>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.department_name.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            //sorting
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.department_name).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.department_name).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<tbl_department>, int>(filtered, all_items.Count);
        }


        #endregion Department

    }
}