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
    public class KnowledgeMsSL : IKnowledgeMSSL
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
            set
            {
                _dataLayer = value;
            }
        }

        #region Home
        public string Post_Login(string userName, string passWord, Controller ctrl)
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
                    returnUrl = "/Admin/Strategy/Index";
                else
                {
                    if (roles.Contains("DataEntry"))
                        returnUrl = "/User/InsertInfo/Index";
                    else if (roles.Contains("DataView"))
                        returnUrl = "/User/SearchInfo/SearchAll";
                    else if (roles.Contains("Public"))
                        returnUrl = "/User/EmployeeProfile/Index";
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

        #region EmployeeController

        public EmployeeViewModel Get_Employee_Index_Page()
        {
            EmployeeViewModel o = new EmployeeViewModel();

            List<tbl_department> deps = DataLayer.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey + "";

            List<tbl_job> jobs = DataLayer.get_Jobs(Int32.Parse(o.dep_id));
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = jobs.First().pkey + "";

            o.emp_id = 0;
            o.first_name = "";
            o.last_name = "";
            o.personel_code = "";

            return o;
        }

        public Tuple<int,string> Post_Add_Edit_Employee(EmployeeViewModel e)
        {
            int insert_result = DataLayer.InsertEmployee(e.emp_id,e.first_name, e.last_name, e.personel_code
                  , Int32.Parse(e.dep_id), Int32.Parse(e.job_id), e.pass, e.data_entry, e.data_view);

            string msg = "";
            int result = 0;

            if(insert_result>0)
                { msg = "Employee inserted successfully.";
                result = 1;
            }
            else if (insert_result==-1)
             { 
                msg = "This personel code is already existed.";
                result = -1;
             }
            else if (insert_result == -2)
            {
                msg = "This password is not valid."; 
                result = -2;
            }

            return new Tuple<int, string>(result, msg) ;
        }

        public EmployeeViewModel Get_Delete_Employee(int employeeId)
        {
            EmployeeViewModel e = new EmployeeViewModel();
            e.emp_id = employeeId;

            return e;
        }

        public void Post_Delete_Employee(EmployeeViewModel vm)
        {
            DataLayer.DeleteEmployee(vm.emp_id);
        }

        public Tuple<List<Employee>, int> Get_EmployeeTableContent(string filter, int sortColumnIndex, string sortDirection, int displayStart, int displayLength)
        {
            List<Employee> all_items = DataLayer.get_Employees();

            //filtering 
            List<Employee> filtered = new List<Employee>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.Emp_fname.Contains(filter)
                                        || i.Emp_lname.Contains(filter)
                                        || i.Emp_pcode.Contains(filter)
                                        || i.Dep_Name.Contains(filter)
                                        || i.Job_Name.Contains(filter)
                    ).ToList();
            }
            else
                filtered = all_items;


            //sorting
            Func<Employee, string> orderingFunction = (c => sortColumnIndex == 2 ? c.Emp_fname :
                                                        sortColumnIndex == 3 ? c.Emp_lname :
                                                        sortColumnIndex == 4 ? c.Emp_pcode :
                                                        sortColumnIndex == 5 ? c.Dep_Name :
                                                         sortColumnIndex == 6 ? c.Dep_Name : "");

            if (sortDirection == "asc")
                filtered = filtered.OrderBy(orderingFunction).ToList();
            else
                filtered = filtered.OrderByDescending(orderingFunction).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<Employee>, int>(filtered, all_items.Count);

        }

        public List<tbl_job> GetJobist(int departmentId)
        {
            List<tbl_job> jobs = DataLayer.get_Jobs(departmentId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            foreach (tbl_job j in jobs)
            {
                lst_obj.Add(new SelectListItem { Value = j.pkey + "", Text = j.job_name });
            }

            return jobs;
        }

        #endregion EmployeeController

        #region JobController

        public JobViewModel Get_Job_Index_Page()
        {
            JobViewModel o = new JobViewModel();
            List<tbl_department> deps = DataLayer.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.job_id = 0;
            o.job_name = "";
            o.selected_dep = deps.First().pkey + "";

            return o;
        }

        public void Post_Add_Edit_Job(JobViewModel j)
        {
            DataLayer.InsertJob(j.job_id, j.job_name, Int32.Parse(j.selected_dep));
        }

        public JobViewModel Get_Delete_Job(int jobId)
        {
            JobViewModel j = new JobViewModel();
            j.job_id = jobId;

            return j;
        }

        public void Post_Delete_Job(JobViewModel vm)
        {
            DataLayer.DeleteJob(vm.job_id);
        }

        public Tuple<List<tbl_job>, int> Get_JobTableContent(int departmentId,string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<tbl_job> all_items = DataLayer.get_Jobs(departmentId);

            //filtering 
            List<tbl_job> filtered = new List<tbl_job>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.job_name.Contains(filter)).ToList();
            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.job_name).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.job_name).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<tbl_job>, int>(filtered, all_items.Count);
        }

        #endregion JobController

        #region JobDescriptionController

        public JobDescriptionViewModel Get_JobDescription_Index_Page()
        {
            JobDescriptionViewModel o = new JobDescriptionViewModel();

            List<tbl_department> deps = DataLayer.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey + "";

            List<tbl_job> jobs = DataLayer.get_Jobs(Int32.Parse(o.dep_id));
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = jobs.First().pkey + "";

            o.jobDesc_id = 0;
            o.jobDesc = "";

            return o;
        }

        public void Post_Add_Edit_JobDescription(JobDescriptionViewModel j)
        {
            DataLayer.InsertJobDescription(j.jobDesc_id, j.jobDesc, Int32.Parse(j.job_id));
        }

        public JobDescriptionViewModel Get_Delete_JobDescription(int JobDescriptionId)
        {
            JobDescriptionViewModel s = new JobDescriptionViewModel();
            s.jobDesc_id = JobDescriptionId;

            return s;
        }

        public void Post_Delete_JobDescription(JobDescriptionViewModel vm)
        {
            DataLayer.DeleteJobDescription(vm.jobDesc_id);
        }

        public Tuple<List<tbl_job_description>, int> Get_JobDescriptionTableContent(int jobId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<tbl_job_description> all_items = DataLayer.get_JobDescriptions(jobId);

            //filtering 
            List<tbl_job_description> filtered = new List<tbl_job_description>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.job_desc.Contains(filter)).ToList();
            }
            else
                filtered = all_items;

            //sorting
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.job_desc).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.job_desc).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<tbl_job_description>, int>(filtered, all_items.Count);
        }

        #endregion JobDescriptionController

        #region DepartmentObjectiveController

        public DepartmentObjectiveViewModel Get_DepartmentObjective_Index_Page(int departmentId)
        {
            DepartmentObjectiveViewModel o = new DepartmentObjectiveViewModel();
            o.dep_id = departmentId;
            o.dep_name = DataLayer.get_department_name(departmentId);
            o.obj_id = 0;
            o.obj_name = "";

            return o;
        }

        public void Post_Add_Edit_DepartmentObjective(DepartmentObjectiveViewModel o)
        {
            DataLayer.InsertObjective(o.obj_id, o.obj_name, o.dep_id);
        }

        public DepartmentObjectiveViewModel Get_Delete_DepartmentObjective(int DepartmentObjectiveId)
        {
            DepartmentObjectiveViewModel s = new DepartmentObjectiveViewModel();
            s.obj_id = DepartmentObjectiveId;

            return s;
        }

        public void Post_Delete_DepartmentObjective(DepartmentObjectiveViewModel vm)
        {
            DataLayer.DeleteObjective(vm.obj_id);
        }

        public Tuple<List<tbl_department_objectives>, int> Get_DepartmentObjectiveTableContent(int departmentId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<tbl_department_objectives> all_items = DataLayer.get_Department_Objectives(departmentId);

            //filtering 
            List<tbl_department_objectives> filtered = new List<tbl_department_objectives>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.objective.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.objective).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.objective).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<tbl_department_objectives>, int>(filtered, all_items.Count);
        }

        #endregion DepartmentObjectiveController
    }
}