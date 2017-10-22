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

using Android.Support.V4.Widget;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using GWG.Resources.fragments;

namespace GWG.Resources.fragments
{
    public class Survey3Fragment : Android.Support.V4.App.Fragment {
        
        private Button mBtnCompleteSurvey;

        private Survey3Fragment mSurvey3Fragment;
        Java.IO.ISerializable mRecord;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.survey3, container, false);

            mBtnCompleteSurvey = view.FindViewById<Button>(Resource.Id.btnCompleteSurvey);
            mBtnCompleteSurvey.Click += MBtnCompleteSurvey_Click;

            return view;
        }

        private void MBtnCompleteSurvey_Click(object sender, EventArgs e)
        {
            // Confirm Values

            // Save Survey

            // Send user back to Graph View

            var intent = new Intent(this.Context, typeof(MainToolbarActivity));
            Bundle bundle = Arguments;
            intent.PutExtra("record", bundle.GetString("record"));
            StartActivity(intent);
        }
    }
}