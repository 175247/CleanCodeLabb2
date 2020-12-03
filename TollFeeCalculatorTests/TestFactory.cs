using System;
using TollFeeCalculatorTests;
using TollFeeCalculatorTests.Mocks;

namespace TollFeeCalculator.Utilities
{
    public static class TestFactory
    {
        public static DateTime[] CreateDateTimeArray(int arrayLength)
        {
            return new DateTime[arrayLength];
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
