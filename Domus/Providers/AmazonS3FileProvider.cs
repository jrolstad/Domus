using System;
using System.IO;
using Amazon.S3.Model;
using Rolstad.Extensions;

namespace Domus.Providers
{
    public class AmazonS3FileProvider
    {
        private readonly string _amazonAccessKey;
        private readonly string _amazonSecretkey;

        /// <summary>
        /// Default constructor for mocking
        /// </summary>
        protected AmazonS3FileProvider()
        {
            
        }

        public AmazonS3FileProvider(string amazonAccessKey, string amazonSecretkey)
        {
            _amazonAccessKey = amazonAccessKey;
            _amazonSecretkey = amazonSecretkey;
        }

        public string Save(string filePath, string bucketName)
        {
            var s3 = new Amazon.S3.AmazonS3Client(_amazonAccessKey,_amazonSecretkey);

            var request = new PutObjectRequest().WithAutoCloseStream(true)
                .WithBucketName(bucketName)
                .WithCannedACL(S3CannedACL.PublicRead)
                .WithFilePath(filePath);

            s3.PutObject(request);

            return "http://s3.amazonaws.com/{1}/{0}".StringFormat(Path.GetFileName(filePath),bucketName);
        }
    }
}