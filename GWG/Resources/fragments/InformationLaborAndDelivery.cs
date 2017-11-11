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
    public class InformationLaborAndDelivery : Android.Support.V4.App.Fragment {

        private Button mBtnLaborBegins;
        private Button mBtnCesareanBirth;
        private Button mBtnFetalHeartRate;
        private Button mBtnBreastfeeding;
        private Button mBtnNewbornMaleCircumcision;
        private Button mBtnPastYourDueDate;
        private Button mBtnVaginalBirthAfterCSection;
        private Button mBtnPainReliefDuringLabor;
        private Button mBtnPretermLabor;
        private Button mBtnPostpartumDepression;
        private Button mBtnExerciseAfterPregnancy;
        private Button mBtnLaborInduction;
        private Button mBtnAssistedVaginalDelivery;
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

            View view = inflater.Inflate(Resource.Layout.info_labor_and_delivery, container, false);
            mBtnLaborBegins = view.FindViewById<Button>(Resource.Id.btnLaborBegins);
            mBtnCesareanBirth = view.FindViewById<Button>(Resource.Id.btnCesareanBirth);
            mBtnFetalHeartRate = view.FindViewById<Button>(Resource.Id.btnFetalHeartRate);
            mBtnBreastfeeding = view.FindViewById<Button>(Resource.Id.btnBreastfeeding);
            mBtnNewbornMaleCircumcision = view.FindViewById<Button>(Resource.Id.btnNewbornMaleCircumcision);
            mBtnPastYourDueDate = view.FindViewById<Button>(Resource.Id.btnPastYourDueDate);
            mBtnVaginalBirthAfterCSection = view.FindViewById<Button>(Resource.Id.btnVaginalBirthAfterCSection);
            mBtnPainReliefDuringLabor = view.FindViewById<Button>(Resource.Id.btnPainReliefDuringLabor);
            mBtnPretermLabor = view.FindViewById<Button>(Resource.Id.btnPretermLabor);
            mBtnPostpartumDepression = view.FindViewById<Button>(Resource.Id.btnPostpartumDepression);
            mBtnExerciseAfterPregnancy = view.FindViewById<Button>(Resource.Id.btnExerciseAfterPregnancy);
            mBtnLaborInduction = view.FindViewById<Button>(Resource.Id.btnLaborInduction);
            mBtnAssistedVaginalDelivery = view.FindViewById<Button>(Resource.Id.btnAssistedVaginalDelivery);
            mBtnBack = view.FindViewById<Button>(Resource.Id.btnBack);

            mBtnLaborBegins.Click += MBtnLaborBegins_Click;
            mBtnCesareanBirth.Click += MBtnCesareanBirth_Click;
            mBtnFetalHeartRate.Click += MBtnFetalHeartRate_Click;
            mBtnBreastfeeding.Click += MBtnBreastfeeding_Click;
            mBtnNewbornMaleCircumcision.Click += MBtnNewbornMaleCircumcision_Click;
            mBtnPastYourDueDate.Click += MBtnPastYourDueDate_Click;
            mBtnVaginalBirthAfterCSection.Click += MBtnVaginalBirthAfterCSection_Click;
            mBtnPainReliefDuringLabor.Click += MBtnPainReliefDuringLabor_Click;
            mBtnPretermLabor.Click += MBtnPretermLabor_Click;
            mBtnPostpartumDepression.Click += MBtnPostpartumDepression_Click;
            mBtnExerciseAfterPregnancy.Click += MBtnExerciseAfterPregnancy_Click;
            mBtnLaborInduction.Click += MBtnLaborInduction_Click;
            mBtnAssistedVaginalDelivery.Click += MBtnAssistedVaginalDelivery_Click;
            mBtnBack.Click += MBtnBack_Click;
            return view;
        }

        private void openLink(string link)
        {
            var uri = Android.Net.Uri.Parse(link);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void MBtnLaborBegins_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq004.pdf?dmc=1&ts=20171107T0108002645");
        }

        private void MBtnCesareanBirth_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq006.pdf?dmc=1&ts=20171107T0108015770");
        }

        private void MBtnFetalHeartRate_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq015.pdf?dmc=1&ts=20171107T0112168118");
        }

        private void MBtnBreastfeeding_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq029.pdf?dmc=1&ts=20171107T0108041082");
        }

        private void MBtnNewbornMaleCircumcision_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq039.pdf?dmc=1&ts=20171107T0112194368");
        }

        private void MBtnPastYourDueDate_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq069.pdf?dmc=1&ts=20171107T0112211400");
        }

        private void MBtnVaginalBirthAfterCSection_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq070.pdf?dmc=1&ts=20171107T0108089520");
        }

        private void MBtnPainReliefDuringLabor_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq086.pdf?dmc=1&ts=20171107T0113021557");
        }

        private void MBtnPretermLabor_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq087.pdf?dmc=1&ts=20171107T0113040307");
        }

        private void MBtnPostpartumDepression_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq091.pdf?dmc=1&ts=20171107T0113050463");
        }

        private void MBtnExerciseAfterPregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq131.pdf?dmc=1&ts=20171107T0113401245");
        }

        private void MBtnLaborInduction_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq154.pdf?dmc=1&ts=20171107T0113410932");
        }

        private void MBtnAssistedVaginalDelivery_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq192.pdf?dmc=1&ts=20171107T0113421245");
        }

        private void MBtnBack_Click(object sender, EventArgs e)
        {
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mInformationFragment);
            trans.Commit();
        }
    }
}