using System.Web;
using System.Web.Mvc;

namespace CSG.ProcessAutomation.Coversheet.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
