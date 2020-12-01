using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    class SettingsMock : ISettings
    {
        public string DataFilePath { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/testData.txt";
    }
}
