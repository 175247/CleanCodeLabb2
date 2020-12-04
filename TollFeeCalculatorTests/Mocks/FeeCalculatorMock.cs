using System;
using TollFeeCalculator;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TollFeeCalculatorTests.Mocks;
using TollFeeCalculator.Utilities;
using System.Collections.Generic;

namespace TollFeeCalculatorTests
{
    public class FeeCalculatorMock : IFeeCalculatorMock
    {
        public void Run(string filePath)
        {
            string[] unformattedDates = GetFileDataAsArray(filePath);
            DateTime[] dates = Factory.CreateDateTimeArray(unformattedDates.Length);
            ParseDateTimes(ref dates, in unformattedDates);
            SortDataArray(ref dates);

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
            try
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    dates[i] = DateTime.Parse(unformattedData[i]);
                    Console.WriteLine(dates[i]);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("File contained invalid data. {0}", exception);
            }

            return dates;
        }

        public DateTime[] SortDataArray(ref DateTime[] dates)
        {
            Array.Sort(dates);
            return dates;
        }

        public int CalculateCost(DateTime[] days)
        {
            int fee = 0;
            int currentIndex = 0;
            var actualTimesToCharge = new List<DateTime>();

            for(int i = 0; i < days.Length; i++)
            {
                if(days[i].Hour == days[i + 1].Hour && )
                {
                    var costOfPassageOne = CalculateFeeFromTime(days[i]);
                    var costOfPassageTwo = CalculateFeeFromTime(days[i + 1]);

                    if(costOfPassageOne > costOfPassageTwo)
                    {
                    actualTimesToCharge.Add(days[i]);
                    }
                    else
                    {
                        actualTimesToCharge.Add(days[i + 1]);
                    }
                }
            }


            foreach (var day in days)
            {
                bool IsWithinSameDay = days[currentIndex].Day == days[currentIndex + 1].Day ? true : false;

                if (!IsWithinSameDay)
                {
                    continue;
                }
                else
                {
                    if (IsWithinSameHour(days[currentIndex], days[currentIndex + 1]))
                    {
                        fee += Math.Max(CalculateFeeFromTime(days[currentIndex]), CalculateFeeFromTime(days[currentIndex + 1]));
                    }
                    else
                    {
                        fee += CalculateFeeFromTime(days[currentIndex + 1]);
                    }
                }
                currentIndex++;
            }
            return Math.Min(fee, 60);
        }

        public bool IsWithinSameHour(DateTime firstPassage, DateTime secondPassage)
        {
            DateTime firstPassageAddedHour = firstPassage.AddHours(1);
            bool isMoreThanOneHour = secondPassage.Hour > firstPassageAddedHour.Hour;

            if (isMoreThanOneHour)
            {
                return false;
            }
            else if (secondPassage.Hour > firstPassage.Hour
                && secondPassage.Minute > firstPassage.Minute)
            {
                return false;
            }
            else if (secondPassage.Hour > firstPassage.Hour
                && secondPassage.Minute < firstPassage.Minute)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public int CalculateFeeFromTime(DateTime timeOfToll)
        {
            if (CheckFreeDates(timeOfToll)) return 0;

            int hour = timeOfToll.Hour;
            int minute = timeOfToll.Minute;
            switch (hour)
            {
                case 6:
                    if (minute <= 29)
                    {
                        return 8;
                    }
                    else
                    {
                        return 13;
                    }
                case 7:
                    return 18;
                case 8:
                    if (minute <= 29)
                    {
                        return 13;
                    }
                    else
                    {
                        return 8;
                    }
                case 15:
                    if (minute <= 29)
                    {
                        return 13;
                    }
                    else
                    {
                        return 18;
                    }
                case 16:
                    return 18;
                case 17:
                    return 13;
                case 18:
                    if (minute <= 29)
                    {
                        return 8;
                    }
                    else
                    {
                        return 0;
                    }
                default:
                    if (hour >= 8 && hour <= 14)
                    {
                        return 8;
                    }
                    else
                    {
                        return 0;
                    }
            }
        }

        //Gets free dates
        public bool CheckFreeDates(DateTime timeOfToll)
        {
            return timeOfToll.DayOfWeek == DayOfWeek.Saturday || timeOfToll.DayOfWeek == DayOfWeek.Sunday || timeOfToll.Month == 7;
        }
    }
}
