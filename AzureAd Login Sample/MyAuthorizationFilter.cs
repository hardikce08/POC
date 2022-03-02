using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using POC.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureAd_Login_Sample
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            // In case you need an OWIN context, use the next line, `OwinContext` class
            // is the part of the `Microsoft.Owin` package.
            // var owinContext = new OwinContext(context.GetOwinEnvironment());
            HttpCookieCollection cookies = System.Web.HttpContext.Current.Request.Cookies;
            if (cookies["UserToken"] != null)
            {
                UserService us = new UserService();
                bool IsAuthorise = us.HasUserHangfireAccess(cookies["UserEmail"].Value);
                if (!IsAuthorise)
                {
                    System.Web.HttpContext.Current.Response.Redirect("/Error/NotFound");
                }
                return true;
            }
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return false;
        }
    }
}