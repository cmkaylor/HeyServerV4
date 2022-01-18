using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyServerV4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
    {
        public CheckOutPage(object x)
        {
            InitializeComponent();

            GetDetails = x;
            var details = (CalendarDayModel)x;

            if (details.CheckOutPhoto != null)
            {
                Name = "Retake";
                byte[] theBytes = Convert.FromBase64String(details.CheckOutPhoto);
                var stream1 = new MemoryStream(theBytes);
                resultImage.Source = ImageSource.FromStream(() => stream1);
            }

            else
            {
                Name = "Take";
            }

            BindingContext = this;
        }

        public object GetDetails
        {
            get; set;
        }

        public object Name
        {
            get; set;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            var details = (CalendarDayModel)GetDetails;

            if (details.CheckOutPhoto != null)
            {
                string result = await DisplayActionSheet("Warning: Taking a new picture will delete the last one.", "Retake", "Go Back");

                switch (result)
                {
                    case "Retake":
                        takepic();
                        break;

                    case "Go Back":
                        break;
                }
            }

            else
            {
                takepic();
            }
        }

        public async void takepic()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            var stream = await result.OpenReadAsync();

            byte[] resultz;
            using (var streamReader = new MemoryStream())
            {
                stream = await result.OpenReadAsync();
                stream.CopyTo(streamReader);
                resultz = streamReader.ToArray();
            }

            string strBase64 = Convert.ToBase64String(resultz);

            var details = (CalendarDayModel)GetDetails;
            details.CheckOutPhoto = strBase64;

            string jsonDays = JsonConvert.SerializeObject(App.Days);
            Application.Current.Properties["TheDays"] = jsonDays;
            await Application.Current.SavePropertiesAsync();

            await Navigation.PushAsync(new CheckOutPage(GetDetails));
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var details = (CalendarDayModel)GetDetails;
            int id = details.Id;
            if (Navigation.NavigationStack.Last().GetType() == typeof(MainPage))
            {
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            else if (Navigation.NavigationStack.Last().GetType() == typeof(MainPage))
            {
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            else
            {
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
        }
    }
}