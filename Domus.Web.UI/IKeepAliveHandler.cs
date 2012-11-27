using System;

namespace Domus.Web.UI
{
    public interface IKeepAliveHandler : IDisposable
    {
        DateTime ApplicationStartTime { get; set; }
    }
}