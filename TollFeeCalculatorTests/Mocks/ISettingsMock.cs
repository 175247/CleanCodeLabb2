using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorTests.Mocks
{
    public interface ISettingsMock
    {
        public string DataFilePath { get; }
        public string InvalidDataFilePath { get; }
    }
}
