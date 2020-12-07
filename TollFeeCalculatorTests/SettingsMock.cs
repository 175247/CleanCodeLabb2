using System;

namespace TollFeeCalculatorTests
{
    public class SettingsMock
    {
        public string DataFilePath { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/testData.txt";
        public string InvalidDataFilePath { get; } = Environment.CurrentDirectory + "../../../../../TollFeeCalculator/enFinKo.txt";
    }
}
