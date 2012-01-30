using System;
using System.Web.Mvc;
using System.Web.Security;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Models.Home;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for the application main page
    /// </summary>
    [HandleError]
    public class HomeController : Controller
    {
        /// <summary>
        /// Provider used for user data
        /// </summary>
        private readonly IDataProvider<User, string> _userProvider;

        /// <summary>
        /// Constructor with dependencies
        /// </summary>
        /// <param name="userProvider">Provider for obtaining users</param>
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

        /// <summary>
        /// Page to Log In / Authenticate
        /// </summary>
        /// <returns></returns>
        public ViewResult LogOn()
        {
            var viewModel = new LogOnViewModel();

            return View(viewModel);
        }

        /// <summary>
        /// Given user credentials, authenticates them (very simple implementation)
        /// </summary>
        /// <param name="viewModel">View Model with authentication data</param>
        /// <returns></returns>
        public ActionResult Authenticate(LogOnViewModel viewModel)
        {
            // Try and get the related user
            var user = _userProvider.Get(viewModel.EmailAddress);

            // If the user is found and password matches, let them in
            if (user != null && string.Equals(user.Password,viewModel.Password,StringComparison.CurrentCultureIgnoreCase))
            {
                FormsAuthentication.SetAuthCookie(viewModel.EmailAddress,true);

                return RedirectToAction("Index", "Recipe");
            }

            // Otherwise return them to this save view
            return View("LogOn", viewModel);
        }
    }
}
