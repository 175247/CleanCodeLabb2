using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorTests
{
    public interface IFeeCalculatorMock
    {
        public void Run(string filePath);
        public string[] GetFileDataAsArray(string filePath);
        public DateTime[] ParseDateTimes(ref DateTime[] dates, in string[] unformattedData);
        public int CalculateCost(DateTime[] date);
        public int CalculateFeeFromTime(DateTime date);
        public bool Free(DateTime day);
    }
}
