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
    public class JobController : Controller
    {
        private IKnowledgeMSSL serviceLayer;
        public JobController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
        // GET: Show Jobs
        public ActionResult Index()
        {
            JobViewModel o = serviceLayer.Get_Job_Index_Page();
            return View(o);
        }

        //create a Job
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_Job(JobViewModel s)
        {
            serviceLayer.Post_Add_Edit_Job(s);
            return Json(new { msg = "Job title inserted successfully." });
        }

        [HttpGet] 
        public ActionResult Delete_Job(int id)
        {
            JobViewModel s = serviceLayer.Get_Delete_Job(id);
            return PartialView("_PartialDeleteJob", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Job(JobViewModel s)
        {
            ModelState["job_name"].Errors.Clear();

            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_Job(s);
                return Json(new { msg = "Job title deleted successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_JobErr", "error in deleting Job");
            }
            return View(s);

        }

        public ActionResult JobAjaxHandler(jQueryDataTableParamModel request)
        {
            int dep_id = Convert.ToInt32(Request["dep_id"].ToString());
            Tuple<List<tbl_job>,int> tbl_content=serviceLayer.Get_JobTableContent(dep_id, request.sSearch, Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);


            var indexed_list = tbl_content.Item1.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + "", SNAME = s.job_name });

            var result = from s in indexed_list select new[] { s.SID, s.SIndex, s.SNAME };

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