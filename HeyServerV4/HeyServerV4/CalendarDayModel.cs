using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HeyServerV4
{
    [AddINotifyPropertyChangedInterface]
    public class CalendarDayModel
    {

        ColorTypeConverter converter = new ColorTypeConverter();
        public string DayNumber
        {
            get;set;
        }

        public string TipOut
        {
            get;set;
        }

        public string Notes
        {
            get;set;
        }

        public int Id
        {
            get;set;
        }

        public string CheckOutPhoto
        {
            get;set;
        }

        [JsonIgnore]

        public Color BackGroundColor
        {
            get { return (Color)converter.ConvertFromInvariantString(ReturnableBackGroundColor()); }
            set
            {
                BackGroundColor = value;
            }
        }

        [JsonIgnore]
        public Color Border
        {
            get { return (Color)converter.ConvertFromInvariantString(ReturnableBColor()); }
            set {Border = value;}

        }
        public string ReturnableBackGroundColor()
        {
            double y = Convert.ToDouble(Application.Current.Properties["TipGoal"]);
            string x;
            double tipOut = Convert.ToDouble(TipOut);
            if (TipOut == null)
            {
                x = "Color.Transparent";
            }
            else if (tipOut < y)
            {
                x = "Color.Red";
            }

            else
            {
                
                x = "Color.LimeGreen";
            }

            return x;
        }
        public string ReturnableBColor()
        {
            string x;
            if (Convert.ToInt32(DayNumber) == DateTime.Now.Day && Id == Convert.ToInt32(Application.Current.Properties["ID"]))
            {
                x = "Color.Black"; 
            }
            else
            {
                x = "Color.Transparent";
            }

            return x;
        }
    }
}
