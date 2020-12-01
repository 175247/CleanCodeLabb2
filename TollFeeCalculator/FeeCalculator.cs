using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class FeeCalculator : IFeeCalculator
    {
        private readonly ISettings _settings;
        public FeeCalculator(ISettings settings)
        {
            _settings = settings;
        }

        public void Run()
        {
            string indata = System.IO.File.ReadAllText(_settings.DataFilePath);
            String[] dateStrings = indata.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length - 1];
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(dateStrings[i]);
            }
            Console.Write("The total fee for the inputfile is" + TotalFeeCost(dates));
        }

        public int TotalFeeCost(DateTime[] d)
        {
            int fee = 0;
            DateTime si = d[0]; //Starting interval
            foreach (var d2 in d)
            {
                long diffInMinutes = (d2 - si).Minutes;
                if (diffInMinutes > 60)
                {
                    fee += CalculateFeeFromTime(d2);
                    si = d2;
                }
                else
                {
                    fee += Math.Max(CalculateFeeFromTime(d2), CalculateFeeFromTime(si));
                }
            }
            return Math.Max(fee, 60);
        }

        public int CalculateFeeFromTime(DateTime timeOfToll)
        {
            if (Free(timeOfToll)) return 0;
            int hour = timeOfToll.Hour;
            int minute = timeOfToll.Minute;
            switch (hour)
            {
                case 6:
                    if (minute <= 29) return 8;
                    return 13;
                case 7:
                    return 18;
                case 8:
                    if (minute <= 29) return 13;
                    return 8;
                case 15:
                    if (minute <= 29) return 13;
                    return 18;
                case 16:
                    return 18;
                case 17:
                    return 13;
                case 18:
                    if (minute <= 29) return 8;
                    return 0;
                default:
                    if (hour >= 8 && hour <= 14) return 8;
                    return 0;
            }
        }

        //Gets free dates
        public bool Free(DateTime day)
        {
            return (int)day.DayOfWeek == 5 || (int)day.DayOfWeek == 6 || day.Month == 7;
        }
    }
}
