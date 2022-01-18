using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeyServerV4
{
    [AddINotifyPropertyChangedInterface]
    public class CalendarMonthModel
    {
        public int NameofMonth
        {
            get;set;
        }

        public int YearOfThatMonth
        {
            get;set;
        }

        public int MonthId
        {
            get;set;
        }

        [JsonIgnore]
        public double MonthTotal
        {
            get { return MakeTotal(); }
            set { MonthTotal = value; }
        }

        public double MakeTotal()
        {
            double x = 0;
            foreach (var name in App.Days)
            {
                if (name.Id == MonthId)
                {
                    x = x + Convert.ToDouble(name.TipOut);
                }
            }

            return x;
        }

        public string StringofMonth
        {
            get { return ConvertIntToMonth(); }
            set { StringofMonth = value; }
        }

        public string ConvertIntToMonth()
        {
            string returnable = "";
            switch(NameofMonth)
            {
                case 1:
                    returnable = "January";
                    break;
                case 2:
                    returnable = "February";
                    break;
                case 3:
                    returnable = "March";
                    break;
                case 4:
                    returnable = "April";
                    break;
                case 5:
                    returnable = "May";
                    break;
                case 6:
                    returnable = "June";
                    break;
                case 7:
                    returnable = "July";
                    break;
                case 8:
                    returnable = "August";
                    break;
                case 9:
                    returnable = "September";
                    break;
                case 10:
                    returnable = "October";
                    break;
                case 11:
                    returnable = "November";
                    break;
                case 12:
                    returnable = "December";
                    break;
            }

            return returnable;
        }
    }
}
