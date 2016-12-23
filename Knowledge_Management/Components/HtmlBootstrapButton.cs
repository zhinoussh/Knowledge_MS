using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management
{
    public static class HtmlBootstrapButton
    {
        public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string innerHtml, object htmlAttributes = null)
        {
            return BootstrapButton(htmlHelper, innerHtml, null, htmlAttributes);
        }

        public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string innerHtml, string cssClass
        , object htmlAttributes = null)
        {
            return BootstrapButton(htmlHelper, innerHtml, cssClass, null, htmlAttributes);
        }

        public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string innerHtml, string cssClass
           , string name, object htmlAttributes = null)
        {
            return BootstrapButton(htmlHelper, innerHtml, cssClass, name, HtmlCommon.ButtonType.button, htmlAttributes);
        }

        public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string innerHtml, string cssClass
            , string name, HtmlCommon.ButtonType buttonType, object htmlAttributes = null)
        {
            return BootstrapButton(htmlHelper, innerHtml, cssClass, name, buttonType, null, htmlAttributes);
        }
        

        public static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string innerHtml, string cssClass
          , string name, HtmlCommon.ButtonType buttonType,string title, object htmlAttributes = null)
        {
            TagBuilder tb = new TagBuilder("button");

            if (!string.IsNullOrEmpty(cssClass))
            {
                if (!cssClass.Contains("btn-"))
                {
                    cssClass = "btn-primary " + cssClass;
                }
            }
            else
                cssClass = "btn-primary";

            tb.AddCssClass(cssClass);
            tb.AddCssClass("btn");

            tb.InnerHtml = innerHtml;

            HtmlCommon.AddIdName(tb, name, "");

            switch (buttonType)
            {
                case HtmlCommon.ButtonType.button:
                    tb.MergeAttribute("type", "button");
                    break;
                case HtmlCommon.ButtonType.submit:
                    tb.MergeAttribute("type", "submit");
                    break;
                case HtmlCommon.ButtonType.reset:
                    tb.MergeAttribute("type", "reset");
                    break;
            }

            if(!string.IsNullOrEmpty(title))
                tb.MergeAttribute("title", title);

            tb.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(tb.ToString());

        }
    }
}