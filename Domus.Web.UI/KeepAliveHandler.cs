﻿using System;
using System.Net;
using System.Security.Policy;
using System.Web;

namespace Domus.Web.UI
{
    public class KeepAliveHandler:IDisposable
    {
        private readonly string _url;

        public KeepAliveHandler()
        {
            _url = HttpContext.Current.Request.Url.AbsoluteUri;
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