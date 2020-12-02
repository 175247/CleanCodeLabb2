using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TollFeeCalculator;
using TollFeeCalculatorTests.Mocks;

namespace TollFeeCalculatorTests
{
    [TestClass]
    public class TollFeeCalculatorTests
    {
        private readonly IFeeCalculatorMock _sut;
        private readonly ISettingsMock _settings;
        public TollFeeCalculatorTests()
        {
            _sut = Factory.CreateMockFeeCalculator();
            _settings = Factory.CreateMockSettings();
        }

        [TestMethod]
        public void DataFile_Should_ThrowException_When_FileNotFound()
        {
            Assert.ThrowsException<FileNotFoundException>(() => _sut.Run(_settings.InvalidDataFilePath));
        }

        [TestMethod]
        public void DataFile_Should_ContainProperDates_When_Read()
        {
            var date = File.ReadAllText(_settings.DataFilePath);
            string soloDate = null;

            if (date.Length > 15)
                soloDate = date.Substring(0, 16);
            else
                Assert.Fail();

            DateTime dateTime = DateTime.Parse(soloDate);
            var dateTimeAsString = dateTime.ToString();
            dateTimeAsString = dateTimeAsString.Substring(0, dateTimeAsString.Length - 3);

            var expected = "2020-06-30 00:05";
            var actual = dateTimeAsString;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateFeeFromTime_Should_ReturnCorrectAmount_When_TimeIsGiven()
        {
            Assert.AreEqual(8, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 6, 5, 0)));
            Assert.AreEqual(13, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 6, 34, 0)));
            Assert.AreEqual(18, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 7, 36, 0)));
            Assert.AreEqual(13, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 8, 14, 0)));
            Assert.AreEqual(8, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 8, 50, 0)));
            Assert.AreEqual(8, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 13, 20, 0)));
            Assert.AreEqual(13, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 15, 5, 0)));
            Assert.AreEqual(18, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 15, 46, 0)));
            Assert.AreEqual(13, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 17, 25, 0)));
            Assert.AreEqual(8, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 18, 15, 0)));
            Assert.AreEqual(0, _sut.CalculateFeeFromTime(new DateTime(2020, 2, 4, 18, 50, 0)));
        }

        [TestMethod]
        public void Program_Should_RunToEnd_When_FileIsFound()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                var expected = "The total fee for the inputfile is60";
                _sut.Run(_settings.DataFilePath);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

    }
}
