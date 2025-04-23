using System.Web;
using System.Web.Mvc;

namespace GFadda_Local_Theatre_Company
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
