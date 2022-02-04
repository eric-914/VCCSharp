using System.Linq;
using NUnit.Framework;
using VCCSharp;

namespace VccSharp.Tests
{
    public class ExtensionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToChunks()
        {
            var data = Enumerable.Range(1, 105).Chunk(50).ToArray();

            Assert.That(data[0].Count, Is.EqualTo(50));
            Assert.That(data[1].Count, Is.EqualTo(50));
            Assert.That(data[2].Count, Is.EqualTo(5));
        }
    }
}