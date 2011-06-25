﻿using Cloney.Domain.Cloning;
using NUnit.Framework;

namespace Cloney.Domain.Tests.Cloning
{
    [TestFixture]
    public class FolderNamespaceExtractorBehavior
    {
        private FolderNamespaceExtractor extractor;


        [SetUp]
        public void SetUp()
        {
            extractor = new FolderNamespaceExtractor();
        }


        [Test]
        public void ExtractNamespace_ShouldReturnEmptyStringForNullOrEmptyPath()
        {
            string str = null;

            Assert.That(extractor.ExtractNamespace(str), Is.EqualTo(string.Empty));
            Assert.That(extractor.ExtractNamespace(""), Is.EqualTo(string.Empty));
        }

        [Test]
        public void ExtractNamespace_ShouldHandleMissingSlash()
        {
            const string str = "Foo.Bar";

            Assert.That(extractor.ExtractNamespace(str), Is.EqualTo("Foo.Bar"));
        }

        [Test]
        public void ExtractNamespace_ShouldHandleSingleSlash()
        {
            const string str = "c:\\Foo.Bar";

            Assert.That(extractor.ExtractNamespace(str), Is.EqualTo("Foo.Bar"));
        }

        [Test]
        public void ExtractNamespace_ShouldHandleMultipleSlashes()
        {
            const string str = "c:\\Development\\MyProject\\Foo.Bar";

            Assert.That(extractor.ExtractNamespace(str), Is.EqualTo("Foo.Bar"));
        }
    }
}