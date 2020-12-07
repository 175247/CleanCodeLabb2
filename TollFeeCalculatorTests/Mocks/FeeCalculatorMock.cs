using System;
using TollFeeCalculator;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TollFeeCalculatorTests.Mocks;
using TollFeeCalculator.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace TollFeeCalculatorTests
{
    public class FeeCalculatorMock : IFeeCalculatorMock
    {
        public void Run(string filePath)
        {
            string[] unformattedDates = GetFileDataAsArray(filePath);
            DateTime[] tollPassages = ParseDateTimes(unformattedDates);
            SortDataArray(ref tollPassages);
            int TotalCost = CalculateCost(tollPassages);
            Console.Write("The total fee for the inputfile is {0}", TotalCost);
        }

        public string[] GetFileDataAsArray(string filePath)
        {
            string fileData = "";
            try
            {
                fileData = File.ReadAllText(filePath);
                string[] unformattedDates = fileData.Split(",");

                return unformattedDates;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Invalid filepath.");

                throw;
            }
        }

        public DateTime[] ParseDateTimes(string[] unformattedData)
        {
            try
            {
                return Enumerable.Range(1, unformattedData.Length)
                    .Select(index => DateTime.Parse(unformattedData[index - 1]))
                    .ToArray();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public DateTime[] SortDataArray(ref DateTime[] tollPassages)
        {
            Array.Sort(tollPassages);

            return tollPassages;
        }

        public int CalculateCost(DateTime[] tollPassages)
        {
            int fee = 0;
            DateTime previousPassage = default(DateTime);

            foreach (var currentPassage in tollPassages)
            {
                if (!IsWithinSameDay(tollPassages[0], currentPassage))
                {
                    break;
                }

                if (previousPassage == default(DateTime)
                    || IsWithinSameHour(previousPassage, currentPassage))
                {
                    int previousPassageFee = CalculateFeeFromTime(previousPassage);
                    int currentPassageFee = CalculateFeeFromTime(currentPassage);

                    if (previousPassageFee >= currentPassageFee)
                    {
                        continue;
                    }
                    else
                    {
                        fee = Math.Abs(fee - previousPassageFee);
                        fee += currentPassageFee;
                    }
                }
                else
                {
                    fee += CalculateFeeFromTime(currentPassage);
                }

                previousPassage = currentPassage;
            }

            return Math.Min(fee, 60);
        }

        public bool IsWithinSameDay(DateTime initialPassage, DateTime currentPassage)
        {
            return initialPassage.Day == currentPassage.Day ? true : false;
        }

        public bool IsWithinSameHour(DateTime previousPassage, DateTime currentPassage)
        {
            bool isMoreThanOneHour = currentPassage.Hour > previousPassage.AddHours(1).Hour;

            bool isCurrentPassageHourAndMinutesHigher = currentPassage.Hour > previousPassage.Hour
                && currentPassage.Minute > previousPassage.Minute;

            bool isCurrentHourHigherButMinutesLower = currentPassage.Hour > previousPassage.Hour
                && currentPassage.Minute < previousPassage.Minute;

            if (isMoreThanOneHour)
            {
                return false;
            }
            else if (isCurrentPassageHourAndMinutesHigher)
            {
                return false;
            }
            else if (isCurrentHourHigherButMinutesLower)
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
            if (CheckFreeDates(timeOfToll))
            {
                return 0;
            }

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

        public bool CheckFreeDates(DateTime timeOfToll)
        {
            return timeOfToll.DayOfWeek == DayOfWeek.Saturday
                    || timeOfToll.DayOfWeek == DayOfWeek.Sunday
                    || timeOfToll.Month == 7;
        }
    }
}