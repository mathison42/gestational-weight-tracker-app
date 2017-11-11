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
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using GWG.Resources.adapter;
using Java.Util;
using GWG.Resources.redcap;

namespace GWG.Resources.fragments
{
    public class InformationCommonTopics : Android.Support.V4.App.Fragment {

        private Button mBtnHowYourBabyGrows;
        private Button mBtnNutritionAndWeightGain;
        private Button mBtnMorningSickness;
        private Button mBtnExerciseDuringPregnancy;
        private Button mBtnTobaccoAlcoholAndDrugs;
        private Button mBtnBleedingDuringPregnancy;
        private Button mBtnBackPain;
        private Button mBtnCarSafety;
        private Button mBtnTravelDuringPregnancy;
        private Button mBtnGroupBStrep;
        private Button mBtnReducingRisksOfBirthDefects;
        private Button mBtnSkinConditions;
        private Button mBtnBack;

        private InformationFragment mInformationFragment;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Bundle bundle = Arguments;

            mInformationFragment = new InformationFragment();

            View view = inflater.Inflate(Resource.Layout.info_common_topics, container, false);
            mBtnHowYourBabyGrows = view.FindViewById<Button>(Resource.Id.btnHowYourBabyGrows);
            mBtnNutritionAndWeightGain = view.FindViewById<Button>(Resource.Id.btnNutritionAndWeightGain);
            mBtnMorningSickness = view.FindViewById<Button>(Resource.Id.btnMorningSickness);
            mBtnExerciseDuringPregnancy = view.FindViewById<Button>(Resource.Id.btnExerciseDuringPregnancy);
            mBtnTobaccoAlcoholAndDrugs = view.FindViewById<Button>(Resource.Id.btnTobaccoAlcoholAndDrugs);
            mBtnBleedingDuringPregnancy = view.FindViewById<Button>(Resource.Id.btnBleedingDuringPregnancy);
            mBtnBackPain = view.FindViewById<Button>(Resource.Id.btnBackPain);
            mBtnCarSafety = view.FindViewById<Button>(Resource.Id.btnCarSafety);
            mBtnTravelDuringPregnancy = view.FindViewById<Button>(Resource.Id.btnTravelDuringPregnancy);
            mBtnGroupBStrep = view.FindViewById<Button>(Resource.Id.btnGroupBStrep);
            mBtnReducingRisksOfBirthDefects = view.FindViewById<Button>(Resource.Id.btnReducingRisksOfBirthDefects);
            mBtnSkinConditions = view.FindViewById<Button>(Resource.Id.btnSkinConditions);
            mBtnBack = view.FindViewById<Button>(Resource.Id.btnBack);

            mBtnHowYourBabyGrows.Click += MBtnHowYourBabyGrows_Click;
            mBtnNutritionAndWeightGain.Click += MBtnNutritionAndWeightGain_Click;
            mBtnMorningSickness.Click += MBtnMorningSickness_Click;
            mBtnExerciseDuringPregnancy.Click += MBtnExerciseDuringPregnancy_Click;
            mBtnTobaccoAlcoholAndDrugs.Click += MBtnTobaccoAlcoholAndDrugs_Click;
            mBtnBleedingDuringPregnancy.Click += MBtnBleedingDuringPregnancy_Click;
            mBtnBackPain.Click += MBtnBackPain_Click;
            mBtnCarSafety.Click += MBtnCarSafety_Click;
            mBtnTravelDuringPregnancy.Click += MBtnTravelDuringPregnancy_Click;
            mBtnGroupBStrep.Click += MBtnGroupBStrep_Click;
            mBtnReducingRisksOfBirthDefects.Click += MBtnReducingRisksOfBirthDefects_Click;
            mBtnSkinConditions.Click += MBtnSkinConditions_Click;
            mBtnBack.Click += MBtnBack_Click;
            return view;
        }

        private void openLink(string link)
        { 
            var uri = Android.Net.Uri.Parse(link);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void MBtnHowYourBabyGrows_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq156.pdf?dmc=1&ts=20171107T0100060448");
        }

        private void MBtnNutritionAndWeightGain_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq001.pdf?dmc=1&ts=20171107T0058047164");
        }

        private void MBtnMorningSickness_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq126.pdf?dmc=1&ts=20171107T0058450134");
        }

        private void MBtnExerciseDuringPregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq119.pdf?dmc=1&ts=20171107T0100445136");
        }

        private void MBtnTobaccoAlcoholAndDrugs_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq170.pdf?dmc=1&ts=20171107T0101004511");
        }

        private void MBtnBleedingDuringPregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq038.pdf?dmc=1&ts=20171107T0059437947");
        }

        private void MBtnBackPain_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq115.pdf?dmc=1&ts=20171107T0059292947");
        }

        private void MBtnCarSafety_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq018.pdf?dmc=1&ts=20171107T0102102325");
        }

        private void MBtnTravelDuringPregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq055.pdf?dmc=1&ts=20171107T0102524670");
        }

        private void MBtnGroupBStrep_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq105.pdf?dmc=1&ts=20171107T0103334358");
        }

        private void MBtnReducingRisksOfBirthDefects_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq146.pdf?dmc=1&ts=20171107T0104134671");
        }

        private void MBtnSkinConditions_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq169.pdf?dmc=1&ts=20171107T0105324673");
        }

        private void MBtnBack_Click(object sender, EventArgs e)
        {
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mInformationFragment);
            trans.Commit();
        }
    }
}