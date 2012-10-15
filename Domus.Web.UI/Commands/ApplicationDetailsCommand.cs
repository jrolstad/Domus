using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using Domus.Providers;
using Domus.Web.UI.Models.Home;

namespace Domus.Web.UI.Commands
{
    public class ApplicationDetailsCommand
    {
        private readonly KeepAliveHandler _keepAliveHandler;

        public ApplicationDetailsCommand(KeepAliveHandler keepAliveHandler)
        {
            _keepAliveHandler = keepAliveHandler;
        }

        public ApplicationDetailsResponse Execute()
        {
            var url = GetBaseUrl();
            var accessKey = ConfigurationManager.AppSettings["AWS_ACCESS_KEY"];
            var secretKey = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];

            var cacheTimeout = AmazonSimpleDbRecipeProvider.CacheDuration;

            var startTime = _keepAliveHandler.ApplicationStartTime;
            var applicationDuration = DateTime.Now.Subtract(startTime);

            var serverName = Environment.MachineName;
            var userName = Environment.UserName;
            var userDomain = Environment.UserDomainName;
            var fullUserName = string.Format(@"{0}\{1}", userDomain, userName);
            var currentTime = DateTime.Now.ToString(CultureInfo.CurrentUICulture);

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
                           CurrentTIme = currentTime
                       };
        }

        private string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";

            return baseUrl;
        }


    }
}