using Amazon.S3.Model;
using Domus.Providers;
using Domus.Providers.FileProviders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class AmazonS3FileProviderTests
    {
        [Test]
        public void When_saving_an_image_then_it_is_sent_to_s3()
        {
            // Arrange
            var s3 = MockRepository.GenerateStub<Amazon.S3.AmazonS3>();

            var provider = new AmazonS3FileProvider(s3);

            // Act
            var result = provider.Save(@"C:\temp.txt", "MyTestBucket");

            // Assert
            s3.AssertWasCalled(s=>s.PutObject(Arg<PutObjectRequest>.Is.Anything));
            Assert.That(result, Is.EqualTo("http://s3.amazonaws.com/MyTestBucket/temp.txt"));
        }

        [Test]
        public void When_creating_an_instance_then_the_credentials_are_set()
        {
            // Act
            var provider = new AmazonS3FileProvider("foo", "bar");

            // Asert
            Assert.That(provider._s3Client,Is.Not.Null);
        }

        [Test]
        public void When_creating_a_request_to_send_a_file_then_is_set_for_the_file_and_bucket()
        {
            // Arrange
            var provider = new AmazonS3FileProvider(null);

            const string bucketName = "SomeBucket";
            const string filePath = @"C:\myfile.jpg";

            // Act
            var request = provider.CreatePutRequest(filePath, bucketName);

            // Assert
            Assert.That(request.BucketName,Is.EqualTo(bucketName));
            Assert.That(request.FilePath,Is.EqualTo(filePath));
            Assert.That(request.CannedACL,Is.EqualTo(S3CannedACL.PublicRead));
            Assert.That(request.AutoCloseStream,Is.True);
        }
    }
}