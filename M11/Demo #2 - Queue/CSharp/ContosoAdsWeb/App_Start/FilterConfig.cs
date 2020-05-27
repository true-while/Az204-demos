using System.Web;
using System.Web.Mvc;
using ContosoAdsWeb.Models.MVC2App.Controllers;

namespace ContosoAdsWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
