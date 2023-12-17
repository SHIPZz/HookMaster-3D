using System;
using CodeBase.Constant;
using UnityEngine;

namespace CodeBase.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToDateTime(this long unixTime) =>
            DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;

        public static long ToUnixTime(this DateTime dateTime) =>
            dateTime.ToDateTimeOffset(TimeSpan.Zero).ToUnixTimeSeconds();

        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime) =>
            dateTime.ToDateTimeOffset(TimeSpan.Zero);

        private static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, TimeSpan offset) =>
            new(DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), offset);
    }
}