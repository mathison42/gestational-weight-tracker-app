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
    public class Survey2Fragment : Android.Support.V4.App.Fragment {

        private Button mBtnContSurvey;

        private EditText mQ4Value;

        private SurveyResults mSurveyResults = new SurveyResults();
        private Survey3Fragment mSurvey3Fragment;
        Bundle mArgs = new Bundle();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            mSurvey3Fragment = new Survey3Fragment();
            Bundle bundle = Arguments;
            mSurveyResults = JsonConvert.DeserializeObject<SurveyResults>(bundle.GetString("surveyResults"));
            mArgs.PutString("record", bundle.GetString("record"));

            View view = inflater.Inflate(Resource.Layout.survey2, container, false);

            mBtnContSurvey = view.FindViewById<Button>(Resource.Id.btnContSurvey);
            mBtnContSurvey.Click += MBtnContSurvey_Click;

            mQ4Value = view.FindViewById<EditText>(Resource.Id.q4Value);
            RadioButton q5Underweight = view.FindViewById<RadioButton>(Resource.Id.q5Underweight);
            RadioButton q5NormalWeight = view.FindViewById<RadioButton>(Resource.Id.q5NormalWeight);
            RadioButton q5Overweight = view.FindViewById<RadioButton>(Resource.Id.q5Overweight);
            RadioButton q5Obese = view.FindViewById<RadioButton>(Resource.Id.q5Obese);
            RadioButton q5NotSure = view.FindViewById<RadioButton>(Resource.Id.q5NotSure);
            q5Underweight.Click += Q5_Click;
            q5NormalWeight.Click += Q5_Click;
            q5Overweight.Click += Q5_Click;
            q5Obese.Click += Q5_Click;
            q5NotSure.Click += Q5_Click;

            return view;
        }

        private void Q5_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            mSurveyResults.q5 = rb.Text;
        }

        private void MBtnContSurvey_Click(object sender, EventArgs e)
        {
            // Save Values
            mSurveyResults.q4 = mQ4Value.Text;
            mArgs.PutString("surveyResults", JsonConvert.SerializeObject(mSurveyResults));
            mSurvey3Fragment.Arguments = mArgs;

            // Confirm all Values

            var trans = this.FragmentManager.BeginTransaction();

            trans.Replace(Resource.Id.fragmentContainer, mSurvey3Fragment);
            trans.Commit();
        }
    }
}