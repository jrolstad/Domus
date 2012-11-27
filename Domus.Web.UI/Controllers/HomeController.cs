using System.Web.Mvc;
using Domus.Commands;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Commands.Requests;
using Domus.Web.UI.Commands.Responses;
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
        private readonly ICommand<AuthenticateUserRequest, AuthenticateUserResponse> _authenticateUserCommand;
        private readonly ICommand<Request, ApplicationDetailsResponse> _applicationDetailsCommand;
        private readonly ICommand<SignOutUserRequest, SignOutUserResponse> _logOffCommand;
        private readonly IFeatureUsageNotifier _featureUsageNotifier;

        public HomeController(ICommand<AuthenticateUserRequest,AuthenticateUserResponse> authenticateUserCommand,
            ICommand<Request, ApplicationDetailsResponse> applicationDetailsCommand,
            ICommand<SignOutUserRequest,SignOutUserResponse> logOffCommand,
            IFeatureUsageNotifier featureUsageNotifier)
        {
            _authenticateUserCommand = authenticateUserCommand;
            _applicationDetailsCommand = applicationDetailsCommand;
            _logOffCommand = logOffCommand;
            _featureUsageNotifier = featureUsageNotifier;
        }

        public ViewResult Index()
        {
            _featureUsageNotifier.Notify(Feature.HomeIndex);

            return View();
        }

        public ViewResult LogOn()
        {
            _featureUsageNotifier.Notify(Feature.HomeLogon);

            var viewModel = new LogOnViewModel();

            return View(viewModel);
        }

        public ActionResult LogOff()
        {
            _featureUsageNotifier.Notify(Feature.HomeLogOff);

            _logOffCommand.Execute(new SignOutUserRequest());

            return RedirectToAction("Index", "Recipe");
        }

        public ActionResult Authenticate(LogOnViewModel viewModel)
        {
            _featureUsageNotifier.Notify(Feature.HomeAuthenticate);

            // Authenticate the user
            var request = new AuthenticateUserRequest {UserName = viewModel.EmailAddress, Password = viewModel.Password};
            var result = _authenticateUserCommand.Execute(request);

            // If the user is authenticated, let 'em in
            if (result.IsAuthenticated)
            {
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
