
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ecaportion.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toast_Droid))]
namespace Ecaportion.Droid
{
    public class Toast_Droid : IToastInterface
    {
        public void Longtime(string message)
        {
            // IToast.MakeText(Android.App.Context, message, ToastLength.Long).Show();

            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void Shorttime(string message)
        {
            throw new NotImplementedException();
        }
    }
}