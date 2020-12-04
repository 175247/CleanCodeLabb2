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
            SortDataArray(ref dates);

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

            for (int i = 0; i < days.Length - 1; i++)
            {
                bool IsWithinSameDay = days[i].Day == days[i + 1].Day ? true : false;

                if (!IsWithinSameDay)
                {
                    continue;
                }
                else
                {

                    if (IsWithinSameHour(days[i], days[i + 1]))
                    {
                        fee += Math.Max(CalculateFeeFromTime(days[i]), CalculateFeeFromTime(days[i + 1]));
                    }
                    else
                    {
                        fee += CalculateFeeFromTime(days[i + 1]);
                    }
            
                }
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
