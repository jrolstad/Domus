using System.IO;
using System.Web.Helpers;

namespace Domus.Providers.FileProviders
{
    /// <summary>
    /// Provider for persisting temporary images
    /// </summary>
    public class TempImageProvider : IImageProvider
    {
        /// <summary>
        /// Saves an image to the temp directory with a given name
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="fileName">Name of the file to save as</param>
        /// <returns>Where the image is saved at</returns>
        public virtual string Save(WebImage image, string fileName)
        {
            var path = GetFilePath(fileName);
            image.Save(path);

            return path;
        }

        /// <summary>
        /// Deletes a file in the temp directory with the given name
        /// </summary>
        /// <param name="fileName"></param>
        public virtual void Delete(string fileName)
        {
            var path = GetFilePath(fileName);
            File.Delete(path);
        }

        /// <summary>
        /// Gets the full path to a file in the temp directory
        /// </summary>
        /// <param name="fileName">Name of the file to get</param>
        /// <returns></returns>
        public string GetFilePath(string fileName)
        {
            return Path.Combine(Path.GetTempPath(), fileName);
        }

    }
}