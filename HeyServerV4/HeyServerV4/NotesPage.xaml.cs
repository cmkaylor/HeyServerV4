using Newtonsoft.Json;
using PropertyChanged;
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
    [AddINotifyPropertyChangedInterface]
    public partial class NotesPage : ContentPage
    {
        public object theObject { get; set; }
        public NotesPage(object x)
        {
            InitializeComponent();
            var details = x as CalendarDayModel;
            theObject = x;

            editor.Text = details.Notes;
            BindingContext = this;

        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(), true);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var broughtItem = theObject as CalendarDayModel;
            foreach(var item in App.Days)
            {
                if(item.Id == broughtItem.Id)
                {
                    if(item.DayNumber == broughtItem.DayNumber)
                    {
                        item.Notes = editor.Text;
                    }
                }

            }
            string jsonDays = JsonConvert.SerializeObject(App.Days);
            Application.Current.Properties["TheDays"] = jsonDays;
            Application.Current.SavePropertiesAsync();
        }
    }
}