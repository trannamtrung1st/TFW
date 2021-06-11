using Infrastructure.Network;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Inventory
{
    [TestFixture]
    public class InventoryServiceTests
    {
        private InventoryService _service;
        private Mock<IWebClientWrapper> _webClientMock;

        private const string Address = "http://abc123.com/inventory/products/1/notifysaleoccured/";
        private const string Json = "{\"quantity\": 2}";

        [SetUp]
        public void SetUp()
        {
            _webClientMock = new Mock<IWebClientWrapper>();

            _webClientMock.Setup(o => o.Post(Address, Json));

            _service = new InventoryService(_webClientMock.Object);
        }

        [Test]
        public void TestNotifySaleOccuredShouldNotifyInventorySystem()
        {
            _service.NotifySaleOcurred(1, 2);

            _webClientMock.Verify(p => p.Post(Address, Json), Times.Once);
        }
    }
}
