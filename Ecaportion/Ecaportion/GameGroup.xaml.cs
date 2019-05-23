using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecaportion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameGroup : ContentPage
    {
        List<string> Grpname = new List<string>();
        List<string> Grpnameup = new List<string>();
        List<string> gamenamelist = new List<string>();
        List<string> stdlist = new List<string>();
        string[] atdcode = new string[250];
        string[] ArrGameGroup = new string[1000];
        List<Mydata> LayoutDataList = new List<Mydata>();
        int loopn = 0;
        string[] groupidok = new string[200];
        int loopcnt = 0;
        string[] dbgameid = new string[500];
        public GameGroup()
        {
            InitializeComponent();
            GroupsList();
        }

        private async void GroupsList()
        {
            Grpname.Clear();
            Grpnameup.Clear();
            loopcnt = 0;
            try
            {
                string str = "select group_Id, group_name from game_group where game_id='" + DataFetch.GameId + "'";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        string dc = myDataSet.Tables[0].Rows[i]["group_name"].ToString();
                        string dcup = dc.ToUpper();
                        groupidok[i] = myDataSet.Tables[0].Rows[i]["group_Id"].ToString();
                        Grpnameup.Add(dcup);
                        Grpname.Add(dc);

                        ArrGameGroup[i] = dc;
                        loopcnt++;
                    }
                }
            }
            catch { }
            VDesignData();
        }

        private void VDesignData()
        {
            VGroupListView.ItemsSource = null;
            LayoutDataList.Clear();
            int sn = 1;
            for (int i = 0; i < loopcnt; i++)
            {
                LayoutDataList.Add(new Mydata() { GroupName = ArrGameGroup[i], Sno = sn.ToString() });
                sn++;
            }
            VGroupListView.ItemsSource = LayoutDataList;
        }

        public class Mydata
        {
            public string GroupName { get; set; }
            public string Sno { get; set; }
        }




        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (GGGTN.Text != null & GGGTN.Text != string.Empty)
            {
                string QGrpName = GGGTN.Text.ToUpper();
                if (Grpnameup.Contains(QGrpName))
                {
                    await DisplayAlert("Alert", "Group Name Already Exists! Try New One", "ok");
                }
                else
                {
                    try
                    {
                        string str = "INSERT INTO game_group(group_name, A_year, game_id) VALUES('" + GGGTN.Text + "', '" + DataFetch.AcademicYear + "', '" + DataFetch.GameId + "')";
                        DataSet myDataSet = new DataSet();
                        await Task.Run(() =>
                        {
                            myDataSet = DataFetch.GetData(str);
                        });

                        if (DataFetch.flag == "T")
                        {
                            AddGrp.IsVisible = false;
                            DependencyService.Get<IToastInterface>().Longtime("Group Formed Successfully");
                            VGroupListView.ItemsSource = null;
                            Grpname.Clear();
                            GGGTN.Text = null;
                            GroupsList();
                        }
                        else
                        {
                            DependencyService.Get<IToastInterface>().Longtime("Operation Failed! Try Again");
                        }

                    }

                    catch { }
                }

            }
            else
            {

                await DisplayAlert("Alert", "Please Enter Group Name", "Ok");
            }
        }




        private void GroupSelected(object sender, ItemTappedEventArgs e)
        {
            var listindex = e.ItemIndex;
            DataFetch.GroupId = groupidok[listindex];
            DataFetch.GroupName = ArrGameGroup[listindex];
            Application.Current.MainPage = new NavigationPage(new GroupMemberList());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            AddGrp.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new GameParticipants());
            return true;
        }


        private void CloseBtnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new GroupMemberList());
        }
    }
}