using System;
using System.Net;

namespace Domus.Web.UI
{
    public class KeepAliveHandler: IKeepAliveHandler
    {
        private readonly string _url;

        public KeepAliveHandler()
        {
            _url = "http://recipes.rolstadfamily.com";

            ApplicationStartTime = Clock.Now;
        }

        public DateTime ApplicationStartTime { get; set; }

        public void Dispose()
        {
            if(!string.IsNullOrWhiteSpace(_url))
            {
                var client = new WebClient();
                var data = client.DownloadString(_url);
            }
        }
    }
}