using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tools
{
    internal class DateExecutor
    {
        public static string GetMonthAbbreviation(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    throw new ArgumentOutOfRangeException("monthNumber", "Month number must be between 1 and 12.");
            }
        }

        public static Time12Hour ConvertTo12HourFormat(int hour24)
        {
            if (hour24 < 0 || hour24 > 23)
            {
                throw new ArgumentOutOfRangeException("hour24", "Hour must be between 0 and 23.");
            }

            int hour12 = hour24 % 12;
            string period = hour24 < 12 ? "AM" : "PM";

            if (hour12 == 0)
            {
                hour12 = 12;
            }

            return new Time12Hour(hour12, period);
        }
    }

    public class Time12Hour
    {
        public int Hour { get; set; }
        public string Period { get; set; }

        public Time12Hour(int hour, string period)
        {
            Hour = hour;
            Period = period;
        }
    }
}
