using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TollFeeCalculator;
using TollFeeCalculator.Utilities;
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
            _sut = TestFactory.CreateMockFeeCalculator();
            _settings = TestFactory.CreateMockSettings();
        }
        
        [TestMethod]
        public void GettingFileDataAsArray_Should_ReturnAnArrayOfString_When_Called()
        {
            var expected = new string[2];
            var actual = _sut.GetFileDataAsArray(_settings.DataFilePath);
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void ParsingDateTimes_Should_ReturnDateTimeArrayWithParsedValues_When_CalledWithStringArray()
        {
            var dates = _sut.GetFileDataAsArray(_settings.DataFilePath);
            DateTime[] dateTimes = TestFactory.CreateDateTimeArray(dates.Length);

            var expected = new DateTime[]
            {
                new DateTime(2020, 6, 30, 6, 34, 0)
            };

            var actual = _sut.ParseDateTimes(ref dateTimes, in dates);
            Assert.AreEqual(expected[0], actual[1]);
        }

        [TestMethod]
        public void PassingDateTimeArray_Should_ReturnTotalCost_When_ChildMethodHasCalculatedValue()
        {
            var dates = _sut.GetFileDataAsArray(_settings.DataFilePath);
            var datesArray = TestFactory.CreateDateTimeArray(dates.Length);
            var formattedTestDates = _sut.ParseDateTimes(ref datesArray, in dates);
            var expected = 71;
            var actual = _sut.CalculateCost(formattedTestDates);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProductionFactory_Should_ReturnDateTimeArray_When_Called()
        {
            var stringArray = new string[1];
            var expected = new DateTime[1];
            var actual = Factory.CreateDateTimeArray(stringArray.Length);
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void TestFactory_Should_ReturnDateTimeArray_When_Called()
        {
            var stringArray = new string[1];
            var expected = new DateTime[1];
            var actual = TestFactory.CreateDateTimeArray(stringArray.Length);
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void TestFactory_Should_ReturnSettingsMock_When_Called()
        {
            var expected = typeof(SettingsMock);
            var actual = TestFactory.CreateMockSettings();
            Assert.AreEqual(expected, actual.GetType());
        }

        [TestMethod]
        public void TestFactory_Should_ReturnFeeCalculatorMock_When_Called()
        {
            var expected = typeof(FeeCalculatorMock);
            var actual = TestFactory.CreateMockFeeCalculator();
            Assert.AreEqual(expected, actual.GetType());
        }

        [TestMethod]
        public void DataFile_Should_ThrowException_When_FileNotFound()
        {
            Assert.ThrowsException<FileNotFoundException>(() => _sut.Run(_settings.InvalidDataFilePath));
        }

        [TestMethod]
        public void DataFile_Should_ContainProperDates_When_Read()
        {
            var dates = File.ReadAllText(_settings.DataFilePath);
            string[] soloDate = new string[1];

            if (dates.Length > 15)
                soloDate[0] = dates.Substring(0, 16);
            else
                Assert.Fail();

            DateTime[] dateTimes = TestFactory.CreateDateTimeArray(1);
            var expected = new DateTime[]
            { 
                new DateTime(2020, 6, 30, 0, 5, 0) 
            };
            var actual = _sut.ParseDateTimes(ref dateTimes, in soloDate);
            Assert.AreEqual(expected[0], actual[0]);
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
            Assert.AreEqual(expectedFee, _sut.CalculateFeeFromTime(new DateTime(year, month, day, hour, minute, second)));
        }

        [TestMethod]
        public void Program_Should_RunToEnd_When_FileIsFound()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                var expected = "The total fee for the inputfile is 71";
                _sut.Run(_settings.DataFilePath);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }
    }
}
