using System;
using System.Linq;
using Domus.Adapters;
using FizzWare.NBuilder;
using NUnit.Framework;
using Rolstad.Extensions;

namespace Domus.Test.Adapters
{
    [TestFixture]
    public class When_converting_a_set_of_items
    {
        private Source[] _source;
        private Destination[] _result;

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
            var adapter = new AutoMapperAdapter<Source, Destination>();

            this._source = Builder<Source>.CreateListOfSize(10).Build().ToArray();
            this._result = adapter.Convert(this._source).ToArray();
        }


        [Test]
        public void Then_each_item_is_converted()
        {
            // Assert
            Assert.That(_result.Length, Is.EqualTo(_source.Length));
        }

        [Test]
        public void Then_the_source_id_is_converted()
        {
            // Assert
            _result.Each(r => Assert.That(this.FindMatch(r).SourceId,Is.EqualTo(r.SourceId)));
        }

        [Test]
        public void Then_the_date_is_converted()
        {
            // Assert
            _result.Each(r => Assert.That(this.FindMatch(r).Date, Is.EqualTo(r.Date)));
        }

        [Test]
        public void Then_the_numeric_is_converted()
        {
            // Assert
            _result.Each(r => Assert.That(this.FindMatch(r).Numeric, Is.EqualTo(r.Numeric)));
        }

        private Source FindMatch(Destination source)
        {
            return _source.First(s => s.SourceId == source.SourceId);
        }

    }
}