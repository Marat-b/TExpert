using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Text;

namespace TExp.Classes
{
    public static class ActionlinkWithSpanHelper
    {
        public static MvcHtmlString ActionlinkWithSpanForIcon(this HtmlHelper htmlHelper,string linkText,string actionName,string controllerName,object routeValues,object htmlAttributes,string spanAttributes)
        {
            string strActionLink = LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
            strActionLink = strActionLink.Replace(linkText, string.Format("<span class='{0}'></span>{1}", spanAttributes, linkText));
            /*var sb=new StringBuilder();
            if (routeValues != null)
            {
                RouteValueDictionary rvd = new RouteValueDictionary(routeValues);
                if (rvd.Count > 0)
                {
                    sb.Append("/?");
                    foreach (var rv in rvd)
                    {
                        sb.Append(rv.Key.ToString() + "=" + rv.Value + "&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            TagBuilder spanBuilder = new TagBuilder("a");
            if (routeValues!=null)
            { 
                spanBuilder.Attributes.Add("href",string.Format("/{0}/{1}{2}", controllerName,actionName,sb.ToString()));
            }
            spanBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            spanBuilder.InnerHtml = string.Format("<span class='{0}'></span>{1}", spanAttributes, linkText);*/

            return MvcHtmlString.Create(strActionLink);
        }

        public static MvcHtmlString ActionlinkWithSpanForIcon(this HtmlHelper htmlHelper, string linkText, string actionName,  object routeValues, object htmlAttributes, string spanAttributes)
        {
            string strActionLink = LinkExtensions.ActionLink(htmlHelper, linkText,  actionName,   routeValues,  htmlAttributes).ToString();
            /*var sb = new StringBuilder();
            sb.Append(string.Format("<span class='{0}'></span>{1}", spanAttributes, linkText));*/
            strActionLink = strActionLink.Replace(linkText, string.Format("<span class='{0}'></span>{1}", spanAttributes, linkText));
            
            /*if (routeValues != null)
            {
                RouteValueDictionary rvd = new RouteValueDictionary(routeValues);
                if (rvd.Count > 0)
                {
                    sb.Append("/?");
                    foreach (var rv in rvd)
                    {
                        sb.Append(rv.Key.ToString() + "=" + rv.Value + "&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
            } 
            TagBuilder spanBuilder = new TagBuilder("a");
            if (routeValues != null)
            {
                spanBuilder.Attributes.Add("href", string.Format("/{0}{1}",  actionName, sb.ToString()));
            } 
            spanBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            spanBuilder.InnerHtml = string.Format("<span class='{0}'></span>{1}", spanAttributes, linkText);*/

            return MvcHtmlString.Create(strActionLink);
        }

        public static MvcHtmlString ActionlinkWithSpanForIcon(this HtmlHelper htmlHelper,  string actionName, object routeValues, object htmlAttributes, string spanAttributes)
        {
            string strActionLink = LinkExtensions.ActionLink(htmlHelper, "null", actionName, routeValues, htmlAttributes).ToString();
            
            strActionLink = strActionLink.Replace("null", string.Format("<span class='{0}'></span>", spanAttributes ));

            return MvcHtmlString.Create(strActionLink);
        }
    }
}