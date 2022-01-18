using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyServerV4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Log : ContentPage
    {
        public Log()
        {
            InitializeComponent();

            lst.FlowItemsSource = App.Months;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(),true);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        private async void lst_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as CalendarMonthModel;
            string action = await DisplayActionSheet(details.StringofMonth + " " + details.YearOfThatMonth, null, "Ok", "Analysis", "Calendar");

            switch(action)
            {
                case "Calendar":
                    await Navigation.PushAsync(new LogCalendar(details.MonthId), true);
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    break;

                case "Analysis":
                    await Navigation.PushAsync(new Stats(details.MonthId), true);
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    break;
            }
        }
    }
}