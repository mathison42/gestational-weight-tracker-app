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
    public class SurveyIntroFragment : Android.Support.V4.App.Fragment {

        private Button mBtnStartSurvey;


        private Survey1Fragment mSurvey1Fragment;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            mSurvey1Fragment = new Survey1Fragment();

            View view = inflater.Inflate(Resource.Layout.survey_intro, container, false);

            mBtnStartSurvey = view.FindViewById<Button>(Resource.Id.btnStartSurvey);

            mBtnStartSurvey.Click += MBtnStartSurvey_Click;

            return view;
        }

        private void MBtnStartSurvey_Click(object sender, EventArgs e)
        {
            var trans = this.FragmentManager.BeginTransaction();

            trans.Replace(Resource.Id.fragmentContainer, mSurvey1Fragment);
            trans.Commit();
        }
    }
}