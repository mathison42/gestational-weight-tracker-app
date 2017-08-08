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

namespace GWG
{
    public class DailyWeightEventArg : EventArgs
    {
        private long mTimestamp;
        private double mWeight;

        public long timestamp
        {
            get { return mTimestamp; }
            set { mTimestamp = value; }
        }

        public double weight
        {
            get { return mWeight; }
            set { mWeight = value; }
        }

        public DailyWeightEventArg(long timestamp, double weight) : base()
        {
            this.timestamp = timestamp;
            this.weight = weight;
        }

    }
    public class dialog_weight : Android.Support.V4.App.DialogFragment
    {
        private EditText mTxtWeight;
        private TextView mViewSaveWeightError;
        private Button mBtnSetWeight;

        public event EventHandler<DailyWeightEventArg> mDailyWeightComplete;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_weight, container, false);

            mTxtWeight = view.FindViewById<EditText>(Resource.Id.txtWeight);
            mViewSaveWeightError = view.FindViewById<TextView>(Resource.Id.viewSaveWeightError);
            mBtnSetWeight = view.FindViewById<Button>(Resource.Id.btnSetWeight);

            mBtnSetWeight.Click += MBtnSetWeight_Click;

            return view;
        }

        private void MBtnSetWeight_Click(object sender, EventArgs e)
        {
            string weightStr = mTxtWeight.Text;
            double weight;
            if (String.IsNullOrWhiteSpace(weightStr))
            {
                mViewSaveWeightError.Text = "Enter weight value.";
            }
            else if (Double.TryParse(weightStr, out weight))
            {
                weight = Math.Round(weight, 1);

                // User has clicked the Save Weight button and entered a valid weight
                mDailyWeightComplete.Invoke(this, new DailyWeightEventArg(DateTime.UtcNow.Ticks, weight));
                this.Dismiss();
            } else
            {
                mViewSaveWeightError.Text = "Weight must be a number.";
            }
        }
    }
}