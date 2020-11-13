using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace lr10v2
{
    public class Date
    {
        private ushort day, month;
        public ushort Day
        {
            get => day;
            set
            {
                if (value > 31)
                {
                    day = 31;
                }
                else
                {
                    day = value;
                }
            }
        }
        public ushort Month
        {
            get => month;
            set
            {
                if (value > 12)
                {
                    month = 12;
                }
                else
                {
                    month = value;
                }
            }
        }
        public int Year { get; set; }


        public Date(string date)
        {
            string[] tokens = date.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 3)
            {
                Debug.WriteLine("парсинг даты сломался");
            }
            else
            {
                Day = ushort.Parse(tokens[0]);
                Month = ushort.Parse(tokens[1]);
                Year = int.Parse(tokens[2]);

            }

        }

        public Date(ushort day, ushort month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }


        public static implicit operator Date(string s) => new Date(s);

        public override string ToString() => $"{Day}.{Month}.{Year}";

        public override bool Equals(object o)
        {

            if ((o == null) || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                var d = (Date)o;
                return (d.Day == Day) && (d.Month == Month) && (d.Year == Year);
            }
        }

        public override int GetHashCode() => HashCode.Combine(Day, Month, Year);
    }

}
