using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;
using TollFeeCalculatorTests.Mocks;

namespace TollFeeCalculatorTests
{
    public class SettingsMock : ISettingsMock
    {
        public string DataFilePath { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/testData.txt";
        public string InvalidDataFilePath { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/enFinKo.txt";
    }
}
