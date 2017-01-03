using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.Admin.ViewModels;
using System;



namespace Knowledge_Management.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private IKnowledgeMSSL serviceLayer;

        public DepartmentController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
        // GET: Show Departments
        public ActionResult Index()
        {
            return View(new DepartmentViewModel());
        }

        //create a Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_Department(DepartmentViewModel d)
        {
            serviceLayer.Post_Add_Edit_Department(d);
            return Json(new { msg = "Department inserted successfully." });
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Department(int id)
        {
           DepartmentViewModel vm= serviceLayer.Get_Delete_Department(id);
           return PartialView("_PartialDeleteDep", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Delete_Department(DepartmentViewModel d)
        {
            ModelState["dep_name"].Errors.Clear();
            serviceLayer.Post_Add_Edit_Department(d);
            return Json(new { msg = "Department deleted successfully." });
        }



        public ActionResult DepartmentAjaxHandler(jQueryDataTableParamModel request)
        {
           Tuple<List<tbl_department>,int> tbl_content = serviceLayer.Get_DepartmentTableContent(request.sSearch,
                                            Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);


            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.pkey + "",
                SIndex = (index + 1) + ""
                , SNAME = s.department_name 
            });

            var result = from s in indexed_list select new[] { s.SID, s.SIndex, s.SNAME };

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords =  tbl_content.Item2,
                iTotalDisplayRecords =  tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }


    }
}