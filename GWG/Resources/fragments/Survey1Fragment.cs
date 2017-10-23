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
    public class Survey1Fragment : Android.Support.V4.App.Fragment {

        private TextView mTxtErrorMessage;
        private Button mBtnContSurvey;
        private SurveyResults mSurveyResults = new SurveyResults();
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

            mTxtErrorMessage = view.FindViewById<TextView>(Resource.Id.txtErrorMessage);

            mBtnContSurvey = view.FindViewById<Button>(Resource.Id.btnContSurvey);
            mBtnContSurvey.Click += MBtnContSurvey_Click;

            RadioButton q1Yes = view.FindViewById<RadioButton>(Resource.Id.q1Yes);
            RadioButton q1No = view.FindViewById<RadioButton>(Resource.Id.q1No);
            RadioButton q2Yes = view.FindViewById<RadioButton>(Resource.Id.q2Yes);
            RadioButton q2No = view.FindViewById<RadioButton>(Resource.Id.q2No);
            RadioButton q3Yes = view.FindViewById<RadioButton>(Resource.Id.q3Yes);
            RadioButton q3No = view.FindViewById<RadioButton>(Resource.Id.q3No);
            q1Yes.Click += Q1_Click;
            q1No.Click += Q1_Click;
            q2Yes.Click += Q2_Click;
            q2No.Click += Q2_Click;
            q3Yes.Click += Q3_Click;
            q3No.Click += Q3_Click;

            return view;
        }

        private void Q1_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            mSurveyResults.q1 = rb.Text;
        }

        private void Q2_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            mSurveyResults.q2 = rb.Text;
        }

        private void Q3_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            mSurveyResults.q3 = rb.Text;
        }

        private void MBtnContSurvey_Click(object sender, EventArgs e)
        {
            // Save Values
            mArgs.PutString("surveyResults", JsonConvert.SerializeObject(mSurveyResults));
            mSurvey2Fragment.Arguments = mArgs;

            // Confirm all Values
            if (String.IsNullOrWhiteSpace(mSurveyResults.q1))
            {
                mTxtErrorMessage.Text = "Please answer question #1";
            }
            else if (String.IsNullOrWhiteSpace(mSurveyResults.q2))
            {
                mTxtErrorMessage.Text = "Please answer question #2";
            }
            else if (String.IsNullOrWhiteSpace(mSurveyResults.q3))
            {
                mTxtErrorMessage.Text = "Please answer question #3";
            } else
            {
                var trans = this.FragmentManager.BeginTransaction();
                trans.Replace(Resource.Id.fragmentContainer, mSurvey2Fragment);
                trans.Commit();
            }
        }
    }
}