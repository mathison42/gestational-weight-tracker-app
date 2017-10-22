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
using GWG.survey;
using Newtonsoft.Json;

namespace GWG.Resources.fragments
{
    public class Survey3Fragment : Android.Support.V4.App.Fragment {
        
        private Button mBtnCompleteSurvey;

        private EditText mQ6Value;
        private EditText mQ7Value;

        private SurveyResults mSurveyResults = new SurveyResults();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.survey3, container, false);

            Bundle bundle = Arguments;
            mSurveyResults = JsonConvert.DeserializeObject<SurveyResults>(bundle.GetString("surveyResults"));

            mBtnCompleteSurvey = view.FindViewById<Button>(Resource.Id.btnCompleteSurvey);
            mBtnCompleteSurvey.Click += MBtnCompleteSurvey_Click;

            mQ6Value = view.FindViewById<EditText>(Resource.Id.q6Value);
            mQ7Value = view.FindViewById<EditText>(Resource.Id.q7Value);

            return view;
        }

        private void MBtnCompleteSurvey_Click(object sender, EventArgs e)
        {
            // Confirm Values
            mSurveyResults.q6 = mQ6Value.Text;
            mSurveyResults.q7 = mQ7Value.Text;

            // Save Survey
            mSurveyResults.toString();

            // Send user back to Graph View
            var intent = new Intent(this.Context, typeof(MainToolbarActivity));
            Bundle bundle = Arguments;
            intent.PutExtra("record", bundle.GetString("record"));
            intent.PutExtra("surveyResults", JsonConvert.SerializeObject(mSurveyResults));
            StartActivity(intent);
        }
    }
}