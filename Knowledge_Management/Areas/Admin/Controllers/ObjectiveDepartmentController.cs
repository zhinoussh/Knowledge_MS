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
    public class ObjectiveDepartmentController : Controller
    {
        // GET: ObjectiveDepartment
        public ActionResult Index(int id)
        {
            DepartmentObjectiveViewModel o = new DepartmentObjectiveViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            o.dep_id = id;
            o.dep_name = DAL.get_department_name(id);
            o.obj_id = 0;
            o.obj_name = "";
            return View(o);
        }
               

        //create a Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_Objective(DepartmentObjectiveViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.InsertObjective(s.obj_id, s.obj_name,s.dep_id);
                return Json(new { msg = "شرح هدف با موفقیت ذخیره شد" });
            }
            else
            {
                ModelState.AddModelError("ADD_ObjectiveErr", "Objective length is exeeding");
            }
            return View(s);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Objective(int id)
        {
            DepartmentObjectiveViewModel s = new DepartmentObjectiveViewModel();
            s.obj_id = id;
            return PartialView("_PartialDeleteObjective", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Objective(DepartmentObjectiveViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteObjective(s.obj_id);
                return Json(new { msg = "شرح هدف با موفقیت Delete شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_ObjectiveErr", "error in deleting Objective");
            }
            return View(s);

        }


        public ActionResult ObjectiveAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            int dep_id = Convert.ToInt32(Request["dep_id"].ToString());

            List<tbl_department_objectives> all_items = DAL.get_Department_Objectives(dep_id);

            //filtering 
            List<tbl_department_objectives> filtered = new List<tbl_department_objectives>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.objective.Contains(request.sSearch)).ToList();

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
                filtered = filtered.OrderBy(s => s.objective).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.objective).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + "", SNAME = s.objective });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.SNAME };

            //var result = new List<string[]>() {
            //        new string[] {"1","1", "Microsoft"},
            //        new string[] {"2", "2", "Google"},
            //        new string[] {"3", "3", "Gowi"}
            //        };

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count,
                iTotalDisplayRecords = all_items.Count,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }


    }
}