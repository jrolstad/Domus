using System.Web.Helpers;

namespace Domus.Providers
{
    public interface IImageProvider
    {
        /// <summary>
        /// Saves an image to the temp directory with a given name
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="fileName">Name of the file to save as</param>
        /// <returns>Where the image is saved at</returns>
        string Save(WebImage image, string fileName);

        /// <summary>
        /// Deletes a file in the temp directory with the given name
        /// </summary>
        /// <param name="fileName"></param>
        void Delete(string fileName);

        /// <summary>
        /// Gets the full path to a file in the temp directory
        /// </summary>
        /// <param name="fileName">Name of the file to get</param>
        /// <returns></returns>
        string GetFilePath(string fileName);
    }
}