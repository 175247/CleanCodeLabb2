using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculatorTests;
using TollFeeCalculatorTests.Mocks;

namespace TollFeeCalculator
{
    public static class Factory
    {
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
