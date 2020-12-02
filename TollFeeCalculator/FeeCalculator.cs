using System;
using System.IO;

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
            string[] unformattedDates = GetFileDataAsArray();
            DateTime[] dates = Factory.CreateDateTimeArray(unformattedDates.Length);
            ParseDateTimes(ref dates, in unformattedDates);
            int TotalCost = CalculateCost(dates);
            Console.Write("The total fee for the inputfile is {0}", TotalCost);
        }

        public string[] GetFileDataAsArray()
        {
            string fileData = File.ReadAllText(_settings.DataFilePath);
            string[] unformattedDates = fileData.Split(",");
            return unformattedDates;
        }

        public DateTime[] ParseDateTimes(ref DateTime[] dates, in string[] unformattedData)
        {
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(unformattedData[i]);
            }
            return dates;
        }

        public int CalculateCost(DateTime[] d)
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
