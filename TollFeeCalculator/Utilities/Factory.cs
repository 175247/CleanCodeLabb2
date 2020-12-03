using System;

namespace TollFeeCalculator
{
    public static class Factory
    {
        public static DateTime[] CreateDateTimeArray(int arrayLength)
        {
            return new DateTime[arrayLength];
        }
    }
}
