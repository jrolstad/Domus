namespace Domus.Providers
{
    public interface IFileProvider
    {
        string Save(string filePath, string bucketName);
    }
}