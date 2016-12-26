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
        // GET: Show Jobs
        public ActionResult Index()
        {
            JobViewModel o = new JobViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<tbl_department> deps = DAL.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.job_id = 0;
            o.job_name = "";
            o.selected_dep = deps.First().pkey+"";
            return View(o);
        }

        //create a Job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_Job(JobViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.InsertJob(s.job_id,s.job_name, Int32.Parse(s.selected_dep));
                return Json(new { msg = "شغل با موفقیت ذخیره شد" });
            }
            else
            {
                ModelState.AddModelError("ADD_JobErr", "Job length is exeeding");
            }
            return View(s);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Job(int id)
        {
            JobViewModel s = new JobViewModel();
            s.job_id = id;
            return PartialView("_PartialDeleteJob", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Job(JobViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteJob(s.job_id);
                return Json(new { msg = "شغل با موفقیت Delete شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_JobErr", "error in deleting Job");
            }
            return View(s);

        }

        public ActionResult JobAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            int dep_id = Convert.ToInt32(Request["dep_id"].ToString());
             List<tbl_job> all_items = DAL.get_Jobs(dep_id);

            //filtering 
            List<tbl_job> filtered = new List<tbl_job>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.job_name.Contains(request.sSearch)).ToList();
            }
            else
                filtered = all_items;


            //sorting
            //      var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<Company, string> orderingFunction = (c => sortColumnIndex == 1 ? c.Name :
            //                                            sortColumnIndex == 2 ? c.Address :
            //                                            c.Town);

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.job_name).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.job_name).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + "", SNAME = s.job_name });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.SNAME };



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