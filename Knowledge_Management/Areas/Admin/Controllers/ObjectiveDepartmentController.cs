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
    public class ObjectiveDepartmentController : Controller
    {
        private IKnowledgeMSSL serviceLayer;

        public ObjectiveDepartmentController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }

        // GET: ObjectiveDepartment
        public ActionResult Index(int id)
        {
            DepartmentObjectiveViewModel o = serviceLayer.Get_DepartmentObjective_Index_Page(id);
            return View(o);
        }
               

        //create a Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_Objective(DepartmentObjectiveViewModel s)
        {
            serviceLayer.Post_Add_Edit_DepartmentObjective(s);
            return Json(new { msg = "Objective for department inserted successfully." });
        }

        [HttpGet] 
        public ActionResult Delete_Objective(int id)
        {
            DepartmentObjectiveViewModel s = serviceLayer.Get_Delete_DepartmentObjective(id);
            return PartialView("_PartialDeleteObjective", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Objective(DepartmentObjectiveViewModel s)
        {
            ModelState["obj_name"].Errors.Clear();

            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_DepartmentObjective(s);
                return Json(new { msg = "Objective for department deleted successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_ObjectiveErr", "error in deleting Objective");
            }
            return View(s);

        }


        public ActionResult ObjectiveAjaxHandler(jQueryDataTableParamModel request)
        {

            int dep_id = Convert.ToInt32(Request["dep_id"].ToString());

           Tuple<List<tbl_department_objectives>,int> tbl_content= serviceLayer.Get_DepartmentObjectiveTableContent(dep_id, request.sSearch, Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

           var indexed_list = tbl_content.Item1.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + "", SNAME = s.objective });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.SNAME };

           
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