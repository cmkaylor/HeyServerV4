using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyServerV4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogCalendar : ContentPage
    {
        public ObservableCollection<string> WeekDays = new ObservableCollection<string>() { "S", "M", "T", "W", "Th", "F", "Sa" };
        public ObservableCollection<CalendarDayModel> DaysModel = new ObservableCollection<CalendarDayModel>();
        public string MonthsName { get; set; }
        private double _monthstotal;
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
        public LogCalendar(int x)
        {
            InitializeComponent();

            theID = x;
            AddCurrentMonthToList();
            GetTotal();

            firstlst.FlowItemsSource = WeekDays;
            lst.FlowItemsSource = DaysModel;
            MonthsName = getMonthName();
            BindingContext = this;
        }

        public string getMonthName()
        {
            string x = "";
            foreach(var item in App.Months)
            {
                if(theID == item.MonthId)
                {
                    x = item.StringofMonth;
                }

                else
                {
                    continue;
                }
            }

            return x;
        }

        public int theID;
        public void GetTotal()
        {
            MonthsTotal = 0;
            foreach (var name in DaysModel)
            {
                if (name.Id == theID)
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
            foreach (var name in App.Days)
            {
                if (name.Id == theID)
                {
                    DaysModel.Add(name);
                }
            }
        }

        public async void AnswerChecker(string theString, int theCase, ItemTappedEventArgs r)
        {
            if (theString.Contains(",") || theString.Contains(" ") || theString.Contains("-"))
            {
                await DisplayAlert("Try Again", "Your input can only be numbers", "Ok");
            }

            else if (theString == "")
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
                switch (theCase)
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
            foreach (var name in App.Days)
            {
                if (name.Id == x && name.DayNumber == y)
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
            string action = await DisplayActionSheet($"You made ${details.TipOut} on the {details.DayNumber}", null, null, "Edit", "Notes", "Delete", "Cancel");

            switch (action)
            {
                case "Cancel":
                    break;

                case "Delete":
                    MonthsTotal = MonthsTotal - Convert.ToDouble(details.TipOut);
                    details.TipOut = null;
                    SaveCalendar(details.Id, details.DayNumber, details.TipOut);
                    break;

                case "Edit":
                    ListClicked(e);
                    SaveCalendar(details.Id, details.DayNumber, details.TipOut);
                    break;

                case "Notes":
                    await Navigation.PushAsync(new NotesPage(details), true);
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

        private async void ImageButton_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Log(), true);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }
    }
}