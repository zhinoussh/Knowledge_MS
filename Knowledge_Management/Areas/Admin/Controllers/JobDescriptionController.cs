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
    public class JobDescriptionController : Controller
    {
        private IKnowledgeMSSL serviceLayer;

        public JobDescriptionController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
        // GET: JobDescription
        public ActionResult Index()
        {
            JobDescriptionViewModel o = serviceLayer.Get_JobDescription_Index_Page();
            return View(o);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_JobDesc(JobDescriptionViewModel s)
        {
            serviceLayer.Post_Add_Edit_JobDescription(s);
            return Json(new { msg = "Job Description inserted successfully." });
        }

        [HttpGet]
        public ActionResult Delete_JobDesc(int id)
        {
            JobDescriptionViewModel s = serviceLayer.Get_Delete_JobDescription(id);
            return PartialView("_PartialDeleteJobDesc", s);
        }

        public JsonResult FillJobs(int DepId)
        {
            List<SelectListItem> lst_obj = serviceLayer.GetJobList(DepId);
            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_JobDesc(JobDescriptionViewModel s)
        {
            ModelState["jobDesc"].Errors.Clear();

            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_JobDescription(s);
                return Json(new { msg = "Job Description deleted successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_JobDescriptionErr", "error in deleting JobDescription");
            }
            return View(s);

        }

        public ActionResult JobDescAjaxHandler(jQueryDataTableParamModel request)
        {
            int job_id = Convert.ToInt32(Request["job_id"].ToString());

            Tuple<List<tbl_job_description>, int> tbl_content = serviceLayer.Get_JobDescriptionTableContent(job_id, request.sSearch
                , Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + ""
                , JobDesc = s.job_desc });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.JobDesc};



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