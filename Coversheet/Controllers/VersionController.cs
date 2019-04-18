using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace CSG.ProcessAutomation.Coversheet.WebApi.Controllers
{
    public class VersionController : ApiController
    {
        public IEnumerable<string> Get()
        {
            string mappedPath = HttpContext.Current.Server.MapPath("~/bin");
            Assembly assembly = Assembly.LoadFrom($"{mappedPath}\\WebApi.Coversheet.dll");
            string version = assembly.GetName().Version.ToString();
            string name = assembly.GetName().Name;

            return new string[]
            {
                "Name: Coversheet Api",
                $"Assembly Name: {name}",
                $"Version: {version}"
            };
        }
    }
}