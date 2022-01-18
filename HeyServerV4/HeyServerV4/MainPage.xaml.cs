using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeyServerV4
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> WeekDays = new ObservableCollection<string>() { "S", "M", "T", "W", "Th", "F", "Sa" };
        public ObservableCollection<CalendarDayModel> DaysModel = new ObservableCollection<CalendarDayModel>();
        public string MonthsName { get; set; }
        private double _monthstotal;

        public static int theCurrentID = Convert.ToInt32(Application.Current.Properties["ID"]);
        public double MonthsTotal
        {
            get => _monthstotal;
            set
            {
                if (value != _monthstotal)
                {
                    _monthstotal = value;
                    OnPropertyChanged(nameof(MonthsTotal));
                }
            }
        }
        public MainPage()
        {
            InitializeComponent();
            AddCurrentMonthToList();
            GetTotal();

            firstlst.FlowItemsSource = WeekDays;
            lst.FlowItemsSource = DaysModel;
            MonthsName = DateTime.Now.ToString("MMMM");
            BindingContext = this;
        }

        public void GetTotal()
        {
            MonthsTotal = 0;
            int currentId = Convert.ToInt32(Application.Current.Properties["ID"]);
            foreach (var name in DaysModel)
            {
                if (name.Id == currentId)
                {
                    MonthsTotal = MonthsTotal + Convert.ToDouble(name.TipOut);
                }
                
                else
                {
                    MonthsTotal = 0;
                }
            }

        }

        public void AddCurrentMonthToList()
        {
            int currentId = Convert.ToInt32(Application.Current.Properties["ID"]);
            foreach (var name in App.Days)
            {
                if (name.Id == currentId)
                {
                    DaysModel.Add(name);
                }
            }
        }

        public async void AnswerChecker(string theString, int theCase, ItemTappedEventArgs r)
        {
            if(theString.Contains(",") || theString.Contains(" ") || theString.Contains("-"))
            {
                await DisplayAlert("Try Again", "Your input can only be numbers", "Ok");
            }

            else if(theString == "")
            {
                switch (theCase)
                {
                    case 1:
                        var details = r.Item as CalendarDayModel;
                        details.TipOut = null;
                        GetTotal();
                        SaveCalendar(details.Id, details.DayNumber, details.TipOut);
                        break;
                    case 2:
                        break;
                }
            }

            else
            {
                switch(theCase)
                {
                    case 1:
                        var details = r.Item as CalendarDayModel;
                        details.TipOut = theString;
                        GetTotal();
                        SaveCalendar(details.Id, details.DayNumber, details.TipOut);
                        break;
                    case 2:
                        Application.Current.Properties["TipGoal"] = theString;
                        await Application.Current.SavePropertiesAsync();

                        await Navigation.PushAsync(new MainPage());
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                        break;
                }
            }
        }

        public void SaveCalendar(int x, string y, string z)
        {
            foreach(var name in App.Days)
            {
                if(name.Id == x && name.DayNumber == y)
                {
                    name.TipOut = z;
                }
            }

            string jsonDays = JsonConvert.SerializeObject(App.Days);
            Application.Current.Properties["TheDays"] = jsonDays;
            Application.Current.SavePropertiesAsync();

            string jsonMonths = JsonConvert.SerializeObject(App.Months);
            Application.Current.Properties["TheMonths"] = jsonMonths;
            Application.Current.SavePropertiesAsync();
        }

        [Obsolete]
        private async void lst_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as CalendarDayModel;
            string action = await DisplayActionSheet($"You made ${details.TipOut}",null, null, "Edit","Notes", "Check Out", "Delete","Cancel");

            switch(action)
            {
                case "Cancel":
                    break;

                case "Delete":
                    string actionDelete = await DisplayActionSheet("Are you sure?", null, null, "Yes", "No");
                    if(actionDelete == "Yes")
                    {
                        MonthsTotal = MonthsTotal - Convert.ToDouble(details.TipOut);
                        details.TipOut = null;
                        details.Notes = null;
                        details.CheckOutPhoto = null;
                        SaveCalendar(details.Id, details.DayNumber, details.TipOut);
                        break;
                    }

                    else
                    {
                        break;
                    }

                case "Edit":
                    ListClicked(e);
                    SaveCalendar(details.Id, details.DayNumber, details.TipOut);
                    break;

                case "Notes":
                    await Navigation.PushAsync(new NotesPage(details), true);
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    break;

                case "Check Out":
                    await Navigation.PushAsync(new CheckOutPage(details), true);
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    break;
            }
        }

        [Obsolete]
        public async void ListClicked(ItemTappedEventArgs e)
        {
            var details = e.Item as CalendarDayModel;
            if (details.DayNumber != null)
            {
                string answer;
                answer = await DisplayPromptAsync(details.DayNumber, details.TipOut, "Ok", null, null, 10, Keyboard.Numeric);
                AnswerChecker(answer, 1, e);
            }

            else
            {
                return;
            }
        }

        [Obsolete]
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            string x = Convert.ToString(Application.Current.Properties["TipGoal"]);
            string answer;
            answer = await DisplayPromptAsync("Your Tip Goal:", $"${x}", "Ok", null, null, 10, Keyboard.Numeric);
            AnswerChecker(answer, 2, null);
        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Log(),true);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }
    }
}
