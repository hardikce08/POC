using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace POC.Helper
{
    public static class SelectListHelper
    {
        public static MvcHtmlString SelectList_ReceiverFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, bool includeAll = true, bool includeNone = false) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_Receiver( ExpressionHelper.GetExpressionText(expression), (metadata.Model ?? "").ToString(), htmlAttributes, includeAll, includeNone);
        }
        public static MvcHtmlString SelectList_Receiver(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null, bool includeAll = true, bool includeNone = false)
        {
            var data = new Dictionary<string, string>();

            data.Add("-1", "-- Select Receiver --");
            data.Add("did:key:z6MkhUDRZpEmsxrjLGpDDmfMzvTp2Wji1PJRqWsi4EfnjJi4", "mining co.");
            data.Add("did:key:z6Mko5QGjK56EwM59qdjn99TrJqQFvNcokxwwCTUJ8sfiG4Z", "fabricator co.");
            data.Add("did:key:z6MkppW5RmMY58oYSgwypFa2bL8NgDT96gzqKwZtZwm7oC78", "carrier co.");
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        }
        public static System.Web.Mvc.SelectList ToSelectList<TEnum>(this TEnum obj, object selectedValue)
    where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return new SelectList(Enum.GetValues(typeof(TEnum))
            .OfType<Enum>()
            .Select(x => new SelectListItem
            {
                Text = Enum.GetName(typeof(TEnum), x),
                Value = (Convert.ToInt32(x))
                .ToString()
            }), "Value", "Text", selectedValue);
        }
    }
}
