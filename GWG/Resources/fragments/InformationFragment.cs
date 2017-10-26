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

        private Button mBtnHowYourBabyGrows;
        private Button mBtnNutritionAndWeightGain;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Bundle bundle = Arguments;

            View view = inflater.Inflate(Resource.Layout.Information, container, false);
            mBtnHowYourBabyGrows = view.FindViewById<Button>(Resource.Id.btnHowYourBabyGrows);
            mBtnNutritionAndWeightGain = view.FindViewById<Button>(Resource.Id.btnNutritionAndWeightGain);

            mBtnHowYourBabyGrows.Click += MBtnHowYourBabyGrows_Click;
            mBtnNutritionAndWeightGain.Click += MBtnNutritionAndWeightGain_Click;
            return view;
        }

        private void MBtnHowYourBabyGrows_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("http://www.xamarin.com");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void MBtnNutritionAndWeightGain_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("http://www.google.com");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
    }
}