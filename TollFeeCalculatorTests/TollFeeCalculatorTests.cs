using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    [TestClass]
    public class TollFeeCalculatorTests
    {
        private readonly FeeCalculator _sut;
        private readonly SettingsMock _settings;
        private readonly Settings _productionSettings;

        public TollFeeCalculatorTests()
        {
            _settings = new SettingsMock();
            _productionSettings = new Settings();
            _sut = new FeeCalculator();
        }

        [TestMethod]
        public void GettingFileDataAsArray_Should_ReturnAnArrayOfString_When_Called()
        {
            var expected = new string[2];
            var actual = _sut.GetFileDataAsArray(_settings.DataFilePath);
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void GettingFileData_Should_ThrowFileNotFound_When_InvalidFilePath() 
        {
            var invalidSearchPath = "Matterhorn Project - Moo!";
            Assert.ThrowsException<FileNotFoundException>(() => _sut.GetFileDataAsArray(invalidSearchPath));
        }

        [TestMethod]
        public void ParsingDateTimes_Should_ReturnDateTimeArrayWithParsedValues_When_CalledWithValidStringArray()
        {
            var dates = _sut.GetFileDataAsArray(_settings.DataFilePath);
            DateTime[] dateTimes = new DateTime[dates.Length];

            var expected = new DateTime[]
            {
                new DateTime(2020, 6, 30, 6, 34, 0)
            };

            var actual = _sut.ParseDateTimes(dates);
            Assert.AreEqual(expected[0], actual[1]);
        }

        [TestMethod]
        public void PassingInvalidDateTimeArray_Should_ThrowFormatException_WhenUnableToParse()
        {
            var invalidStringDataArray = new string[1] { "moo" };
            Assert.ThrowsException<FormatException>(() => _sut.ParseDateTimes(invalidStringDataArray));
        }

        [TestMethod]
        public void PassingDateTimeArray_Should_ReturnTotalCost_When_ChildMethodHasCalculatedValue()
        {
            var dates = _sut.GetFileDataAsArray(_settings.DataFilePath);
            var formattedTestDates = _sut.ParseDateTimes(dates);
            var expected = 55;
            var actual = _sut.CalculateCost(formattedTestDates);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HighestFee_Should_OnlyBeCounted_When_MorePassagesOccursWithinOneHour()
        {
            var passageArray = new[]
            {
                new DateTime(2020, 6, 30, 15, 05, 0),
                new DateTime(2020, 6, 30, 15, 29, 0),
                new DateTime(2020, 6, 30, 15, 35, 0)
            };

            var expected = 18;
            var actual = _sut.CalculateCost(passageArray);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFactory_Should_ReturnDateTimeArray_When_Called()
        {
            var stringArray = new string[1];
            var expected = new DateTime[1];
            var actual = new DateTime[stringArray.Length];
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void DataFile_Should_NotThrowException_When_FileIsFound()
        {
            string[] expected = new string[0];
            var actual = _sut.GetFileDataAsArray(_settings.DataFilePath);
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [TestMethod]
        public void DataFile_Should_ContainProperDates_When_Read()
        {
            var expected = new DateTime(2020, 6, 30, 0, 5, 0);
            var actual = _sut.ParseDateTimes(_sut.GetFileDataAsArray(_settings.DataFilePath))[0];
            Assert.AreEqual(expected, actual);
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
        public void CalculateFeeFromTime_Should_ReturnCorrectAmount_When_TimeIsGiven(int expectedFee, int year, int month, int day, int hour, int minute, int second)
        {
            Assert.AreEqual(expectedFee, _sut.CalculateFeeFromTime(new DateTime(year, month, day, hour, minute, second)));
        }

        [TestMethod]
        public void IsWithinSameHour_Should_PickTheCorrectAlternative_When_SentPreviousAndCurrentPassage()
        {
            var greaterHoursAndMinutes = new[]
            {
                new DateTime(2020, 6, 30, 14, 15, 0),
                new DateTime(2020, 6, 30, 15, 20, 0)
            };

            var expectedHigherHoursAndMinutes = 21;
            var actualHigherHoursAndMinutes = _sut.CalculateCost(greaterHoursAndMinutes);

            Assert.AreEqual(expectedHigherHoursAndMinutes, actualHigherHoursAndMinutes);

            var greaterHourLowerMinutes = new[]
            {
                new DateTime(2020, 6, 30, 15, 25, 0),
                new DateTime(2020, 6, 30, 16, 24, 0)
            };

            var expectedGreaterHoursLowerMinutes = 18;
            var actualGreaterHoursLowerMinutes = _sut.CalculateCost(greaterHourLowerMinutes);

            Assert.AreEqual(expectedGreaterHoursLowerMinutes, actualGreaterHoursLowerMinutes);
        }

        [TestMethod]
        public void Settings_Should_ReturnFilePath_When_UsingItsGetter()
        {
            var expected = Environment.CurrentDirectory + "../../../../testData.txt";
            var actual = _productionSettings.DataFilePath;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Program_Should_RunToEnd_When_FileIsFound()
        {
            // Dir/FilePath is different when running tests, so SettingsMock is used.
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                var expected = "The total fee for the inputfile is 55";
                _sut.Run(new SettingsMock().DataFilePath);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }
    }
}