using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Ecaportion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameDate : ContentPage
    {
        List<string> GameSchedule = new List<string>();
        List<string> GameDates = new List<string>();
        List<string> GameNameList = new List<string>();
        string[] dbgameid = new string[200];
        string[] ArrGameDate = new string[1000];
        List<Mydata> VLayoutDataList = new List<Mydata>();
        int loopn = 0;
        public GameDate()
        {
            InitializeComponent();
            SportsSchedule();
            SDF.Text = DataFetch.GameName.ToLower();
        }

        private async void SportsSchedule()
        {
            loopn = 0;
            GameSchedule.Clear();
            string SpSch = "";
            try
            {
                string str = "select game_date from game_date";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        SpSch = myDataSet.Tables[0].Rows[i]["game_date"].ToString();
                        DateTime d = DateTime.ParseExact(SpSch, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string ok = d.ToString("dddd, dd MMMM yyyy");
                        GameSchedule.Add(ok);
                        ArrGameDate[i] = ok;
                        GameDates.Add(SpSch);
                        loopn++;
                    }
                }
            }
            catch { }
            Vlayoutdata();
        }

        private void Vlayoutdata()
        {
            VDateList.ItemsSource = null;
            int sn = 1;
            for (int i = 0; i < loopn; i++)
            {
               VLayoutDataList.Add(new Mydata() { GameDate = ArrGameDate[i], Sno = sn.ToString() });
                sn++;
            }
            VDateList.ItemsSource = VLayoutDataList;
        }

        public class Mydata
        {
            public string GameDate { get; set; }
            public string Sno { get; set; }
        }

        private void VdateClicked(object sender, ItemTappedEventArgs e)
        {
            var listindex = e.ItemIndex;
            DataFetch.GameDate = GameDates[listindex];
            Application.Current.MainPage = new NavigationPage(new GameParticipants());
        }

        private async void SubmitCE(object sender, EventArgs e)
        {
            DateTime objdate = SDate.Date;
            string day = objdate.Day.ToString("00");
            string month = objdate.Month.ToString("00");
            string year = objdate.Year.ToString();
            string Qdate = month.ToString() + "/" + day.ToString() + "/" + year.ToString();
            if (GameDates.Contains(Qdate))
            {
                await DisplayAlert("Alert", "The Date Is Already IN The List", "ok");
            }
            else
            {
                try
                {
                    string str = "insert into game_date(game_Id,game_date,A_year)values('" + DataFetch.GameId + "','" + Qdate + "','" + DataFetch.AcademicYear + "')";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });
                    if (DataFetch.flag == "T")
                    {
                        DependencyService.Get<IToastInterface>().Longtime("Game Scheduled Successfully");
                        ScheduleGame.IsVisible = false;
                        SportsSchedule();
                    }
                    else
                    {
                        DependencyService.Get<IToastInterface>().Longtime("Operation Failed! Try Again");
                    }
                }
                catch { }
            }

        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            ScheduleGame.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());
            return true;
        }
        
        private void CloseIconClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new GameDate());
        }
    }
}