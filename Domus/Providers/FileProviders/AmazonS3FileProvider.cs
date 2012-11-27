using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Rolstad.Extensions;

namespace Domus.Providers.FileProviders
{
    /// <summary>
    /// File Provider that uses Amazon S3 for file persistence
    /// </summary>
    public class AmazonS3FileProvider : IFileProvider
    {
        internal readonly AmazonS3 _s3Client;

        /// <summary>
        /// Default constructor for mocking
        /// </summary>
        protected AmazonS3FileProvider()
        {
            
        }

        /// <summary>
        /// Constructor with authentication attributes
        /// </summary>
        /// <param name="amazonAccessKey">Amazon Access Key</param>
        /// <param name="amazonSecretkey">Amazon Secret Key</param>
        public AmazonS3FileProvider(string amazonAccessKey, string amazonSecretkey)
            :this(new AmazonS3Client(amazonAccessKey,amazonSecretkey))
        {

        }

        /// <summary>
        /// Constructor with S3 client only
        /// </summary>
        /// <param name="s3Client">S3 client to perform the actions</param>
        public AmazonS3FileProvider(AmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        /// <summary>
        /// Given a file, saves it to a bucket in S3
        /// </summary>
        /// <param name="filePath">File to save</param>
        /// <param name="bucketName">Bucket to put it into</param>
        /// <returns>Url of where the file is located</returns>
        public string Save(string filePath, string bucketName)
        {
           
            var request = CreatePutRequest(filePath, bucketName);

            _s3Client.PutObject(request);

            return string.Format("http://s3.amazonaws.com/{1}/{0}",Path.GetFileName(filePath),bucketName);
        }

        /// <summary>
        /// Given a file and bucket, creates a request to put the file
        /// </summary>
        /// <param name="filePath">File to put</param>
        /// <param name="bucketName">Bucket to put the file into</param>
        /// <returns></returns>
        internal PutObjectRequest CreatePutRequest(string filePath, string bucketName)
        {
            return new PutObjectRequest()
                .WithAutoCloseStream(true)
                .WithBucketName(bucketName)
                .WithCannedACL(S3CannedACL.PublicRead)
                .WithFilePath(filePath);
        }
    }
}