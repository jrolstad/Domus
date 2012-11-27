using System;
using Domus.Mappers;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace Domus.Test.Mappers
{
    [TestFixture]
    public class When_converting_from_one_type_to_another_with_the_same_names
    {
        private Source _source;
        private Destination _result;

        public class Source
        {
            public string SourceId { get; set; }

            public DateTime Date { get; set; }

            public decimal Numeric { get; set; }
        }

        public class Destination
        {
            public string SourceId { get; set; }

            public DateTime Date { get; set; }

            public decimal Numeric { get; set; }
        }

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            var adapter = new AutoMapperMapper<Source, Destination>();

            _source = Builder<Source>.CreateNew().Build();
            _result = adapter.Map(_source);
        }


        [Test]
        public void Then_the_source_id_is_converted()
        {
            // Assert
            Assert.That(_result.SourceId, Is.EqualTo(_source.SourceId));
        }

        [Test]
        public void Then_the_date_is_converted()
        {
            // Assert
            Assert.That(_result.Date, Is.EqualTo(_source.Date));
        }

        [Test]
        public void Then_the_numeric_is_converted()
        {
            // Assert
            Assert.That(_result.Numeric, Is.EqualTo(_source.Numeric));
        }

    }
}