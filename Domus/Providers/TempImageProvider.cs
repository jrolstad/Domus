using System;
using System.IO;
using System.Web.Helpers;

namespace Domus.Providers
{
    public class TempImageProvider
    {
        public virtual string Save(WebImage image, string fileName)
        {
            var path = GetFilePath(fileName);
            image.Save(path);

            return path;
        }

        public virtual void Delete(string fileName)
        {
            var path = GetFilePath(fileName);
            File.Delete(path);
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(Path.GetTempPath(), fileName);
        }

    }
}