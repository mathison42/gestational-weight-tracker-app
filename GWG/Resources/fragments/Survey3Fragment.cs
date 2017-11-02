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
using static Android.Resource;
using Android.Text.Style;
using Android.Graphics;
using GWG.Resources.redcap;

namespace GWG.Resources.fragments
{
    public class Survey3Fragment : Android.Support.V4.App.Fragment {

        private TextView mTxtErrorMessage;
        private Button mBtnCompleteSurvey;

        private TextView mViewSurveyQ6;
        private EditText mQ6Value;
        private TextView mViewSurveyQ7;
        private EditText mQ7Value;

        private SurveyResults mSurveyResults = new SurveyResults();
        REDCapResult mRecord;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.survey3, container, false);

            Bundle bundle = Arguments;
            mSurveyResults = JsonConvert.DeserializeObject<SurveyResults>(bundle.GetString("surveyResults"));
            mRecord = JsonConvert.DeserializeObject<REDCapResult>(bundle.GetString("record"));

            mTxtErrorMessage = view.FindViewById<TextView>(Resource.Id.txtErrorMessage);

            mBtnCompleteSurvey = view.FindViewById<Button>(Resource.Id.btnCompleteSurvey);
            mBtnCompleteSurvey.Click += MBtnCompleteSurvey_Click;

            // Underline Q6 "should ideally gain"
            mViewSurveyQ6 = view.FindViewById<TextView>(Resource.Id.viewSurveyQ6);
            SpannableStringBuilder builderQ6 = new SpannableStringBuilder();
            string q6Str = Resources.GetString(Resource.String.surveyQ6);
            string q6UnderlineStr = Resources.GetString(Resource.String.surveyQ6Underline);
            builderQ6.Append(q6Str);
            int startUnderline = q6Str.IndexOf(q6UnderlineStr);
            int endUnderline = startUnderline + q6UnderlineStr.Length;

            builderQ6.SetSpan(new UnderlineSpan(), startUnderline, endUnderline, SpanTypes.ExclusiveExclusive);
            mViewSurveyQ6.TextFormatted = builderQ6;

            // Underline Q7 "will gain"
            mViewSurveyQ7 = view.FindViewById<TextView>(Resource.Id.viewSurveyQ7);
            SpannableStringBuilder builderQ7 = new SpannableStringBuilder();
            string q7Str = Resources.GetString(Resource.String.surveyQ7);
            string q7UnderlineStr = Resources.GetString(Resource.String.surveyQ7Underline);
            builderQ7.Append(q7Str);
            startUnderline = q7Str.IndexOf(q7UnderlineStr);
            endUnderline = startUnderline + q7UnderlineStr.Length;

            builderQ7.SetSpan(new UnderlineSpan(), startUnderline, endUnderline, SpanTypes.ExclusiveExclusive);
            mViewSurveyQ7.TextFormatted = builderQ7;

            mQ6Value = view.FindViewById<EditText>(Resource.Id.q6Value);
            mQ7Value = view.FindViewById<EditText>(Resource.Id.q7Value);

            return view;
        }

        private void MBtnCompleteSurvey_Click(object sender, EventArgs e)
        {
            // Confirm Values
            mSurveyResults.q6 = mQ6Value.Text;
            mSurveyResults.q7 = mQ7Value.Text;

            if (System.String.IsNullOrWhiteSpace(mSurveyResults.q6))
            {
                mTxtErrorMessage.Text = "Please answer question #6";
            }
            else if (System.String.IsNullOrWhiteSpace(mSurveyResults.q7))
            {
                mTxtErrorMessage.Text = "Please answer question #7";
            }
            else
            {
                mTxtErrorMessage.Text = "Survey Complete";
                mTxtErrorMessage.SetTextColor(Android.Graphics.Color.DarkGreen);
        
                // Save Survey
                mSurveyResults.toString();
                REDCapHelper rch = new REDCapHelper(mRecord.redcapid, mRecord.record_id);
                rch.AddRecord(mSurveyResults);
                
                mTxtErrorMessage.Text = "Survey Complete! Please log out.";
            }
        }
    }
}