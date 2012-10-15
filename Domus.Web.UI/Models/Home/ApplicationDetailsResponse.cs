using System;

namespace Domus.Web.UI.Models.Home
{
    public class ApplicationDetailsResponse
    {
        public string BaseUrl { get; set; }

        public string AwsAccessKey { get; set; }

        public string AwsSecretKey { get; set; }

        public TimeSpan CacheDuration { get; set; }

        public string ApplicationStartTime { get; set; }

        public TimeSpan ApplicationUpTime { get; set; }
    }
}