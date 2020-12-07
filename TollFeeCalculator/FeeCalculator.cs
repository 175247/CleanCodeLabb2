using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TollFeeCalculator
{
    public class FeeCalculator
    {
        private readonly ISettings _settings;

        public FeeCalculator(ISettings settings)
        {
            _settings = settings;
        }

        public void Run()
        {
            string[] unformattedDates = GetFileDataAsArray();
            DateTime[] tollPassages = ParseDateTimes(unformattedDates);

            SortDataArray(ref tollPassages);
            int TotalCost = CalculateCost(tollPassages);
            Console.Write("The total fee for the inputfile is {0}", TotalCost);
        }

        public string[] GetFileDataAsArray()
        {
            string fileData = "";
            try
            {
                fileData = File.ReadAllText(_settings.DataFilePath);
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
            var formattedDataList = new List<DateTime>();

            try
            {
                for (int i = 1; i < unformattedData.Length - 1; i++)
                {
                    var formattedData = DateTime.Parse(unformattedData[i - 1]);
                    formattedDataList.Add(formattedData);
                }

                return formattedDataList.ToArray();
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

            bool isCurrentPassageBothValuesHigher = currentPassage.Hour > previousPassage.Hour
                && currentPassage.Minute > previousPassage.Minute;

            bool isCurrentHourHigherButMinutesLower = currentPassage.Hour > previousPassage.Hour
                && currentPassage.Minute < previousPassage.Minute;

            if (isMoreThanOneHour)
            {
                return false;
            }
            else if (isCurrentPassageBothValuesHigher)
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