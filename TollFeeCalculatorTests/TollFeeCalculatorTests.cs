using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    [TestClass]
    public class TollFeeCalculatorTests
    {
        [TestMethod]
        public void DataFile_Should_ThrowException_When_FileNotFound()
        {
            var filePath = Environment.CurrentDirectory + "testData.txt";
            Action<String> actual = Program.run;
            Assert.ThrowsException<System.IO.FileNotFoundException>(() => actual(filePath));
        }

        [TestMethod]
        public void DataFile_Should_ContainProperDates_When_Read()
        {
            var filePath = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/testData.txt";
            var date = File.ReadAllText(filePath);
            string soloDate = null;

            if (date.Length > 300)
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
                var filePath = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/testData.txt";
                Console.SetOut(stringWriter);
                var expected = "The total fee for the inputfile is60";
                Program.run(filePath);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

    }
}
