using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Knowledge_Management
{
    public static class HtmlCheckBox
    {
        public static MvcHtmlString AwesomeCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, bool>> bindingExpression, string innerHtml, object htmlAttributes = null)
        {
            return AwesomeCheckBoxFor(htmlHelper, bindingExpression, null, innerHtml, null, htmlAttributes);
        }

        public static MvcHtmlString AwesomeCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
          Expression<Func<TModel, bool>> bindingExpression, string id, string innerHtml, object htmlAttributes = null)
        {
            return AwesomeCheckBoxFor(htmlHelper, bindingExpression, id, innerHtml, null, htmlAttributes);
        }

        public static MvcHtmlString AwesomeCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel,bool>> bindingExpression
            ,string id,string innerHtml,string checkboxCss,object htmlAttributes =null)
        {
            StringBuilder sb = new StringBuilder(512);

            RouteValueDictionary rvd = new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            HtmlCommon.AddIdName(rvd, id, null);
          
            if (!string.IsNullOrEmpty(checkboxCss))
                checkboxCss = "checkbox " + checkboxCss;
            else
                checkboxCss = "checkbox";

            sb.Append("<div class='" + checkboxCss + "'>");
            sb.Append(InputExtensions.CheckBoxFor(htmlHelper, bindingExpression, rvd));
            sb.Append("<label for='" + id + "'>");
            sb.Append(innerHtml);
            sb.Append("</label>");
            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());

        }
    }
}