using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.Admin.ViewModels;



namespace Knowledge_Management.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private IKnowledgeMSSL serviceLayer;
        public EmployeeController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
        // GET: Show Employees
        public ActionResult Index()
        {
            EmployeeViewModel o = serviceLayer.Get_Employee_Index_Page();
            return View(o);
        }

        //create a Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_Employee(EmployeeViewModel s)
        {
            Tuple<int, string> result = serviceLayer.Post_Add_Edit_Employee(s);
            return Json(new { msg = result.Item2, result = result.Item1 });
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Employee(int id)
        {
            EmployeeViewModel e= serviceLayer.Get_Delete_Employee(id);

            return PartialView("_PartialDeleteEmp", e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Employee(EmployeeViewModel s)
        {
            ModelState["first_name"].Errors.Clear();
            ModelState["last_name"].Errors.Clear();
            ModelState["personel_code"].Errors.Clear();
            ModelState["pass"].Errors.Clear();

            if (ModelState.IsValid)
            {

                return Json(new { msg = "Employee deleted successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_EmployeeErr", "error in deleting Employee");
            }
            return View(s);

        }


        public JsonResult FillJobs(int DepId)
        {
            List<tbl_job> lst_obj = serviceLayer.GetJobist(DepId);
            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult EmployeeAjaxHandler(jQueryDataTableParamModel request)
        {

            Tuple<List<Employee>, int> tbl_content=serviceLayer.Get_EmployeeTableContent(request.sSearch,Convert.ToInt32(Request["iSortCol_0"])
                                                ,Request["sSortDir_0"],request.iDisplayStart,request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.Emp_Id + "",
                SIndex = (index + 1) + ""
                , FNAME = s.Emp_fname , LNAME = s.Emp_lname , Pcode = s.Emp_pcode,DepName=s.Dep_Name,DepId=s.Dep_Id+""
                ,Jobname=s.Job_Name,JobId=s.Job_Id+"",Dt_Entry=s.data_entry+"",DT_View=s.data_view+""});

            var result = from s in indexed_list
                         select new[] { s.SID, s.DepId, s.JobId, s.SIndex, s.FNAME, s.LNAME, s.Pcode
                             , s.DepName, s.Jobname,s.Dt_Entry,s.DT_View };
            
            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}