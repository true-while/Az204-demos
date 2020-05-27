using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoAdsWeb.Models.MVC2App.Controllers;

namespace ContosoAdsWeb.Models
{
    using System;
    using System.Web.Mvc;
    using Microsoft.ApplicationInsights;

    namespace MVC2App.Controllers
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
        public class AiHandleErrorAttribute : HandleErrorAttribute
        {
            public override void OnException(ExceptionContext filterContext)
            {
                if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
                {
                    //If customError is Off, then AI HTTPModule will report the exception
                    if (filterContext.HttpContext.IsCustomErrorEnabled)
                    {
                        // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                        var ai = new TelemetryClient();
                        ai.TrackException(filterContext.Exception);
                    }
                }
                base.OnException(filterContext);
            }
        }
    }

}