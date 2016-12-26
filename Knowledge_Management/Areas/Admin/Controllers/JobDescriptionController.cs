using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.ViewModels;
using Knowledge_Management.DAL;



namespace Knowledge_Management.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class JobDescriptionController : Controller
    {
        // GET: JobDescription
        public ActionResult Index()
        {
            JobDescriptionViewModel o = new JobDescriptionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            
            List<tbl_department> deps = DAL.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey+"";

            List<tbl_job> jobs = DAL.get_Jobs(Int32.Parse(o.dep_id));
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = jobs.First().pkey + "";

            o.jobDesc_id = 0;
            o.jobDesc = "";
            return View(o);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_JobDesc(JobDescriptionViewModel s)
        {

            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.InsertJobDescription(s.jobDesc_id, s.jobDesc,Int32.Parse(s.job_id));
                return Json(new { msg = "شرح شغل با موفقیت ذخیره شد" });
            }
            else
            {
                ModelState.AddModelError("ADD_JobDescriptionErr", "JobDescription length is exeeding");
            }
            return View(s);
        }


        [HttpGet] // this action result returns the partial containing the modal
    public ActionResult Delete_JobDesc(int id)
        {
            JobDescriptionViewModel s = new JobDescriptionViewModel();
            s.jobDesc_id = id;
            return PartialView("_PartialDeleteJobDesc", s);
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
        public ActionResult Delete_JobDesc(JobDescriptionViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteJobDescription(s.jobDesc_id);
                return Json(new { msg = "شرح شغل مورد نظر با موفقیت Delete شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_JobDescriptionErr", "error in deleting JobDescription");
            }
            return View(s);

        }

        public ActionResult JobDescAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            int job_id = Convert.ToInt32(Request["job_id"].ToString());
            List<tbl_job_description> all_items = DAL.get_JobDescriptions(job_id);

            //filtering 
            List<tbl_job_description> filtered = new List<tbl_job_description>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.job_desc.Contains(request.sSearch)).ToList();
            }
            else
                filtered = all_items;


            //sorting
           
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s=>s.job_desc).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.job_desc).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + ""
                , JobDesc = s.job_desc });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.JobDesc};



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