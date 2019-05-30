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
    public partial class GameParticipants : ContentPage
    {
        List<string> ListOfClasses = new List<string>();
        List<string> ListOfSections = new List<string>();
        List<string> ListOfStudents = new List<string>();
        string[] ArrGameParticipants = new string[1000];
        List<Mydata> ram = new List<Mydata>();
        int loopn = 0;
        string[] ArrStudents = new string[1000];
        List<Mydata> ram1 = new List<Mydata>();
        int loopn4 = 0;
        int StdN = 0;
        int tsec = 0;
        int count = 0;
        int participantsno = 0;
        string qseck = "";
        string qclassk = "";
        string Listindex = "";
        string StdInfo = "";
        List<string> ListOfIds = new List<string>();
        List<string> testzone = new List<string>();
        List<string> VGPlist = new List<string>();
        string[] AStdCode = new string[250];
        string[] Gid = new string[250];
        string[] arr = new string[250];
        string[] ClassCode = new string[250];
        string[] SecCode = new string[250];
        int loopcnt = 0;
        public GameParticipants()
        {
            InitializeComponent();
            VGameName.Text = DataFetch.GameName;
            TSelected.Text = 0.ToString();
            GameParticipantsList();
        }

        private async void GameParticipantsList()
        {
            try
            {
                string str = "select stud_code from game_participants where game_Id='" + DataFetch.GameId + "'";
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
            Total.Text = loopcnt.ToString();
            Okokok();
        }

        private void Okkoo()
        {
            ListTest.ItemsSource = null;
            int sn = 1;
            for (int i = 0; i < loopcnt; i++)
            {
                ram.Add(new Mydata() { STDDetails = ArrGameParticipants[i], Sno = sn.ToString() });
                sn++;
            }
            ListTest.ItemsSource = ram;
        }

        private void Okkoo1()
        {
            ListTest1.ItemsSource = null;
            int sn = 1;
            for (int i = 0; i < loopn4; i++)
            {
                ram1.Add(new Mydata() { STDDetails = ArrStudents[i], Sno = sn.ToString() });
                sn++;
            }
            ListTest1.ItemsSource = ram1;
        }

        public class Mydata
        {
            public string STDDetails { get; set; }
            public string Sno { get; set; }
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
                            VGPlist.Add(StdInfo);
                        }

                    }
                }
                catch { }
                ArrGameParticipants[j] = StdInfo;
            }
            Okkoo();
        }

        private async void PClassShow()
        {
            string classname = "";
            try
            {
                string str = "select class_code, class_desc from class";
                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        ClassCode[i] = myDataSet.Tables[0].Rows[i]["class_code"].ToString();
                        classname = myDataSet.Tables[0].Rows[i]["class_desc"].ToString();
                        ListOfClasses.Add(classname);
                    }
                    // loadingItems.IsVisible = false;
                    GPPClass.ItemsSource = ListOfClasses;
                }
            }
            catch { }
        }

        private void GPPClassSI(object sender, EventArgs e)
        {
            GPPSection.ItemsSource = null;
            ListOfSections.Clear();
            PSectionShow();
            BTNPShow.IsEnabled = false;
        }


        private async void PSectionShow()
        {
            string secname = "";
            string QCD = GPPClass.SelectedItem.ToString();
            try
            {
                string str = "SELECT distinct(sec_desc) , (section.sec_code)FROM student_admission_current_year INNER JOIN class ON student_admission_current_year.CLASS_CODE = class.class_code INNER JOIN section ON student_admission_current_year.SEC_CODE = section.SEC_CODE WHERE class_desc= '" + QCD + "'";

                DataSet myDataSet = new DataSet();
                await Task.Run(() =>
                {
                    myDataSet = DataFetch.GetData(str);
                });
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                    {
                        secname = myDataSet.Tables[0].Rows[i]["sec_desc"].ToString();
                        SecCode[i] = myDataSet.Tables[0].Rows[i]["sec_code"].ToString();

                        ListOfSections.Add(secname);
                    }
                    // loadingItems.IsVisible = false;
                    GPPSection.ItemsSource = ListOfSections;
                }
            }
            catch { }
        }

        private void AddPCE(object sender, EventArgs e)
        {
            PClassShow();
            GPLBlock.IsVisible = false;
            ADDGPBlock.IsVisible = true;
        }

        private async void BTNPShow_Clicked(object sender, EventArgs e)
        {
            loopn4 = 0;
            Details.Text = DataFetch.GameName.ToLower();
            ListTest1.ItemsSource = null;
            ram1.Clear();
            ListOfStudents.Clear();
            int abc = GPPSection.SelectedIndex;
            int cde = GPPClass.SelectedIndex;
            qseck = SecCode[abc];
            qclassk = ClassCode[cde];
            if (qseck != null & qclassk != null)
            {
                try
                {
                    string str = "select ID, (FIRST_NAME  +' '+   MIDDLE_NAME + ' '  +LAST_NAME)  AS NAME from student inner join student_admission_current_year on student.STUD_CODE=student_admission_current_year.STUD_CODE where CLASS_CODE='" + qclassk + "' AND SEC_CODE='" + qseck + "'";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });
                    if (myDataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                        {
                            AStdCode[i] = myDataSet.Tables[0].Rows[i]["ID"].ToString();
                            string Stddetails = myDataSet.Tables[0].Rows[i]["NAME"].ToString();
                            ArrStudents[i] = Stddetails;
                            loopn4++;
                            ListOfStudents.Add(Stddetails);
                        }

                    }
                    abcd.IsVisible = true;
                }
                catch { }
            }

            Okkoo1();
            ListOfIds.Clear();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string[] ArrOFIndex = ListOfIds.ToArray();
            int run = ArrOFIndex.Length;
            for (int i = 0; i < run; i++)
            {
                string a = ArrOFIndex[i];
                int b = Convert.ToInt32(a);
                try
                {
                    string str = "INSERT INTO game_participants (game_Id,stud_code,A_year,game_date) Values('" + DataFetch.GameId + "','" + AStdCode[b] + "','" + DataFetch.AcademicYear + "','" + DataFetch.GameDate + "')";
                    DataSet myDataSet = new DataSet();
                    await Task.Run(() =>
                    {
                        myDataSet = DataFetch.GetData(str);
                    });
                }
                catch { }
            }

            if (DataFetch.flag == "T")
            {
                DependencyService.Get<IToastInterface>().Longtime("Game Participants Added Successfully");
                ListOfStudents.Clear();
                ListOfIds.Clear();
                Application.Current.MainPage = new NavigationPage(new GameParticipants());
            }
            else
            {
                DependencyService.Get<IToastInterface>().Longtime("Operation Failed! Try Again");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new GameGroup());
        }

        private void GPPSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTNPShow.IsEnabled = true;
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new GameDate());
            return true;
        }

        private void ListTest_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string Listindex = e.ItemIndex.ToString();
            if (ListOfIds.Contains(Listindex))
            {
                ListOfIds.Remove(Listindex);
                StdN--;
            }
            else
            {
                ListOfIds.Add(Listindex);
                StdN++;
            }
            TSelected.Text = StdN.ToString();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
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




