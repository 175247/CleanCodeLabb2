using System;

namespace TollFeeCalculator
{
    public static class Factory
    {
        public static DateTime[] CreateDateTimeArray(String[] dateStrings)
        {
            return new DateTime[dateStrings.Length - 1];
        }
    }
}
