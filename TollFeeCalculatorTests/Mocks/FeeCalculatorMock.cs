using System;
using TollFeeCalculator;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TollFeeCalculatorTests.Mocks;
using TollFeeCalculator.Utilities;

namespace TollFeeCalculatorTests
{
    public class FeeCalculatorMock : IFeeCalculatorMock
    {
        public void Run(string filePath)
        {
            string[] unformattedDates = GetFileDataAsArray(filePath);
            DateTime[] dates = Factory.CreateDateTimeArray(unformattedDates.Length);
            ParseDateTimes(ref dates, in unformattedDates);
            int TotalCost = CalculateCost(dates);
            Console.Write("The total fee for the inputfile is {0}", TotalCost);
        }

        public string[] GetFileDataAsArray(string filePath)
        {
            string fileData = File.ReadAllText(filePath);
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

        public int CalculateCost(DateTime[] day)
        {
            int fee = 0;
            DateTime singleDay = day[0];
            foreach (var times in day)
            {
                long diffInMinutes = (times - singleDay).Minutes;
                if (diffInMinutes > 60)
                {
                    fee += CalculateFeeFromTime(times);
                    singleDay = times;
                }
                else
                {
                    fee += Math.Max(CalculateFeeFromTime(times), CalculateFeeFromTime(singleDay));
                }
            }
            return Math.Min(fee, 60);
        }

        public int CalculateFeeFromTime(DateTime timeOfToll)
        {
            if (CheckFreeDates(timeOfToll)) return 0;
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
        public bool CheckFreeDates(DateTime timeOfToll) 
        {
            return timeOfToll.DayOfWeek == DayOfWeek.Saturday || timeOfToll.DayOfWeek == DayOfWeek.Sunday || timeOfToll.Month == 7;
        }
    }
}
