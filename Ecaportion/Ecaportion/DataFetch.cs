using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
namespace Ecaportion
{
    public class DataFetch
    {
        public static string AcademicYear = "2076";
        public static string ip = "192.168.1.76";
        public static string flag = "";
        public static string GameId = "";
        public static string GameDate = "";
        public static string GroupId = "";
        public static string GameName = "";
        public static string GroupName = "";


        public static DataSet GetData(string qry)
        {
            flag = "";
            string aa = "";
#if __ANDROID__

            Ecaportion.Droid.WebReference.GetConnectService kk = new Droid.WebReference.GetConnectService
            {
                Url = "http://" + ip.ToString() + ":8888/getconnect"
            };
            try
            {
                aa = kk.getdatateacher(qry);
                flag = "T";
            }
            catch { }




#endif



            try
            {
                DataSet deserializedProduct = JsonConvert.DeserializeObject<DataSet>(aa);

                return deserializedProduct;
            }
            catch
            {
                return null;
            }

        }


    }
}
