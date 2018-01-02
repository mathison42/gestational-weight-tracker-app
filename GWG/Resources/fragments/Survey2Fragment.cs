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
using Android.Text;
using Android.Text.Style;

namespace GWG.Resources.fragments
{
    public class Survey2Fragment : Android.Support.V4.App.Fragment {

        private TextView mTxtErrorMessage;
        private Button mBtnContSurvey;

        private TextView mViewSurveyQ4;
        private EditText mQ4Value;
        private CheckBox mQ4NotSure;
        private TextView mViewSurveyQ5;

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

            mTxtErrorMessage = view.FindViewById<TextView>(Resource.Id.txtErrorMessage);

            mBtnContSurvey = view.FindViewById<Button>(Resource.Id.btnContSurvey);
            mBtnContSurvey.Click += MBtnContSurvey_Click;

            mViewSurveyQ4 = view.FindViewById<TextView>(Resource.Id.viewSurveyQ4);
            mQ4Value = view.FindViewById<EditText>(Resource.Id.q4Value);
            mQ4NotSure = view.FindViewById<CheckBox>(Resource.Id.checkboxNotSure);
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
            /**mSurveyResults.q4 = "";
            mSurveyResults.q5 = "";*/

            // Make Q4 invisible if Q3 is No
            if (mSurveyResults.q3.ToLower() == "no" || mSurveyResults.q3.ToLower() == "n")
            {
                mViewSurveyQ4.Visibility = Android.Views.ViewStates.Invisible;
                mQ4NotSure.Visibility = Android.Views.ViewStates.Invisible;
                mQ4Value.Visibility = Android.Views.ViewStates.Invisible;
                mQ4Value.Text = "N/A";
            }

            // Underline Q5 "just before"
            mViewSurveyQ5 = view.FindViewById<TextView>(Resource.Id.viewSurveyQ5);
            SpannableStringBuilder builderQ5 = new SpannableStringBuilder();
            string q5Str = Resources.GetString(Resource.String.surveyQ5);
            string q5UnderlineStr = Resources.GetString(Resource.String.surveyQ5Underline);
            builderQ5.Append(q5Str);
            int startUnderline = q5Str.IndexOf(q5UnderlineStr);
            int endUnderline = startUnderline + q5UnderlineStr.Length;

            builderQ5.SetSpan(new UnderlineSpan(), startUnderline, endUnderline, SpanTypes.ExclusiveExclusive);
            mViewSurveyQ5.TextFormatted = builderQ5;
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
            if (mQ4NotSure.Checked)
            {
                mSurveyResults.q4 = "Not Sure";
            }
            else
            {
                mSurveyResults.q4 = mQ4Value.Text;

            }
            mArgs.PutString("surveyResults", JsonConvert.SerializeObject(mSurveyResults));
            mSurvey3Fragment.Arguments = mArgs;

            var trans = this.FragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mSurvey3Fragment);
            trans.Commit();
        }
    }
}