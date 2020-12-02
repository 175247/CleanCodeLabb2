using System;
using TollFeeCalculatorTests;
using TollFeeCalculatorTests.Mocks;

namespace TollFeeCalculator.Utilities
{
    public class TestFactory
    {
        public static DateTime[] CreateDateTimeArray(String[] dateStrings)
        {
            return new DateTime[dateStrings.Length - 1];
        }

        public static ISettingsMock CreateMockSettings()
        {
            return new SettingsMock();
        }

        public static IFeeCalculatorMock CreateMockFeeCalculator()
        {
            return new FeeCalculatorMock();
        }
    }
}
