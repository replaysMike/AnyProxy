using AnyProxy.Tests.TestObjects;
using NUnit.Framework;
using System;
using AnyProxy.Extensions;

namespace AnyProxy.Tests
{
    [TestFixture]
    public class ProxyProviderTests
    {
        [Test]
        public void Should_CreateProxy()
        {
            var objectToProxy = new BasicObject(1, "Name", DateTime.Now);
            var newObject = objectToProxy.Proxy();
            Assert.NotNull(newObject);
        }
    }
}
