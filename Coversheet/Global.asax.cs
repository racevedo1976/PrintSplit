using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using BusinessInk.Common.Logging;
// ReSharper disable MemberCanBePrivate.Global

namespace CSG.ProcessAutomation.Coversheet.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static NLog.Logger Log;

        protected void Application_Start()
        {
            // Setup and configure logging support.
            bool traceEnabled = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TraceEnabled"]);
            NLog.LogManager.Configuration = LogConfiguration.CentralLogging(
                isInfoOn: true,
                isWarningOn: true,
                isTraceOn: traceEnabled,
                isDebugOn: false,
                isErrorOn: true
            );

            Log = NLog.LogManager.GetLogger("Coversheet API");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
