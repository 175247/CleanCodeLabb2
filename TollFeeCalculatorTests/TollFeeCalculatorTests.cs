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

        [DataRow(8, 2020, 12, 4, 6, 5, 0, DisplayName = "Assures that between 6:00-6:29, a fee of 8 is calculated")]
        [DataRow(13, 2020, 12, 3, 6, 34, 0, DisplayName = "Assures that between 6:30-6:59, a fee of 13 is calculated")]
        [DataRow(18, 2020, 12, 1, 7, 36, 0, DisplayName = "Assures that between 7:00-7:59, a fee of 18 is calculated")]
        [DataRow(13, 2020, 12, 4, 8, 14, 0, DisplayName = "Assures that between 8:00-8:29, a fee of 13 is calculated")]
        [DataRow(8, 2020, 12, 2, 8, 50, 0, DisplayName = "Assures that after 8:29, a fee of 8 is calculated")]
        [DataRow(8, 2020, 12, 1, 13, 20, 0, DisplayName = "Assures that between 8:30-14:59, a fee of 8 is calculated")]
        [DataRow(13, 2020, 12, 7, 15, 5, 0, DisplayName = "Assures that between 15:00-15:29, a fee of 13 is calculated")]
        [DataRow(18, 2020, 12, 2, 15, 46, 0, DisplayName = "Assures that between 15:30-16:59, a fee of 18 is calculated")]
        [DataRow(13, 2020, 12, 3, 17, 25, 0, DisplayName = "Assures that between 17:00-17:59, a fee of 13 is calculated")]
        [DataRow(8, 2020, 12, 4, 18, 15, 0, DisplayName = "Assures that between 18:00-18:29, a fee of 8 is calculated")]
        [DataRow(0, 2020, 12, 4, 18, 50, 0, DisplayName = "Assures that between 18:30-5:59, a fee of 0 is calculated")]
        [DataRow(0, 2020, 12, 5, 8, 50, 0, DisplayName = "Assures that on a Saturday, a fee of 0 is calculated")]
        [DataRow(0, 2020, 12, 6, 14, 5, 0, DisplayName = "Assures that on a Sunday, a fee of 0 is calculated")]
        [DataRow(0, 2020, 7, 7, 9, 50, 0, DisplayName = "Assures that during July, on a weekday, a fee of 0 is calculated")]
        [DataTestMethod]
        public void CalculateFeeFromTime_Should_ReturnCorrectAmount_When_TimeIsGiven(int expectedFee,int year, int month, int day, int hour, int minute, int second)
        {
            Program program = new Program();

            Assert.AreEqual(expectedFee, program.CalculateFeeFromTime(new DateTime(year, month, day, hour, minute, second)));
        }

        [TestMethod]
        public void CheckIsFreePass_Should_ReturnCorrectBool_When_DateIsGiven()
        {
        }
    }
}
