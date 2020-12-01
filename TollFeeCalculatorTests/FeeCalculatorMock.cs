using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    public class FeeCalculatorMock : IFeeCalculator
    {
        public void Run(string filePath)
        {
            string indata = System.IO.File.ReadAllText(filePath);
            String[] dateStrings = indata.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length - 1];
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(dateStrings[i]);
            }
            Console.Write("The total fee for the inputfile is" + TotalFeeCost(dates));
        }

        public int TotalFeeCost(DateTime[] date)
        {
            int fee = 0;
            DateTime si = date[0]; //Starting interval
            foreach (var d2 in date)
            {
                long diffInMinutes = (d2 - si).Minutes;
                if (diffInMinutes > 60)
                {
                    fee += TollFeePass(d2);
                    si = d2;
                }
                else
                {
                    fee += Math.Max(TollFeePass(d2), TollFeePass(si));
                }
            }
            return Math.Max(fee, 60);
        }

        public int TollFeePass(DateTime date)
        {
            if (Free(date)) return 0;
            int hour = date.Hour;
            int minute = date.Minute;
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        public bool Free(DateTime day)
        {
            return (int)day.DayOfWeek == 5 || (int)day.DayOfWeek == 6 || day.Month == 7;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
