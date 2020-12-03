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
            var dates = File.ReadAllText(_settings.DataFilePath);
            string[] soloDate = new string[1];
            
            if (dates.Length > 15)
                soloDate[0] = dates.Substring(0, 16);

            DateTime[] dateTimes = TestFactory.CreateDateTimeArray(1);

            var expected = new DateTime[]
            {
                new DateTime(2020, 6, 30, 0, 5, 0)
            };

            var actual = _sut.ParseDateTimes(ref dateTimes, in soloDate);
            Assert.AreEqual(expected.GetType(), actual.GetType());
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
            var expected = new SettingsMock();
            var actual = TestFactory.CreateMockSettings();
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void TestFactory_Should_ReturnFeeCalculatorMock_When_Called()
        {
            var expected = new FeeCalculatorMock();
            var actual = TestFactory.CreateMockFeeCalculator();
            Assert.AreEqual(expected.GetType(), actual.GetType());
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
                var expected = "The total fee for the inputfile is 71";
                _sut.Run(_settings.DataFilePath);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

    }
}
