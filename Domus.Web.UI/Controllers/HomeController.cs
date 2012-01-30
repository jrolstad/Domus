using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Models.Home;
using Rolstad.Extensions;
using log4net;
using log4net.Appender;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for the application main page
    /// </summary>
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IDataProvider<User, string> _userProvider;

        public HomeController(IDataProvider<User,string> userProvider)
        {
            _userProvider = userProvider;
        }

        /// <summary>
        /// Main home page
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult LogOn()
        {
            var viewModel = new LogOnViewModel();

            return View(viewModel);
        }

        public ActionResult Authenticate(LogOnViewModel viewModel)
        {
            var user = _userProvider.Get(viewModel.EmailAddress);


            if (user != null
                && user.Password == viewModel.Password)
            {
                FormsAuthentication.SetAuthCookie(viewModel.EmailAddress,true);

                return RedirectToAction("Index", "Recipe");
            }

            return View("LogOn", viewModel);
        }
    }
}
