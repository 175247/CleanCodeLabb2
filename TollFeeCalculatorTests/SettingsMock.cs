using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    public class SettingsMock : ISettings
    {
        public string DataFilePath { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/testData.txt";
        public string DataFilePathFail { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/enFinKo.txt";
    }
}
