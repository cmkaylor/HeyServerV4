using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.ChartEntry;
using OxyPlot;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace HeyServerV4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stats : ContentPage
    {
        public static int theID;
        public ObservableCollection<CalendarDayModel> model = new ObservableCollection<CalendarDayModel>();
        public PlotModel TwoLineModel { get; set; }
        public int theYear { get; set; }
        public int theMonth { get; set; }
        public string theMonthName { get; set; }

        public List<Entry> entries = new List<Entry>();
        public Stats(int x)
        {

            InitializeComponent();
            theID = x;
            getList();
            entries.Add(new Entry(returnableTotal(1)){Label = "Sunday", ValueLabel = Convert.ToString(returnableTotal(1)), Color = SKColors.Blue, ValueLabelColor = SKColors.Blue });
            entries.Add(new Entry(returnableTotal(2)) { Label = "Monday", ValueLabel = Convert.ToString(returnableTotal(2)), Color = SKColors.Red, ValueLabelColor = SKColors.Red });
            entries.Add(new Entry(returnableTotal(3)) { Label = "Tuesday", ValueLabel = Convert.ToString(returnableTotal(3)), Color = SKColors.Green, ValueLabelColor = SKColors.Green});
            entries.Add(new Entry(returnableTotal(4)) { Label = "Wednesday", ValueLabel = Convert.ToString(returnableTotal(4)), Color = SKColors.DarkOrange, ValueLabelColor = SKColors.DarkOrange });
            entries.Add(new Entry(returnableTotal(5)) { Label = "Thursday", ValueLabel = Convert.ToString(returnableTotal(5)), Color = SKColors.Violet, ValueLabelColor = SKColors.Violet });
            entries.Add(new Entry(returnableTotal(6)) { Label = "Friday", ValueLabel = Convert.ToString(returnableTotal(6)), Color = SKColors.Turquoise, ValueLabelColor = SKColors.Turquoise });
            entries.Add(new Entry(returnableTotal(0)) { Label = "Saturday", ValueLabel = Convert.ToString(returnableTotal(0)), Color = SKColors.Indigo, ValueLabelColor = SKColors.Indigo });


            chartViewPie.Chart = new PieChart { Entries = entries };
            TwoLineModel = CreateAreaChart();
            BindingContext = this;
        }

        public void getList()
        {
            var firstElement = App.Months.First();
            int x = firstElement.NameofMonth;
            string y = firstElement.StringofMonth;
            theMonth = x;
            theMonthName = y;
            theYear = firstElement.YearOfThatMonth;
            foreach (var item in App.Days)
            {
                if (item.Id == theID)
                {
                    model.Add(item);
                }
            }
        }

        public float returnableTotal(int x)
        {
            List<int> thelist = new List<int>();
            thelist = getDays(x);
            double y = 0;
             foreach (var item in model)
               {
                double tipout = Convert.ToDouble(item.TipOut);
                if(thelist.Contains(Convert.ToInt32(item.DayNumber)))
                  {
                    y = y + tipout;
                  }
               }

            return (float)y;
        }

        public  List<int> getDays(int x)
        {
            List<int> lst = new List<int>();
            int intMonth = theMonth;
            int intYear = theYear;
            int intDaysThisMonth = DateTime.DaysInMonth(intYear, intMonth);
            DateTime oBeginnngOfThisMonth = new DateTime(intYear, intMonth, 1);
            for (int i = 1; i < intDaysThisMonth + 1; i++)
            {
                if (Convert.ToInt32(oBeginnngOfThisMonth.AddDays(i).DayOfWeek) == x)
                {
                    DateTime day = new DateTime(intYear, intMonth, i);
                    int z = day.Day;
                    lst.Add(z);
                }
            }
            return lst;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Log(), true);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        public PlotModel CreateAreaChart()
        {
            var plotModel1 = new PlotModel { Title = null};
            var areaSeries1 = new LineSeries();
            var areaSeries2 = new LineSeries();

            double y = Convert.ToDouble(Application.Current.Properties["TipGoal"]);

            foreach (var item in model)
            {
                if(item.TipOut != null)
                {
                    double x = Convert.ToDouble(item.TipOut);
                    areaSeries1.Points.Add(new DataPoint(Convert.ToInt32(item.DayNumber), x));
                    areaSeries2.Points.Add(new DataPoint(Convert.ToInt32(item.DayNumber), y));
                }
               
            }

            plotModel1.Series.Add(areaSeries1);
            plotModel1.Series.Add(areaSeries2);
            return plotModel1;
        }
    }
}