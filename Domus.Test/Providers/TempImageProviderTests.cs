using System.IO;
using Domus.Providers;
using NUnit.Framework;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class TempImageProviderTests
    {
        [Test]
        public void When_getting_a_file_path_then_it_is_in_the_temp_directory()
        {
            // Arrange
            var provider = new TempImageProvider();

            const string fileName = "someFile.txt";

            // Act
            var result = provider.GetFilePath(fileName);

            // Assert
            Assert.That(result,Is.EqualTo(Path.Combine(Path.GetTempPath(),fileName)));
        }

        [Test]
        public void When_deleting_a_file_then_it_is_deleted()
        {
            // Arrange
            var fileToDelete = Path.GetTempFileName();

            var provider = new TempImageProvider();

            // Act
            provider.Delete(fileToDelete);

            // Assert
            Assert.That(File.Exists(fileToDelete),Is.False);
        }
    }
}