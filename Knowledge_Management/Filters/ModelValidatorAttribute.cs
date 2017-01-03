using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management
{
    public class ModelValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            base.OnActionExecuting(filterContext);
        }
    }
}