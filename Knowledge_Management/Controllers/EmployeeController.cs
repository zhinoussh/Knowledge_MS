using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.ViewModels;
using Knowledge_Management.DAL;


namespace Knowledge_Management.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        // GET: Show Employees
        public ActionResult Index()
        {
            EmployeeViewModel o = new EmployeeViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            
            List<tbl_department> deps = DAL.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey + "";

            List<tbl_job> jobs = DAL.get_Jobs(Int32.Parse(o.dep_id));
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = jobs.First().pkey + "";

            o.emp_id = 0;
            o.first_name = "";
            o.last_name = "";
            o.personel_code = "";
          
            return View(o);
        }

        //create a Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_Employee(EmployeeViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

              int result=  DAL.InsertEmployee(s.emp_id, s.first_name, s.last_name, s.personel_code
                    , Int32.Parse(s.dep_id), Int32.Parse(s.job_id),s.pass,s.data_entry,s.data_view);

                if(result>0)
                    return Json(new { msg = "پرسنل با موفقیت ذخیره شد" ,result=1});
                else
                    return Json(new { msg = "این کد پرسنلی قبلا در سیستم ثبت شده است", result = -1 });

            }
            else
            {
                ModelState.AddModelError("ADD_EmployeeErr", "Employee length is exeeding");
            }
            return View(s);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Employee(int id)
        {
            EmployeeViewModel s = new EmployeeViewModel();
            s.emp_id = id;
            return PartialView("_PartialDeleteEmp", s);
        }


    
        public JsonResult FillJobs(int DepId)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<tbl_job> jobs = DAL.get_Jobs(DepId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            foreach (tbl_job j in jobs)
            {

                lst_obj.Add(new SelectListItem { Value=j.pkey+"",Text=j.job_name});
            }

            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Employee(EmployeeViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteEmployee(s.emp_id);
                return Json(new { msg = "پرسنل مورد نظر با موفقیت حذف شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_EmployeeErr", "error in deleting Employee");
            }
            return View(s);

        }

        public ActionResult EmployeeAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            List<Employee> all_items = DAL.get_Employees();

            //filtering 
            List<Employee> filtered = new List<Employee>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.Emp_fname.Contains(request.sSearch)
                                        || i.Emp_lname.Contains(request.sSearch)
                                        || i.Emp_pcode.Contains(request.sSearch)
                                        || i.Dep_Name.Contains(request.sSearch)
                                        || i.Job_Name.Contains(request.sSearch)
                    ).ToList();
            }
            else
                filtered = all_items;


            //sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Employee, string> orderingFunction = (c => sortColumnIndex == 2 ? c.Emp_fname :
                                                        sortColumnIndex == 3 ? c.Emp_lname :
                                                        sortColumnIndex == 4 ? c.Emp_pcode :
                                                        sortColumnIndex == 5 ? c.Dep_Name :
                                                         sortColumnIndex == 6 ? c.Dep_Name : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(orderingFunction).ToList();
            else
                filtered = filtered.OrderByDescending(orderingFunction).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.Emp_Id + "", SIndex = (index + 1) + ""
                , FNAME = s.Emp_fname , LNAME = s.Emp_lname , Pcode = s.Emp_pcode,DepName=s.Dep_Name,DepId=s.Dep_Id+""
                ,Jobname=s.Job_Name,JobId=s.Job_Id+"",Dt_Entry=s.data_entry+"",DT_View=s.data_view+""});

            var result = from s in indexed_list
                         select new[] { s.SID, s.DepId, s.JobId, s.SIndex, s.FNAME, s.LNAME, s.Pcode
                             , s.DepName, s.Jobname,s.Dt_Entry,s.DT_View };



            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}