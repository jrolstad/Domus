using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Rolstad.Extensions;
using log4net;
using log4net.Appender;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for the application main page
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Main home page
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View();
        }

        public ContentResult Logs()
        {
            var logs = LogManager
                .GetRepository()
                .GetAppenders()
                .Where(a => a is FileAppender)
                .Cast<FileAppender>()
                .Select(a => a.File);

            var logbuilder = new StringBuilder();
            logs.Each(l=>
                          {
                              var newFilename = Guid.NewGuid().ToString();
                              System.IO.File.Copy(l, newFilename);

                              var file = System.IO.File.ReadAllLines(newFilename);
                              file.Each(f => logbuilder.AppendLine(f));
                          });

            return Content(logbuilder.ToString());
        }

    }
}
