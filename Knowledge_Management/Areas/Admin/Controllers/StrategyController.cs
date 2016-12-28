using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.Admin.ViewModels;


namespace Knowledge_Management.Areas.Admin.Controllers
{
    
    [CustomAuthorize(Roles="Admin")]
    public class StrategyController : Controller
    {
        // GET: Show strategies
        public ActionResult Index()
        {
            return View(new StrategyViewModel());
        }

        //create a strategy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_Strategy(StrategyViewModel s)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.InsertStrategy(s.strategy_id,s.strategy_name);
                return Json(new { msg = "Strategy description inserted successfully."});
            }
            else
            {
                ModelState.AddModelError("ADD_StrategyErr", "strategy length is exeeding");
            }
            return View(s);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Strategy(int id)
        {
            StrategyViewModel s = new StrategyViewModel();
            s.strategy_id = id;
            return PartialView("_PartialDeleteStrategy", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Strategy(StrategyViewModel s)
        {
            ModelState["strategy_name"].Errors.Clear();

            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteStrategy(s.strategy_id);
                return Json(new { msg = "Strategy description deleted successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_StrategyErr", "error in deleting strategy");
            }
            return View(s);

        }

        public ActionResult StrategyAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            List<tbl_strategy> all_items = DAL.get_all_strategies();
             

           //filtering 
            List<tbl_strategy> filtered = new List<tbl_strategy>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.strategy_name.Contains(request.sSearch)).ToList();

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
                filtered = filtered.OrderBy(s => s.strategy_name).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.strategy_name).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new {SID= s.pkey + "", SIndex=(index + 1) + "",SNAME= s.strategy_name });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.SNAME};

            //var result = new List<string[]>() {
            //        new string[] {"1","1", "Microsoft"},
            //        new string[] {"2", "2", "Google"},
            //        new string[] {"3", "3", "Gowi"}
            //        };
             
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