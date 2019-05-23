using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecaportion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupMemberList : ContentPage
    {
        int loopcnt = 0;
        string[] arr = new string[500];

        List<string> GMList = new List<string>();
        int loopn = 0;

        string[] ArrStdDetails = new string[1000];

        List<Mydata> ram = new List<Mydata>();

        string StdInfo = "";

        public GroupMemberList()
        {
            InitializeComponent();
            VGroupName.Text = DataFetch.GroupName;
            GMLL();
        }




        private void Okkoo()
        {
            ListTest.ItemsSource = null;
            //  string[] ArrStdDetails = new string[1000];

            int sn = 1;
            for (int i = 0; i < loopcnt; i++)
            {
                ram.Add(new Mydata() { StdDetails = ArrStdDetails[i], Sno = sn.ToString() });
                sn++;
            }
            ListTest.ItemsSource = ram;

        }

        public class Mydata
        {
            public string StdDetails { get; set; }
            public string Sno { get; set; }

        }








        private async void GMLL()
        {
            loopcnt = 0;
            try
            {
                string str = "select stud_code from game_group_member where group_Id='" + DataFetch.GroupId + "'";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        arr[i] = myDataSet.Tables[0].Rows[i]["stud_code"].ToString();
                        loopcnt++;
                    }
                }
            }
            catch { }
            Total.Text = "(" + loopcnt.ToString() + ")";
            Okokok();
        }





        private async void Okokok()
        {
           
            for (int j = 0; j < loopcnt; j++)
            {
                int Y = Convert.ToInt32(arr[j]);
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

                            GMList.Add(StdInfo);
                           
                        }

                    }
                }
                catch { }

                ArrStdDetails[j] = StdInfo;
            }
            Okkoo();
          //  VGMList.ItemsSource = GMList;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new GameGroup());
        }


        private void AddTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new GameGroupMembers());
        }


        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new GameGroup());
            return true;
        }
    }
}