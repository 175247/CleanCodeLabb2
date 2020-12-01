using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculatorTests;

namespace TollFeeCalculator
{
    public static class Factory
    {
        public static SettingsMock CreateMockSettings()
        {
            return new SettingsMock();
        }

        public static FeeCalculatorMock CreateMockFeeCalculator()
        {
            return new FeeCalculatorMock();
        }
    }
}
