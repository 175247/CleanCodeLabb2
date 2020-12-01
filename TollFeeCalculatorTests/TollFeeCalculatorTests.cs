using Microsoft.VisualStudio.TestTools.UnitTesting;
using TollFeeCalculator;
using System;

namespace TollFeeCalculatorTests
{
    [TestClass]
    public class TollFeeCalculatorTests
    {
        [TestMethod]
        public void DataFile_Should_ThrowException_When_CannotBeFound()
        {
        }

        [TestMethod]
        public void DataFile_Should_ThrowException_When_DoesNotContainDataWithDate()
        {
        }

        [TestMethod]
        public void CalculateFeeFromTime_Should_ReturnCorrectAmount_When_TimeIsGiven()
        {
            Assert.AreEqual(8, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 6, 5, 0)));
            Assert.AreEqual(13, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 6, 34, 0)));
            Assert.AreEqual(18, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 7, 36, 0)));
            Assert.AreEqual(13, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 8, 14, 0)));
            Assert.AreEqual(8, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 8, 50, 0)));
            Assert.AreEqual(8, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 13, 20, 0)));
            Assert.AreEqual(13, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 15, 5, 0)));
            Assert.AreEqual(18, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 15, 46, 0)));
            Assert.AreEqual(13, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 17, 25, 0)));
            Assert.AreEqual(8, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 18, 15, 0)));
            Assert.AreEqual(0, Program.CalculateFeeFromTime(new DateTime(2020, 2, 4, 18, 50, 0)));
        }

        [TestMethod]
        public void CheckIsFreePass_Should_ReturnCorrectBool_When_DateIsGiven()
        {
        }
    }
}
