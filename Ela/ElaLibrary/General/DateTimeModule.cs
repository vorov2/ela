using System;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using Ela.Runtime;
using System.Globalization;

namespace Ela.Library.General
{
	public sealed class DateTimeModule : ForeignModule
	{
		internal const string DEFAULT_FORMAT = "dd/MM/yyyy HH:mm:ss";
        internal static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-US");
        
        public DateTimeModule()
		{

		}
		
        public override void Initialize()
		{
			Add<Int64>("now", Now);
			Add<Int64>("today", Today);
            Add<Int32,Int32,Int32,Int32,Int32,Int32,Int32,Int64>("newDateTime", NewDateTime);
            Add("maxDateTime", new ElaValue(DateTime.MaxValue.Ticks));
            Add("minDateTime", new ElaValue(DateTime.MinValue.Ticks));
			Add<Int64,Int64,Int64>("add", Add);
			Add<Int64,Int64,Int64>("addTicks", AddTicks);
			Add<Double,Int64,Int64>("addMilliseconds", AddMilliseconds);
			Add<Double,Int64,Int64>("addSeconds", AddSeconds);
			Add<Double,Int64,Int64>("addMinutes", AddMinutes);
			Add<Double,Int64,Int64>("addHours", AddHours);
			Add<Double,Int64,Int64>("addDays", AddDays);
			Add<Int32,Int64,Int64>("addMonths", AddMonths);
			Add<Int32,Int64,Int64>("addYears", AddYears);
			Add<Int64,Int32>("years", Years);
			Add<Int64,Int32>("months", Months);
			Add<Int64,Int32>("days", Days);
			Add<Int64,Int32>("hours", Hours);
			Add<Int64,Int32>("minutes", Minutes);
			Add<Int64,Int32>("seconds", Seconds);
			Add<Int64,Int32>("milliseconds", Milliseconds);
			Add<Int64,Int64>("ticks", Ticks);
			Add<Int64,String>("dayOfWeek", GetDayOfWeek);
			Add<Int64,Int32>("dayOfYear", GetDayOfYear);
			Add<Int64,Int64>("date", GetDate);
			Add<String,String,Int64>("parse", Parse);
			Add<String,Int64,String>("formatToString", ToStringFormat);
			Add<Int64,Int64,ElaRecord>("diff", Diff);
		}

        public long NewDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(year, month, day, hour, minute, second, millisecond).Ticks;
        }

		public long Now()
		{
			return DateTime.Now.Ticks;
		}


		public long Today()
		{
			return DateTime.Today.Ticks;
		}


        public long Add(long toAdd, long val)
		{
			return new DateTime(val).Add(new TimeSpan(toAdd)).Ticks;
		}


		public long AddTicks(long ticks, long val)
		{
			return new DateTime(val).AddTicks(ticks).Ticks;	
		}


		public long AddMilliseconds(double ms, long val)
		{
			return new DateTime(val).AddMilliseconds(ms).Ticks;
		}


		public long AddSeconds(double sec, long val)
		{
			return new DateTime(val).AddSeconds(sec).Ticks;
		}


		public long AddMinutes(double mins, long val)
		{
			return new DateTime(val).AddMinutes(mins).Ticks;
		}


		public long AddHours(double hours, long val)
		{
			return new DateTime(val).AddHours(hours).Ticks;
		}


		public long AddDays(double days, long val)
		{
			return new DateTime(val).AddDays(days).Ticks;
		}


		public long AddMonths(int months, long val)
		{
			return new DateTime(val).AddMonths(months).Ticks;
		}


		public long AddYears(int years, long val)
		{
			return new DateTime(val).AddYears(years).Ticks;
		}


		public int Years(long val)
		{
			return new DateTime(val).Year;
		}


		public int Months(long val)
		{
			return new DateTime(val).Month;
		}


		public int Days(long val)
		{
			return new DateTime(val).Day;
		}


		public int Hours(long val)
		{
			return new DateTime(val).Hour;
		}


		public int Minutes(long val)
		{
			return new DateTime(val).Minute;
		}


		public int Seconds(long val)
		{
			return new DateTime(val).Second;
		}


		public int Milliseconds(long val)
		{
			return new DateTime(val).Millisecond;
		}


		public long Ticks(long val)
		{
			return val;
		}


		public string GetDayOfWeek(long val)
		{
			var dw = new DateTime(val).DayOfWeek;

			switch (dw)
			{
				case DayOfWeek.Friday: return "Fri";
				case DayOfWeek.Monday: return "Mon";
				case DayOfWeek.Saturday: return "Sat";
				case DayOfWeek.Sunday: return "Sun";
				case DayOfWeek.Thursday: return "Thu";
				case DayOfWeek.Tuesday: return "Tue";
				case DayOfWeek.Wednesday: return "Wed";
				default: return String.Empty;
			}
		}


		public int GetDayOfYear(long val)
		{
			return new DateTime(val).DayOfYear;
		}


		public long GetDate(long val)
		{
			return new DateTime(val).Date.Ticks;
		}
			

		public long Parse(string format, string value)
		{
			return DateTime.ParseExact(value, String.IsNullOrEmpty(format) ? DEFAULT_FORMAT : format, Culture).Ticks;
		}


		public string ToStringFormat(string format, long val)
		{
			return new DateTime(val).ToString(
                String.IsNullOrEmpty(format) ? DEFAULT_FORMAT : format, Culture);
		}
        
		public ElaRecord Diff(long left, long right)
		{
			var ts = new DateTime(left) - new DateTime(right);
			return new ElaRecord(
				new ElaRecordField("ticks", ts.Ticks),
				new ElaRecordField("milliseconds", (Int32)ts.TotalMilliseconds),
				new ElaRecordField("seconds", (Int32)ts.TotalSeconds),
				new ElaRecordField("minutes", (Int32)ts.TotalMinutes),
				new ElaRecordField("hours", (Int32)ts.TotalHours),
				new ElaRecordField("days", (Int32)ts.TotalDays));
		}
	}
}