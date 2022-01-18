using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HeyServerV4;
using System.Collections.Generic;
using System.Globalization;

namespace HeyServerV4
{
    public partial class App : Application
    {
        public static ObservableCollection<CalendarMonthModel> Months = new ObservableCollection<CalendarMonthModel>();
        public static ObservableCollection<CalendarDayModel> Days = new ObservableCollection<CalendarDayModel>();
        public App()
        {
            InitializeComponent();

            if(Application.Current.Properties.ContainsKey("TheMonths") == true)
            {
                Months = JsonConvert.DeserializeObject<ObservableCollection<CalendarMonthModel>>(Convert.ToString(Application.Current.Properties["TheMonths"]));
                Days = JsonConvert.DeserializeObject<ObservableCollection<CalendarDayModel>>(Convert.ToString(Application.Current.Properties["TheDays"]));
                MakeNewMonth(DateTime.Now.Month, DateTime.Now.Year);
                string jsonDays = JsonConvert.SerializeObject(Days);
                Application.Current.Properties["TheDays"] = jsonDays;

                string jsonMonths = JsonConvert.SerializeObject(Months);
                Application.Current.Properties["TheMonths"] = jsonMonths;

                Application.Current.SavePropertiesAsync();
                MainPage = new NavigationPage(new MainPage());
            }

            else
            {
                makeMonth(1);
                makeDays(1);
                SetIntLists();
                InItSavedProps();

                string jsonMonths = JsonConvert.SerializeObject(Months);
                Application.Current.Properties["TheMonths"] = jsonMonths;
                Application.Current.SavePropertiesAsync();

                string jsonDays = JsonConvert.SerializeObject(Days);
                Application.Current.Properties["TheDays"] = jsonDays;
                Application.Current.SavePropertiesAsync();

                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            MakeNewMonth(DateTime.Now.Month, DateTime.Now.Year);
            string jsonDays = JsonConvert.SerializeObject(Days);
            Application.Current.Properties["TheDays"] = jsonDays;

            string jsonMonths = JsonConvert.SerializeObject(Months);
            Application.Current.Properties["TheMonths"] = jsonMonths;

            Application.Current.SavePropertiesAsync();
            MainPage = new NavigationPage(new MainPage());
        }

        public void InItSavedProps()
        {
            Application.Current.SavePropertiesAsync();

            Application.Current.Properties["NextYear"] = DateTime.Now.Year + 1;
            Application.Current.SavePropertiesAsync();

            Application.Current.Properties["NextMonth"] = DateTime.Now.Month + 1;
            Application.Current.SavePropertiesAsync();

            Application.Current.Properties["ID"] = 1;
            Application.Current.SavePropertiesAsync();

            Application.Current.Properties["TipGoal"] = 100;

            return;
        }

        public bool oldMonthChecker(int x, int y)
        {
            bool z = false;
            foreach(var item in Months)
            {
                if(item.NameofMonth == x && item.YearOfThatMonth == y)
                {
                    z = true;
                }

                else
                {
                    break;
                }
            }

            return z;
        }

        //x is the now month and y is the now year
        private void MakeNewMonth(int x, int y)
        {
            if (oldMonthChecker(x, y) == false)
            {
                int nextYear = Convert.ToInt32(Application.Current.Properties["NextYear"]);
                int nextMonth = Convert.ToInt32(Application.Current.Properties["NextMonth"]);
                int iD = Convert.ToInt32(Application.Current.Properties["ID"]);

                if (y >= nextYear)
                {
                    nextYear++;
                    Application.Current.Properties["NextYear"] = nextYear;
                    Application.Current.SavePropertiesAsync();

                    iD++;
                    makeDays(iD);
                    makeMonth(iD);

                    Application.Current.Properties["ID"] = iD;
                    Application.Current.SavePropertiesAsync();

                    Application.Current.Properties["NextMonth"] = x + 1;
                    Application.Current.SavePropertiesAsync();
                }

                else
                {

                    if (x >= nextMonth)
                    {
                        iD++;
                        makeDays(iD);
                        makeMonth(iD);

                        Application.Current.Properties["ID"] = iD;
                        Application.Current.SavePropertiesAsync();

                        int test = x;

                        if (test == 12)
                        {
                            Application.Current.Properties["NextMonth"] = x;
                            Application.Current.SavePropertiesAsync();
                        }

                        else
                        {
                            test++;
                            Application.Current.Properties["NextMonth"] = test;
                            Application.Current.SavePropertiesAsync();
                        }
                    }

                }
            }
            return;
        }

        private void SetIntLists()
        {
            ObservableCollection<CalendarDayModel> thedays = new ObservableCollection<CalendarDayModel>();
            string jsonDays = JsonConvert.SerializeObject(thedays);
            Application.Current.Properties["TheDays"] = jsonDays;

            ObservableCollection<CalendarMonthModel> themonths = new ObservableCollection<CalendarMonthModel>();
            string jsonMonths = JsonConvert.SerializeObject(themonths);
            Application.Current.Properties["TheMonths"] = jsonMonths;

            Application.Current.SavePropertiesAsync();

            return;
        }

        public void makeMonth(int x)
        {
            Months.Add(new CalendarMonthModel {NameofMonth = DateTime.Now.Month, YearOfThatMonth = DateTime.Today.Year, MonthId = x});
        }

        public void makeDays(int x)
        {
            AddSpaces(x);
            AddCal(x);
        }

        private int GetDayOfWeek(int x)
        {
            //x is the month in int
            DateTime dateValue = new DateTime(DateTime.Now.Year, x, 1);
            int numb = (int)dateValue.DayOfWeek;

            return numb;
        }

        public void AddSpaces(int x)
        {
            int thisMonth = DateTime.Now.Month;
            for (int i = 0; i < GetDayOfWeek(thisMonth); i++)
            {
                Days.Add(new CalendarDayModel { DayNumber = null, Notes = null, Id = x});
            }

            return;
        }

        public void AddCal(int x)
        {
            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                Days.Add(new CalendarDayModel { DayNumber = Convert.ToString(i), Notes = null, Id = x});
            }

            return;
        }
    }
}
