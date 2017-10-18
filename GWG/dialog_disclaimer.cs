using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GWG
{
    public class dialog_disclaimer : Android.Support.V4.App.DialogFragment
    {
        private Button mBtnAccept;
        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_disclaimer, container, false);

            mBtnAccept = view.FindViewById<Button>(Resource.Id.btnAccept);
            mBtnAccept.Click += MBtnAccept_Click;

            // Forces user to accept the disclaimer
            Cancelable = false;

            return view;
        }

        private void MBtnAccept_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}