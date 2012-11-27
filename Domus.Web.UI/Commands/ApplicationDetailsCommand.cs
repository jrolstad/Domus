using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using Domus.Commands;
using Domus.Providers;
using Domus.Providers.Repositories;
using Domus.Web.UI.Models.Home;

namespace Domus.Web.UI.Commands
{
    public class ApplicationDetailsCommand:ICommand<Request,ApplicationDetailsResponse>
    {
        private readonly IKeepAliveHandler _keepAliveHandler;

        public ApplicationDetailsCommand(IKeepAliveHandler keepAliveHandler)
        {
            _keepAliveHandler = keepAliveHandler;
        }

        public ApplicationDetailsResponse Execute(Request userRequest)
        {
            var url = GetBaseUrl();
            var accessKey = ConfigurationManager.AppSettings["AWS_ACCESS_KEY"];
            var secretKey = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];

            var cacheTimeout = AmazonSimpleDbRecipeProvider.CacheDuration;

            var startTime = _keepAliveHandler.ApplicationStartTime;
            var applicationDuration = Clock.Now.Subtract(startTime);

            var serverName = Environment.MachineName;
            var userName = Environment.UserName;
            var userDomain = Environment.UserDomainName;
            var fullUserName = string.Format(@"{0}\{1}", userDomain, userName);
            var currentTime = Clock.Now.ToString(CultureInfo.CurrentUICulture);

            return new ApplicationDetailsResponse
                       {
                           BaseUrl = url,
                           AwsAccessKey = accessKey,
                           AwsSecretKey = secretKey,
                           CacheDuration = cacheTimeout,
                           ApplicationStartTime = startTime.ToString(CultureInfo.CurrentUICulture),
                           ApplicationUpTime = applicationDuration,
                           ServerName = serverName,
                           UserName = fullUserName,
                           CurrentTime = currentTime
                       };
        }

        private string GetBaseUrl()
        {
            if (HttpContext.Current == null)
                return null;

            var request = HttpContext.Current.Request;
            var baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";

            return baseUrl;
        }


    }
}