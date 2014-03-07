using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxMind.GeoIP2;

namespace Sammsar.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var reader = new DatabaseReader(@"C:\Users\hkwedir.MIS\Documents\Visual Studio 2013\Projects\Sammsar\Sammsar.Web\App_Data\GeoLite2-City.mmdb");
            var omni = reader.Omni("128.101.101.101");

            Console.WriteLine(omni.Country.IsoCode); // 'US'
            Console.WriteLine(omni.Country.Name); // 'United States'
            Console.WriteLine(omni.Country.Names["zh-CN"]); // '美国'

            Console.WriteLine(omni.MostSpecificSubdivision.Name); // 'Minnesota'
            Console.WriteLine(omni.MostSpecificSubdivision.IsoCode); // 'MN'

            Console.WriteLine(omni.City.Name); // 'Minneapolis'

            Console.WriteLine(omni.Postal.Code); // '55455'

            Console.WriteLine(omni.Location.Latitude); // 44.9733
            Console.WriteLine(omni.Location.Longitude); // -93.2323

            ViewBag.ip = Request.ServerVariables["REMOTE_ADDR"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}