using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Domus.Commands;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Commands;
using Domus.Web.UI.Models.Home;
using Rolstad.MVC.Errors;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for the application main page
    /// </summary>
    [HandleError]
    [HandleErrorAndLog]
    public class HomeController : Controller
    {
        /// <summary>
        /// Provider used for user data
        /// </summary>
        private readonly IDataProvider<User, string> _userProvider;

        private readonly ICommand<Request, ApplicationDetailsResponse> _applicationDetailsCommand;
        private readonly IFeatureUsageNotifier _featureUsageNotifier;

        /// <summary>
        /// Constructor with dependencies
        /// </summary>
        /// <param name="userProvider">Provider for obtaining users</param>
        /// <param name="applicationDetailsCommand">Details of the application</param>
        /// <param name="featureUsageNotifier"></param>
        public HomeController(IDataProvider<User,string> userProvider,
            ICommand<Request, ApplicationDetailsResponse> applicationDetailsCommand,
            IFeatureUsageNotifier featureUsageNotifier)
        {
            _userProvider = userProvider;
            _applicationDetailsCommand = applicationDetailsCommand;
            _featureUsageNotifier = featureUsageNotifier;
        }

        /// <summary>
        /// Main home page
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            _featureUsageNotifier.Notify(Feature.HomeIndex);

            return View();
        }

        /// <summary>
        /// Page to Log In / Authenticate
        /// </summary>
        /// <returns></returns>
        public ViewResult LogOn()
        {
            _featureUsageNotifier.Notify(Feature.HomeLogon);
            var viewModel = new LogOnViewModel();

            return View(viewModel);
        }

        public ActionResult LogOff()
        {
            _featureUsageNotifier.Notify(Feature.HomeLogOff);

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Recipe");
        }

        /// <summary>
        /// Given user credentials, authenticates them (very simple implementation)
        /// </summary>
        /// <param name="viewModel">View Model with authentication data</param>
        /// <returns></returns>
        public ActionResult Authenticate(LogOnViewModel viewModel)
        {
            _featureUsageNotifier.Notify(Feature.HomeAuthenticate);

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

        [Authorize]
        public ActionResult ApplicationDetails()
        {
            _featureUsageNotifier.Notify(Feature.HomeApplicationDetails);

            var result = _applicationDetailsCommand.Execute(Domus.Commands.Request.Empty);
            return this.Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}
