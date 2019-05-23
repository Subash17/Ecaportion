using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Ecaportion
{
    public partial class MainPage : TabbedPage
    {
        List<string> Gamelist = new List<string>();       
        List<string> GameParticipantsListIndividual = new List<string>();
        List<string> Gamelistup = new List<string>();
        string[] GID = new string[250];
        int loopn = 0;
        string QGameDate = "";
        int loopforindividual = 0;
        int loopforGroup = 0;
        int GRI = 0;
        int decidetorun = 0;
        int gameposition = 0;
        string QGID = "";
        string[] ArrStdDetails = new string[1000];
        string[] ArrIndividualStdDetails = new string[1000];
        string[] ArrIndividualStdCode = new string[1000];
        string[] ArrGroupName = new string[1000];
        string[] ArrGroupId = new string[1000];
        string[] ArrGroupStdCode = new string[1000];
        List<Mydata> VTemplateData = new List<Mydata>();
        Grid grid = new Grid() { Margin = new Thickness(0, 1, 0, 5) };
        List<Picker> _myentries = new List<Picker>();

        public MainPage()
        {
            InitializeComponent();
            ListOfGames();
        }

        public class Mydata
        {
            public string StdData { get; set; }
            public string Sno { get; set; }
        }

        private async void ListOfGames()
        {
            string dc = "";
            loopn = 0;
            try
            {
                string str = "select game_id, game_name from game";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        dc = myDataSet.Tables[0].Rows[i]["game_name"].ToString();
                        ArrStdDetails[i] = dc;
                        string updc = dc.ToUpper();
                        GID[i] = myDataSet.Tables[0].Rows[i]["game_id"].ToString();
                        Gamelistup.Add(updc);
                        Gamelist.Add(dc);
                        loopn++;
                    }
                }
            }
            catch { }
            PDWGameName.ItemsSource = Gamelist;
            DataForLayout();
        }


        private void DataForLayout()
        {
            VListOfGames.ItemsSource = null;
            VTemplateData.Clear();
            int sn = 1;
            for (int i = 0; i < loopn; i++)
            {
                VTemplateData.Add(new Mydata() { StdData = ArrStdDetails[i], Sno = sn.ToString() });
                sn++;
            }
            VListOfGames.ItemsSource = VTemplateData;
        }


        private async void GGSubmit(object sender, EventArgs e)
        {
            string GName = GameName.Text;
            if (GName != null & GName != string.Empty)
            {
                string upce = GName.ToUpper();
                if (Gamelistup.Contains(upce))
                {
                    await DisplayAlert("Alert", "The Game Is Already IN The List", "ok");
                }
                else
                {
                    try
                    {
                        string str = "insert into game(A_year, game_name) values('" + DataFetch.AcademicYear + "','" + GName + "')";
                        DataSet myDataSet = new DataSet();
                        await Task.Run(() =>
                        {
                            myDataSet = DataFetch.GetData(str);
                        });
                        if (DataFetch.flag == "T")
                        {
                            GameName.Text = null;
                            ListOfGames();
                            DependencyService.Get<IToastInterface>().Longtime("Game Added Successfully");
                            AddGameBlock.IsVisible = false;
                        }
                        else
                        {
                            DependencyService.Get<IToastInterface>().Longtime("Operation Failed! Try Again");
                        }
                    }
                    catch
                    {
                        await DisplayAlert("Alert", "Please,  Check Your Connection ", "ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("Alert", "Please, Enter Game Name", "ok");
            }

        }


        private void AddGame(object sender, EventArgs e)
        {
            AddGameBlock.IsVisible = true;
        }

        private void GameSelected(object sender, ItemTappedEventArgs e)
        {
            var listindex = e.ItemIndex;
            DataFetch.GameId = GID[listindex];
            DataFetch.GameName = ArrStdDetails[listindex];
            Application.Current.MainPage = new NavigationPage(new GameDate());
        }

        private void CloseBtnTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }


        //Code For Result Portion
        private async void PDWGameName_SelectedIndexChanged(object sender, EventArgs e)
        {
            loopforGroup = 0;
            int abc = PDWGameName.SelectedIndex;
            QGID = GID[abc];
            try
            {
                string str = "select game_date from game_participants where game_Id='" + QGID + "'";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    QGameDate = myDataSet.Tables[0].Rows[0]["game_date"].ToString();
                }
            }

            catch { }

            //Code For List Of Groups/Ids
            try
            {
                string str = "select group_name, group_Id from game_group where game_Id='" + QGID + "'";
                DataSet myDataSet = new DataSet();
                myDataSet = DataFetch.GetData(str);
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        decidetorun = 0;
                        loopforGroup++;
                        string Gn = myDataSet.Tables[0].Rows[i]["group_name"].ToString();
                        string Gid = myDataSet.Tables[0].Rows[i]["group_Id"].ToString();
                        ArrGroupName[i] = Gn;
                        ArrGroupId[i] = Gid;
                    }
                }
            }
            catch
            {
                decidetorun++;
                IndividualParticipants();
            }

            Testcodebehind();
        }



        private async void IndividualParticipants()
        {
            loopforindividual = 0;
            GameParticipantsListIndividual.Clear();
            try
            {
                string str = "select stud_code from game_participants where game_Id='" + QGID + "'";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        loopforindividual++;
                        string abcdd4 = myDataSet.Tables[0].Rows[i]["stud_code"].ToString();
                        ArrIndividualStdCode[i] = abcdd4;
                    }
                }
            }
            catch { }
            GPIStdDetails();
        }

        // Students Detail Info Block
        private async void GPIStdDetails()
        {
            string StdInfo = "";
            for (int j = 0; j < loopforindividual; j++)
            {
                int Y = Convert.ToInt32(ArrIndividualStdCode[j]);
                try
                {
                    string str = "select ( STUD_CODE+'   '+ FIRST_NAME +' '+MIDDLE_NAME+' '+LAST_NAME)  AS Stddetails  from student where ID='" + Y + "'";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });
                    if (myDataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {
                            StdInfo = myDataSet.Tables[0].Rows[i]["StdDetails"].ToString();
                        }
                    }

                }
                catch { }
                ArrIndividualStdDetails[j] = StdInfo;
            }
            Testcodebehind();
        }

        //Grid ListViewBlock
        private void Testcodebehind()
        {
            GRI = 0;
            int looprun = 0;
            string[] stdorgroup = new string[150];
            if (decidetorun == 0)
            {
                looprun = loopforGroup;
                stdorgroup = ArrGroupName;
                GRI++;
            }
            else
            {
                looprun = loopforindividual;
                stdorgroup = ArrIndividualStdDetails;
            }

            Grid grid = new Grid() { Margin = new Thickness(0, 1, 0, 5) };
            dinsert.Children.Clear();
            _myentries.Clear();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < looprun; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
                var StdDetailsok = new Label { Text = stdorgroup[i], Margin = new Thickness(20, 5, 5, 0), TextColor = (Color.Black), FontSize = (20), VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Start };
                var mark = new Picker { Margin = new Thickness(0, 5, 10, 0), TextColor = (Color.Black), FontSize = (20), VerticalOptions = LayoutOptions.CenterAndExpand, Items = { "First", "Second", "Third", "Fourth" }, Title = "Rank", HorizontalOptions = LayoutOptions.EndAndExpand, };
                _myentries.Add(mark);
                var MyLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 1,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = 10
                };
                MyLayout.BackgroundColor = Color.White;
                MyLayout.Children.Add(StdDetailsok);
                MyLayout.Children.Add(mark);
                grid.Children.Add(MyLayout, 0, i);
                grid.RowSpacing = 1;
            }
            dinsert.Children.Add(grid);
        }

        // code to insert into database
        private void SaveBtnClicked(object sender, EventArgs e)
        {
            if (GRI == 0)
            {
                IndividualWinners();
            }
            else
            {
                GroupWinners();
            }
        }

        //Individual Participants
        private async void IndividualWinners()
        {
            PDWGameName.IsEnabled = false;
            int a = 0;
            string GP = "";
            int abc = PDWGameName.SelectedIndex;
            string abcd = QGameDate;
            string gameid = GID[abc];
            int QGroupCode = 0;
            foreach (var entry in _myentries)
            {
                if (entry.SelectedIndex < 0)
                {
                    gameposition = 0;
                }
                else
                {
                    var marks = entry.Items[entry.SelectedIndex];
                    if (marks == "First") { gameposition = 1; }
                    else if (marks == "Second") { gameposition = 2; }
                    else if (marks == "Third") { gameposition = 3; }
                    else if (marks == "Fourth") { gameposition = 4; }
                }
                GP = gameposition.ToString();
                try
                {
                    string str = "insert into game_winner(game_id , game_date ,stud_code ,group_code ,position) values('" + gameid + "','" + QGameDate + "','" + ArrIndividualStdCode[a] + "','" + QGroupCode + "','" + GP + "')";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });

                }
                catch { }
                a++;
            }
            PDWGameName.IsEnabled = true;
        }


        private async void GroupWinners()
        {
            PDWGameName.IsEnabled = false;
            string gp = "";
            int a = 0;
            int insertloop = 0;
            int abc = PDWGameName.SelectedIndex;
            string abcd = QGameDate;
            string gameid = GID[abc];
            string QGroupCode = "";
            foreach (var entry in _myentries)
            {
                insertloop = 0;
                QGroupCode = ArrGroupId[a];
                if (entry.SelectedIndex < 0)
                {
                    gameposition = 0;
                }
                else
                {
                    var marks = entry.Items[entry.SelectedIndex];
                    if (marks == "First") { gameposition = 1; }
                    else if (marks == "Second") { gameposition = 2; }
                    else if (marks == "Third") { gameposition = 3; }
                    else if (marks == "Fourth") { gameposition = 4; }
                }
                gp = gameposition.ToString();

                try
                {
                    string str = "select stud_code from game_group_member where group_Id='" + QGroupCode + "'";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });
                    if (myDataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {
                            insertloop++;
                            string GroupStdCodes = myDataSet.Tables[0].Rows[i]["stud_code"].ToString();
                            ArrGroupStdCode[i] = GroupStdCodes;
                        }
                    }

                }
                catch { }

                for (int i = 0; i < insertloop; i++)
                {
                    try
                    {
                        string str = "insert into game_winner(game_id , game_date ,stud_code ,group_code ,position) values('" + gameid + "','" + QGameDate + "','" + ArrGroupStdCode[i] + "','" + QGroupCode + "','" + gp + "')";
                        DataSet myDataSet = new DataSet();
                        await Task.Run(() =>
                        {
                            myDataSet = DataFetch.GetData(str);
                        });
                        //if (DataFetch.flag == "T")
                        //{

                        //    DependencyService.Get<IToastInterface>().Longtime("Game Added Successfully");

                        //}
                        //else
                        //{
                        //    DependencyService.Get<IToastInterface>().Longtime("Operation Failed! Try Again");
                        //}
                    }
                    catch
                    {
                        // await DisplayAlert("Alert", "Please,  Check Your Connection ", "ok");
                    }
                   
                }

                a++;
            }
            PDWGameName.IsEnabled = true;
        }
    }
}






