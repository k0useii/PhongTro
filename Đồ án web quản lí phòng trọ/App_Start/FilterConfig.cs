using System.Web;
using System.Web.Mvc;

namespace Đồ_án_web_quản_lí_phòng_trọ
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
