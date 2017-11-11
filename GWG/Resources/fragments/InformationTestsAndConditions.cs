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
    public class InformationTestsAndConditions : Android.Support.V4.App.Fragment {

        private Button mBtnGestationalDiabetes;
        private Button mBtnHighBloodPressure;
        private Button mBtnWomenWithDiabetes;
        private Button mBtnObesityAndPregnancy;
        private Button mBtnMultiplePregnancy;
        private Button mBtnRoutinePregnancyTests;
        private Button mBtnCarrierScreening;
        private Button mBtnPrenatalScreeningTests;
        private Button mBtnPrenatalDiagnosticTests;
        private Button mBtnFluVaccineAndPregnancy;
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

            View view = inflater.Inflate(Resource.Layout.info_tests_and_conditions, container, false);
            mBtnGestationalDiabetes = view.FindViewById<Button>(Resource.Id.btnGestationalDiabetes);
            mBtnHighBloodPressure = view.FindViewById<Button>(Resource.Id.btnHighBloodPressure);
            mBtnWomenWithDiabetes = view.FindViewById<Button>(Resource.Id.btnWomenWithDiabetes);
            mBtnObesityAndPregnancy = view.FindViewById<Button>(Resource.Id.btnObesityAndPregnancy);
            mBtnMultiplePregnancy = view.FindViewById<Button>(Resource.Id.btnMultiplePregnancy);
            mBtnRoutinePregnancyTests = view.FindViewById<Button>(Resource.Id.btnRoutinePregnancyTests);
            mBtnCarrierScreening = view.FindViewById<Button>(Resource.Id.btnCarrierScreening);
            mBtnPrenatalScreeningTests = view.FindViewById<Button>(Resource.Id.btnPrenatalScreeningTests);
            mBtnPrenatalDiagnosticTests = view.FindViewById<Button>(Resource.Id.btnPrenatalDiagnosticTests);
            mBtnFluVaccineAndPregnancy = view.FindViewById<Button>(Resource.Id.btnFluVaccineAndPregnancy);
            mBtnBack = view.FindViewById<Button>(Resource.Id.btnBack);

            mBtnGestationalDiabetes.Click += MBtnGestationalDiabetes_Click;
            mBtnHighBloodPressure.Click += MBtnHighBloodPressure_Click;
            mBtnWomenWithDiabetes.Click += MBtnWomenWithDiabetes_Click;
            mBtnObesityAndPregnancy.Click += MBtnObesityAndPregnancy_Click;
            mBtnMultiplePregnancy.Click += MBtnMultiplePregnancy_Click;
            mBtnRoutinePregnancyTests.Click += MBtnRoutinePregnancyTests_Click;
            mBtnCarrierScreening.Click += MBtnCarrierScreening_Click;
            mBtnPrenatalScreeningTests.Click += MBtnPrenatalScreeningTests_Click;
            mBtnPrenatalDiagnosticTests.Click += MBtnPrenatalDiagnosticTests_Click;
            mBtnFluVaccineAndPregnancy.Click += MBtnFluVaccineAndPregnancy_Click;
            mBtnBack.Click += MBtnBack_Click;
            return view;
        }

        private void openLink(string link)
        {
            var uri = Android.Net.Uri.Parse(link);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void MBtnGestationalDiabetes_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq177.pdf?dmc=1&ts=20171107T0101504356");
        }

        private void MBtnHighBloodPressure_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq034.pdf?dmc=1&ts=20171107T0059048259");
        }

        private void MBtnWomenWithDiabetes_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq176.pdf?dmc=1&ts=20171107T0106184674");
        }

        private void MBtnObesityAndPregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq182.pdf?dmc=1&ts=20171107T0106591862");
        }

        private void MBtnMultiplePregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq188.pdf?dmc=1&ts=20171107T0107006550");
        }

        private void MBtnRoutinePregnancyTests_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq133.pdf?dmc=1&ts=20171107T0101318418");
        }

        private void MBtnCarrierScreening_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq179.pdf?dmc=1&ts=20171107T0106452331");
        }

        private void MBtnPrenatalScreeningTests_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq165.pdf?dmc=1&ts=20171107T0105072016");
        }

        private void MBtnPrenatalDiagnosticTests_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq164.pdf?dmc=1&ts=20171107T0105062797");
        }

        private void MBtnFluVaccineAndPregnancy_Click(object sender, EventArgs e)
        {
            openLink("https://www.acog.org/-/media/For-Patients/faq189.pdf?dmc=1&ts=20171107T0107016862");
        }

        private void MBtnBack_Click(object sender, EventArgs e)
        {
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mInformationFragment);
            trans.Commit();
        }
    }
}