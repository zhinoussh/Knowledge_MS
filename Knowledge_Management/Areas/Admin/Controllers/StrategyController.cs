using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.Admin.ViewModels;
using System;


namespace Knowledge_Management.Areas.Admin.Controllers
{
    
    [CustomAuthorize(Roles="Admin")]
    public class StrategyController : Controller
    {
        IKnowledgeMSSL serviceLayer;
        public StrategyController (IKnowledgeMSSL service)
	    {
            serviceLayer=service;
        }
        // GET: Show strategies
        public ActionResult Index()
        {
            return View(new StrategyViewModel());
        }

        //create a strategy
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_Strategy(StrategyViewModel s)
        {
            serviceLayer.Post_Add_Edit_Strategy(s);
            return Json(new { msg = "Strategy description inserted successfully." });
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Strategy(int id)
        {
            StrategyViewModel s = serviceLayer.Get_Delete_Strategy(id);
            return PartialView("_PartialDeleteStrategy", s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Strategy(StrategyViewModel s)
        {
            ModelState["strategy_name"].Errors.Clear();

            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_Strategy(s);
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

            Tuple<List<tbl_strategy>, int>  tbl_content=serviceLayer.Get_StrategyTableContent(request.sSearch
                , Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new { SID = s.pkey + "", SIndex = (index + 1) + "", SNAME = s.strategy_name });

            var result = from s in indexed_list  select new[] { s.SID, s.SIndex, s.SNAME};

             
            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords =tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}