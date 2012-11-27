using System;
using System.Net;

namespace Domus.Web.UI
{
    public class KeepAliveHandler:IDisposable
    {
        private readonly string _url;
        public readonly DateTime ApplicationStartTime;

        public KeepAliveHandler()
        {
            _url = "http://recipes.rolstadfamily.com";

            ApplicationStartTime = DateTime.Now;
        }


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