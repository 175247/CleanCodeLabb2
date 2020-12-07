using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Settings : ISettings
    {
        public string DataFilePath { get; set; } = Environment.CurrentDirectory + "../../../../testData.txt";
    }
}
