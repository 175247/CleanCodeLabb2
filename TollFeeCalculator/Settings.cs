using System;

namespace TollFeeCalculator
{
    public class Settings
    {
        public string DataFilePath { get; } = Environment.CurrentDirectory + "../../../../testData.txt";
    }
}
