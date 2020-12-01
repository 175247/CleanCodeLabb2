﻿using System;

namespace TollFeeCalculator
{
    public interface IFeeCalculator
    {
        public void Run();
        public int TotalFeeCost(DateTime[] date);
        public int CalculateFeeFromTime(DateTime date);
        public bool Free(DateTime day);
    }
}