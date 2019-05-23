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
    public partial class GameGroupMembers : ContentPage
    {
        List<string> ListOfStudents = new List<string>();
        List<string> ListOfGroups = new List<string>();
        List<string> ListOfIds = new List<string>();
        List<string> ListOfStdCodes = new List<string>();
        string[] StdCode = new string[250];
        string Listindex = "";
        int LoopCnt = 0;
        int cnt = 0;
        string StdInfo = "";
        int loopn = 0;

        string[] ArrStdDetails = new string[1000];

        List<Mydata> ram = new List<Mydata>();

        public GameGroupMembers()
        {
            InitializeComponent();
            MStdCodes();
            GroupNameheader.Text = DataFetch.GroupName.ToLower();
            GameNameheader.Text = DataFetch.GameName.ToLower();
            CountStd.Text = 0.ToString();
        }

        private async void MStdCodes()
        {
            try
            {
                string str = "select stud_code from game_participants where game_id='" + DataFetch.GameId + "'";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {

                        StdCode[i] = myDataSet.Tables[0].Rows[i]["stud_code"].ToString();
                        LoopCnt++;
                        ListOfStdCodes.Add(StdCode[i]);
                    }
                }
            }
            catch { }
            MStdName();
        }

        private async void MStdName()
        {

            for (int j = 0; j < LoopCnt; j++)
            {
                int Y = Convert.ToInt32(StdCode[j]);
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
                            ListOfStudents.Add(StdInfo);
                        }
                    }
                }
                catch { }

                ArrStdDetails[j] = StdInfo;
            }

            VDesignData();
        }

        private void VDesignData()
        {
            ListTest.ItemsSource = null;
            int sn = 1;
            for (int i = 0; i < LoopCnt; i++)
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

        private void ListTest_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string Listindex = e.ItemIndex.ToString();
            if (ListOfIds.Contains(Listindex))
            {
                ListOfIds.Remove(Listindex);
                cnt--;
            }
            else
            {
                ListOfIds.Add(Listindex);
                cnt++;
            }
            CountStd.Text = cnt.ToString();
        }

        private void GGMStudentsName_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ListOfIds.Contains(Listindex))
            {
                ListOfIds.Remove(Listindex);
                cnt--;
            }
            else
            {
                ListOfIds.Add(Listindex);
                cnt++;
            }
            CountStd.Text = cnt.ToString();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string[] ArrOFIndex = ListOfIds.ToArray();
            int jdjdj = ArrOFIndex.Length;
            for (int i = 0; i < jdjdj; i++)
            {
                int A = Convert.ToInt32(ArrOFIndex[i]);
                string QStdcode = StdCode[A];
                try
                {
                    string str = "INSERT INTO game_group_member (group_Id, stud_code, A_year) VALUES ('" + DataFetch.GroupId + "','" + QStdcode + "','" + DataFetch.AcademicYear + "')";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });
                    if (DataFetch.flag == "T")
                    {
                        DependencyService.Get<IToastInterface>().Longtime("Group Members Added Successfully");
                    }
                    else
                    {
                        DependencyService.Get<IToastInterface>().Longtime("Operation Failed! Try Again");
                    }
                }

                catch { }

            }
            ListOfStudents.Clear();
            await Navigation.PushAsync(new GroupMemberList());
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new GameGroup());
            return true;
        }

        private void VCTapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View.BackgroundColor == Color.Green)
            {
                viewCell.View.BackgroundColor = Color.Default;
            }
            else
            {
                viewCell.View.BackgroundColor = Color.Green;
            }

        }
    }
}

