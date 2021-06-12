using NUnit.Framework;
using Cross.Dates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cross.Dates.Tests
{
    [TestFixture()]
    public class DateServiceTests
    {
        private DateService _dateService;

        [SetUp]
        public void SetUp()
        {
            _dateService = new DateService();
        }

        [Test()]
        public void GetDateTest()
        {
            var expectedObj = new
            {
                date = DateTime.Now.Date
            };

            var actualDate = _dateService.GetDate();

            Assert.AreEqual(expectedObj.date, actualDate);
        }
    }
}