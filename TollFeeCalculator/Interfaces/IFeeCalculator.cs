using System;

namespace TollFeeCalculator
{
    public interface IFeeCalculator
    {
        public void Run();
        public string[] GetFileDataAsArray();
        public DateTime[] ParseDateTimes(ref DateTime[] dates, in string[] unformattedData);
        public int CalculateCost(DateTime[] date);
        public int CalculateFeeFromTime(DateTime date);
        public bool CheckFreeDates(DateTime day);
    }
}