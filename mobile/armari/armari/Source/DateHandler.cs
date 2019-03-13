using UIKit;
using System;
using System.Collections.Generic;

namespace armari
{
    public class DateHandler
    {
        #region Computed Properties
        private DateTime ThisDay { get; set; }
        public List<string> DayOfWeekStr = new List<string>();
        #endregion

        #region Constructors
        public DateHandler()
        {
            ThisDay = DateTime.Today;
        }
        #endregion

        public int GetDayFromStr(string today)
        {
            string day = today.Trim();
            day = day.Substring(0, 2);

            int index = 1;

            switch (day)
            {
                case "Su":
                    index = 0;
                    break;
                case "Mo":
                    index = 1;
                    break;
                case "Tu":
                    index = 2;
                    break;
                case "We":
                    index = 3;
                    break;
                case "Th":
                    index = 4;
                    break;
                case "Fr":
                    index = 5;
                    break;
                case "Sa":
                    index = 6;
                    break;
            }

            return index;
        }

        public string GetDay(int num)
        {
            string day;
            int caseSwitch = num % 7;

            switch (caseSwitch)
            {
                case 0:
                    day = "Sunday";
                    break;
                case 1:
                    day = "Monday";
                    break;
                case 2:
                    day = "Tuesday";
                    break;
                case 3:
                    day = "Wednesday";
                    break;
                case 4:
                    day = "Thursday";
                    break;
                case 5:
                    day = "Friday";
                    break;
                case 6:
                    day = "Saturday";
                    break;
                default:
                    day = "Monday";
                    break;
            }

            return day;
        }

        public List<string> GetListOfDays(int num)
        {
            List<string> days = new List<string>();

            int index = GetDayFromStr(ThisDay.ToString("D"));

            days.Insert(0, "Today");

            for (int i = 1; i < num; ++i)
            {
                days.Insert(0, GetDay(index));
                index--;
            }

            return days;
        }
    }
}