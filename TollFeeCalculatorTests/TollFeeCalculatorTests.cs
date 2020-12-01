using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    [TestClass]
    public class TollFeeCalculatorTests
    {
        private readonly Action<String> RunMockProgram;
        private readonly SettingsMock _settings;
        public TollFeeCalculatorTests()
        {
            RunMockProgram = Factory.CreateMockFeeCalculator().Run;
            _settings = Factory.CreateMockSettings();
        }

        [TestMethod]
        public void DataFile_Should_ThrowException_When_FileNotFound()
        {
            Assert.ThrowsException<FileNotFoundException>(() => RunMockProgram(_settings.DataFilePathFail));
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
        public void Program_Should_RunToEnd_When_FileIsFound()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                var expected = "The total fee for the inputfile is60";
                RunMockProgram(_settings.DataFilePath);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

    }
}
