using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.Admin.ViewModels;



namespace Knowledge_Management.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        // GET: Show Departments
        public ActionResult Index()
        {
            return View(new DepartmentViewModel());
        }

        //create a Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_Department(DepartmentViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.InsertDepartment(s.dep_id, s.dep_name);
                return Json(new { msg = "واحد سازمانی با موفقیت ذخیره شد" });
            }
            else
            {
                ModelState.AddModelError("ADD_DepartmentErr", "Department length is exeeding");
            }
            return View(s);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Department(int id)
        {
            DepartmentViewModel s = new DepartmentViewModel();
            s.dep_id = id;
            return PartialView("_PartialDeleteDep", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Delete_Department(DepartmentViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteDepartment(s.dep_id);
               return Json(new { msg = "واحد سازمانی با موفقیت Delete شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_DepartmentErr", "error in deleting Department");
            }
            return View(s);

        }



        public ActionResult DepartmentAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            List<tbl_department> all_items = DAL.get_all_Departments();


            //filtering 
            List<tbl_department> filtered = new List<tbl_department>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.department_name.Contains(request.sSearch)).ToList();

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
                filtered = filtered.OrderBy(s => s.department_name).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.department_name).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + "", SNAME = s.department_name });

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