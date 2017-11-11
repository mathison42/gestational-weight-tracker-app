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
    public class InformationFragment : Android.Support.V4.App.Fragment {

        private Button mBtnCommonPregnancyTopics;
        private Button mBtnTestsAndConditions;
        private Button mBtnLaborAndDelivery;

        private InformationCommonTopics mInformationCommonTopics;
        private InformationTestsAndConditions mInformationTestsAndConditions;
        private InformationLaborAndDelivery mInformationLaborAndDelivery;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Bundle bundle = Arguments;

            View view = inflater.Inflate(Resource.Layout.Information, container, false);
            mBtnCommonPregnancyTopics = view.FindViewById<Button>(Resource.Id.btnCommonPregnancyTopics);
            mBtnTestsAndConditions = view.FindViewById<Button>(Resource.Id.btnTestsAndConditions);
            mBtnLaborAndDelivery = view.FindViewById<Button>(Resource.Id.btnLaborAndDelivery);

            mBtnCommonPregnancyTopics.Click += MBtnCommonPregnancyTopics_Click;
            mBtnTestsAndConditions.Click += MBtnTestsAndConditions_Click;
            mBtnLaborAndDelivery.Click += mBtnLaborAndDelivery_Click;

            mInformationCommonTopics = new InformationCommonTopics();
            mInformationTestsAndConditions = new InformationTestsAndConditions();
            mInformationLaborAndDelivery = new InformationLaborAndDelivery();
            return view;
        }

        private void MBtnCommonPregnancyTopics_Click(object sender, EventArgs e)
        {
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mInformationCommonTopics);
            trans.Commit();
        }

        private void MBtnTestsAndConditions_Click(object sender, EventArgs e)
        {
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mInformationTestsAndConditions);
            trans.Commit();
        }

        private void mBtnLaborAndDelivery_Click(object sender, EventArgs e)
        {
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, mInformationLaborAndDelivery);
            trans.Commit();
        }
    }
}