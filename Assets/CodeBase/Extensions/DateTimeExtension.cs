using System;
using CodeBase.Data;

namespace CodeBase.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToDateTime(this Date date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static Date ToDate(this DateTime dateTime)
        {
            var date = new Date()
            {
                Day = dateTime.Day,
                Month = dateTime.Month,
                Year = dateTime.Year
            };

            return date;
        }
    }
}