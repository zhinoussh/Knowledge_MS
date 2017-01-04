using Knowledge_Management.Areas.Admin.ViewModels;
using Knowledge_Management.Areas.User.ViewModels;
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

        public List<SelectListItem> GetJobList(int departmentId)
        {
            List<tbl_job> jobs = DataLayer.get_Jobs(departmentId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            foreach (tbl_job j in jobs)
            {
                lst_obj.Add(new SelectListItem { Value = j.pkey + "", Text = j.job_name });
            }

            return lst_obj;
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

        #region StrategyController
      
        public void Post_Add_Edit_Strategy(StrategyViewModel s)
        {
            DataLayer.InsertStrategy(s.strategy_id, s.strategy_name);
        }

        public StrategyViewModel Get_Delete_Strategy(int StrategyId)
        {
            StrategyViewModel s = new StrategyViewModel();
            s.strategy_id = StrategyId;

            return s;
        }

        public void Post_Delete_Strategy(StrategyViewModel vm)
        {
            DataLayer.DeleteStrategy(vm.strategy_id);
        }

        public Tuple<List<tbl_strategy>, int> Get_StrategyTableContent(string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<tbl_strategy> all_items = DataLayer.get_all_strategies();

            //filtering 
            List<tbl_strategy> filtered = new List<tbl_strategy>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.strategy_name.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.strategy_name).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.strategy_name).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<tbl_strategy>, int>(filtered, all_items.Count);

        }
        
        #endregion StrategyController

        #region EntrybyDetailController

        public DetailQuestionViewModel Get_EntrybyDetail_Index_Page()
        {
            DetailQuestionViewModel o = new DetailQuestionViewModel();

            List<tbl_department> deps = DataLayer.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey;

            List<tbl_job> jobs = DataLayer.get_Jobs(o.dep_id);
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = jobs.First().pkey;


            List<tbl_department_objectives> dep_objs = DataLayer.get_Department_Objectives(o.dep_id);
            o.lst_dep_objective = new SelectList(dep_objs, "pkey", "objective");
            o.dep_obj_id = 0;

            List<tbl_strategy> strategies = DataLayer.get_all_strategies();
            o.lst_strategy = new SelectList(strategies, "pkey", "strategy_name");
            o.strategy_id = 0;

            List<tbl_job_description> jobDescs = DataLayer.get_JobDescriptions(o.job_id);
            o.lst_job_desc = new SelectList(jobDescs, "pkey", "job_desc");
            o.job_desc_id = 0;

            return o;
        }

        public DetailQuestionViewModel Get_Delete_QuestionbyDetail(int questionId)
        {
            DetailQuestionViewModel q = new DetailQuestionViewModel();
            q.question_id = questionId;

            return q;
        }

        public void Post_Delete_QuestionbyDetail(DetailQuestionViewModel vm)
        {
            DataLayer.DeleteQuestion(vm.question_id);
        }

        public Tuple<List<QuestionViewModel>, int> Get_QuestionbyDetailTableContent(int depOjectiveId, int jobDescriptionId, int strategyId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();

            all_items = DataLayer.get_all_Questions_by_details(jobDescriptionId, depOjectiveId, strategyId);


            //filtering 
            List<QuestionViewModel> filtered = new List<QuestionViewModel>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.question.Contains(filter)
                                             || i.lst_keywords.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.question).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.question).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<QuestionViewModel>, int>(filtered, all_items.Count);

        }

        public List<SelectListItem> GetJobDescriptionList(int jobId)
        {
            List<tbl_job_description> desc = DataLayer.get_JobDescriptions(jobId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            lst_obj.Add(new SelectListItem { Value = "0", Text = "----" });

            foreach (tbl_job_description jd in desc)
            {
                lst_obj.Add(new SelectListItem { Value = jd.pkey + "", Text = jd.job_desc });
            }

            return lst_obj;
        }

        public List<SelectListItem> GetDepartmentObjectioveList(int departmentId)
        {
            List<tbl_department_objectives> objectives = DataLayer.get_Department_Objectives(departmentId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            lst_obj.Add(new SelectListItem { Value = "0", Text = "----" });

            foreach (tbl_department_objectives o in objectives)
            {
                lst_obj.Add(new SelectListItem { Value = o.pkey + "", Text = o.objective });
            }

            return lst_obj;
        }

        #endregion EntrybyDetailController

        #region EntrybyJobController

        public JobDepQuestionViewModel Get_EntrybyJob_Index_Page()
        {
            JobDepQuestionViewModel o = new JobDepQuestionViewModel();

            List<tbl_department> deps = DataLayer.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey + "";

            List<tbl_job> jobs = DataLayer.get_Jobs(Int32.Parse(o.dep_id));
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = "0";

            return o;
        }

        JobDepQuestionViewModel Get_Delete_QuestionbyJob(int questionId)
        {
            JobDepQuestionViewModel q = new JobDepQuestionViewModel();
            q.question_id = questionId;

            return q;
        }

        public void Post_Delete_QuestionbyJob(JobDepQuestionViewModel vm)
        {
            DataLayer.DeleteQuestion(vm.question_id);
        }

        public Tuple<List<QuestionViewModel>, int> Get_QuestionbyJobTableContent(int departmentId, int jobId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();

            if (jobId != 0)
            {
                all_items = DataLayer.get_all_Questions_by_job(jobId);
            }
            else if (departmentId != 0)
                all_items = DataLayer.get_all_Questions_by_alljobs_department(departmentId);


            //filtering 
            List<QuestionViewModel> filtered = new List<QuestionViewModel>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.question.Contains(filter)
                                             || i.lst_keywords.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.question).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.question).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<QuestionViewModel>, int>(filtered, all_items.Count);
        }

        #endregion EntrybyJobController

        #region EntrybyEmployeeController

        //Question
        public EmployeeQuestionViewModel Get_Question_Index_Page(int employeeId)
        {
            EmployeeQuestionViewModel vm = new EmployeeQuestionViewModel();
            if (employeeId != 0)
            {
                List<string> emp_props = DataLayer.get_Employee_byId(employeeId);
                vm.emp_id = employeeId;
                vm.Description = "Defined by  " + emp_props[1] + " Personel Code: " + emp_props[0];
            }

            return vm;
        }

        public Tuple<List<QuestionViewModel>, int> Get_QuestionbyEmployeeTableContent(int employeeId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();

            if (employeeId != 0)
            {
                List<string> emp_props = DataLayer.get_Employee_byId(employeeId);
                all_items = DataLayer.get_all_Questions_by_employee(emp_props[0]);
            }

            //filtering 
            List<QuestionViewModel> filtered = new List<QuestionViewModel>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.question.Contains(filter)
                                             || i.lst_keywords.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.question).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.question).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<QuestionViewModel>, int>(filtered, all_items.Count);
        }

        public EmployeeQuestionViewModel Get_Delete_QuestionbyEmployee(int questionId)
        {
            EmployeeQuestionViewModel q = new EmployeeQuestionViewModel();
            q.question_id = questionId;

            return q;
        }

        public void Post_Delete_QuestionbyEmployee(EmployeeQuestionViewModel vm)
        {
            DataLayer.DeleteQuestion(vm.question_id);
        }

        //Solution
        public SolutionViewModel Get_QuestionSolutions_Index_Page(int questionId)
        {
            SolutionViewModel vm = new SolutionViewModel();
            if (questionId != 0)
            {
                vm.question = DataLayer.get_question_name(questionId);
                vm.question_id = questionId;
                vm.employee_prop = "Defined by: " + DataLayer.get_Question_Writer(questionId);
            }

            return vm;
        }

        public EmployeeQuestionViewModel Get_EmployeeSolutions_Index_Page(int employeeId)
        {
            EmployeeQuestionViewModel vm = new EmployeeQuestionViewModel();
            if (employeeId != 0)
            {
                List<string> emp_props = DataLayer.get_Employee_byId(employeeId);
                vm.emp_id = employeeId;
                vm.Description = "Defined by  " + emp_props[1] + " Personel Code: " + emp_props[0];
            }

            return vm;
        }

        public Tuple<List<SolutionEmployeeViewModel>, int> Get_SolutionForQuestionTableContent(int questionId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<SolutionEmployeeViewModel> all_items = DataLayer.get_Solutions_by_Question(questionId, 0);

            //filtering 
            List<SolutionEmployeeViewModel> filtered = new List<SolutionEmployeeViewModel>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.solution.Contains(filter)).ToList();
            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.solution).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.solution).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<SolutionEmployeeViewModel>, int>(filtered, all_items.Count);
        }

        public Tuple<List<SolutionEmployeeViewModel>, int> Get_SolutionForEmployeeTableContent(int employeeId, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<SolutionEmployeeViewModel> all_items = DataLayer.get_Solutions_by_employee(employeeId);

            //filtering 
            List<SolutionEmployeeViewModel> filtered = new List<SolutionEmployeeViewModel>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.solution.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.solution).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.solution).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<SolutionEmployeeViewModel>, int>(filtered, all_items.Count);
        }

        public SolutionEmployeeViewModel Get_Delete_Solution(int solutionId)
        {
            SolutionEmployeeViewModel q = new SolutionEmployeeViewModel();
            q.solution_id = solutionId;

            return q;
        }

        public void Post_Delete_Solution(SolutionEmployeeViewModel vm,Controller ctrl)
        {
            long id_solution = vm.solution_id;

            List<tbl_solution_uploads> lst_uploads = DataLayer.get_uploads_by_solution(id_solution, 0);
            //delete uploaded files
            foreach (var item in lst_uploads)
            {
                System.IO.File.Delete(ctrl.Server.MapPath(@"~/Upload/" + item.file_path));
            }

            //delete solution and upload from db
            DataLayer.Delete_Solution(id_solution);
        }

        public FullSolutionViewModel Get_FullSolution(int solutionId)
        {
            FullSolutionViewModel vm = new FullSolutionViewModel();
            vm = DataLayer.get_Solution_by_id(solutionId);
            vm.solution_writer = "Defined by: " + vm.solution_writer;
            vm.question_writer = "Defined by: " + vm.question_writer;

            return vm;
        }

        public EmployeeQuestionViewModel Post_Confirm_Solution_for_Employee(int solutionId, int employeeId)
        {
            DataLayer.change_confirm_status_solution(solutionId);
            EmployeeQuestionViewModel vm = new EmployeeQuestionViewModel();
            List<string> emp_props = DataLayer.get_Employee_byId(employeeId);
            vm.emp_id = employeeId;
            vm.Description = "Defined by  " + emp_props[1] + " Personel Code: " + emp_props[0];

            return vm;
        }

        public SolutionViewModel Post_Confirm_Solution_for_Question(int solutionId, int questionId)
        {
            DataLayer.change_confirm_status_solution(solutionId);
            SolutionViewModel vm = new SolutionViewModel();
            vm.question = DataLayer.get_question_name(questionId);
            vm.question_id = questionId;
            return vm;
        }

        public UploadViewModel Get_Delete_Upload(int uploadId)
        {
            UploadViewModel q = new UploadViewModel();
            q.upload_id = uploadId;
            return q;
        }

        public void Post_Delete_Upload(UploadViewModel vm, Controller ctrl)
        {
            System.IO.File.Delete(ctrl.Server.MapPath(@"~\Upload\" + DataLayer.get_file_path(vm.upload_id)));
            DataLayer.DeleteUpload(vm.upload_id);
        }

        public Tuple<List<tbl_solution_uploads>, int> Get_UploadForSolutionTableContent(int solutionId, int displayStart, int displayLength)
        {
            List<tbl_solution_uploads> filtered = DataLayer.get_uploads_by_solution(solutionId, 0);

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<tbl_solution_uploads>, int>(filtered, filtered.Count);
        }

        public Tuple<byte[], string> GetFilePropertie(int uploadId,Controller ctrl)
        {
            string file_name = DataLayer.get_file_path(uploadId);
            string file_path = ctrl.Server.MapPath(@"~\Upload\" + file_name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file_path);

            return new Tuple<byte[], string>(fileBytes, file_name);
        }

        public FullSolutionViewModel Post_Confirm_Upload(int uploadId, int solutionId)
        {
            DataLayer.change_confirm_status_upload(uploadId);
            FullSolutionViewModel vm = new FullSolutionViewModel();
            vm = DataLayer.get_Solution_by_id(solutionId);
            return vm;
        }

        #endregion EntrybyEmployeeController


        #region InsertInfoController

        public QuestionViewModel Get_NewQuestion_Page(int questionId, Controller ctrl)
        {
            QuestionViewModel o = new QuestionViewModel();

            List<string> emp_prop = DataLayer.get_Employee_prop(get_userName(ctrl));
            int dep_id = Int32.Parse(emp_prop[0]);
            int job_id = Int32.Parse(emp_prop[1]);


            List<tbl_department_objectives> dep_objs = DataLayer.get_Department_Objectives(dep_id);
            o.lst_dep_objective = new SelectList(dep_objs, "pkey", "objective");

            List<tbl_strategy> strategies = DataLayer.get_all_strategies();
            o.lst_strategy = new SelectList(strategies, "pkey", "strategy_name");

            List<tbl_job_description> jobDescs = DataLayer.get_JobDescriptions(job_id);
            o.lst_job_desc = new SelectList(jobDescs, "pkey", "job_desc");

            //insert
            if (questionId==0)
            {
                o.question_id = 0;
                o.question = "";
                o.lst_keywords = "";
                o.dep_obj_id = 0;
                o.strategy_id = 0;
                o.job_desc_id = 0;
            }
            //edit
            else
            {
                QuestionViewModel question = DataLayer.get_question_byId(questionId);
                o.question_id = questionId;
                o.question = question.question;
                o.lst_keywords = question.lst_keywords;
                o.strategy_id = question.strategy_id.HasValue ? question.strategy_id.Value : 0;
                o.dep_obj_id = question.dep_obj_id.HasValue ? question.dep_obj_id.Value : 0;
                o.job_desc_id = question.job_desc_id.HasValue ? question.job_desc_id.Value : 0;
            }

            return o;
        }

        public void Post_Add_Edit_Question(QuestionViewModel vm, Controller ctrl)
        {
            List<string> lst_keywords = new List<string>();
            int count_keywords = Int32.Parse(ctrl.Request["count"].ToString());
            for (int i = 1; i <= count_keywords; i++)
            {
                if (ctrl.Request["field" + i] != null)
                    lst_keywords.Add(ctrl.Request["field" + i].ToString());
            }

            DataLayer.InsertQuestion(vm.question_id, vm.question, vm.dep_obj_id, vm.job_desc_id, vm.strategy_id
                , lst_keywords, get_userName(ctrl));
        }

        public QuestionViewModel Get_Delete_Question(int questionId)
        {
            QuestionViewModel q = new QuestionViewModel();
            q.question_id = questionId;
            return q;
        }

        public void Post_Delete_Question(QuestionViewModel vm)
        {
            DataLayer.DeleteQuestion(vm.question_id);
        }

        public Tuple<List<QuestionViewModel>, int> Get_UserQuestionsTableContent(Controller ctrl, string filter, string sortDirection, int displayStart, int displayLength)
        {
            List<QuestionViewModel> all_items = DataLayer.get_all_Questions_by_employee(get_userName(ctrl));

            //filtering 
            List<QuestionViewModel> filtered = new List<QuestionViewModel>();

            if (!string.IsNullOrEmpty(filter))
            {
                filtered = all_items.Where(i => i.question.Contains(filter)).ToList();

            }
            else
                filtered = all_items;


            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.question).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.question).ToList();

            //pagination
            filtered = filtered.Skip(displayStart).Take(displayLength).ToList();

            return new Tuple<List<QuestionViewModel>, int>(filtered, all_items.Count);
        }

        #endregion InsertInfoController

        private string get_userName(Controller ctrl)
        {
            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = ctrl.HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            //string UserName =ctrl.User.Identity.Name;

            return UserName;
        }

    }
}