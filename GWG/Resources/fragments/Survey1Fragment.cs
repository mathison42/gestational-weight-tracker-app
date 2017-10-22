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
    public class Survey1Fragment : Android.Support.V4.App.Fragment {

        private Button mBtnContSurvey;


        private Survey2Fragment mSurvey2Fragment;
        Bundle mArgs = new Bundle();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            mSurvey2Fragment = new Survey2Fragment();
            Bundle bundle = Arguments;
            mArgs.PutString("record", bundle.GetString("record"));

            View view = inflater.Inflate(Resource.Layout.survey1, container, false);

            mBtnContSurvey = view.FindViewById<Button>(Resource.Id.btnContSurvey);

            mBtnContSurvey.Click += MBtnContSurvey_Click;

            return view;
        }

        private void MBtnContSurvey_Click(object sender, EventArgs e)
        {
            mSurvey2Fragment.Arguments = mArgs;
            var trans = this.FragmentManager.BeginTransaction();

            trans.Replace(Resource.Id.fragmentContainer, mSurvey2Fragment);
            trans.Commit();
        }
    }
}