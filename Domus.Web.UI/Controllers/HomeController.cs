using System.Web.Mvc;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for the application main page
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Main home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
