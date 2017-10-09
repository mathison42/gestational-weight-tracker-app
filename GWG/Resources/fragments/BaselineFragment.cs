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
using Android.Support.V4.App;
using static Android.Support.V4.App.FragmentManager;

namespace GWG.Resources.fragments
{
    public class BaselineFragment : Android.Support.V4.App.Fragment
    {
        private Button mBtnCalcPeriod;
        private Button mBtnSetADate;
        private TextView mViewDate;

        private EditText mTxtHeight;
        private EditText mTxtWeight;
        private TextView mTxtBMI;

        private TextView mViewSaveProfileError;
        private Button mBtnSaveProfile;

        private DateTime mDueDate;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Baseline, container, false);

            // Retrieve Objects
            mBtnCalcPeriod = view.FindViewById<Button>(Resource.Id.btnCalcPeriod);
            mBtnSetADate = view.FindViewById<Button>(Resource.Id.btnSetADate);
            mViewDate = view.FindViewById<TextView>(Resource.Id.viewDate);


            mTxtHeight = view.FindViewById<EditText>(Resource.Id.txtHeight);
            mTxtWeight = view.FindViewById<EditText>(Resource.Id.txtWeight);
            mTxtBMI    = view.FindViewById<TextView>(Resource.Id.txtBMI);

            mViewSaveProfileError = view.FindViewById<TextView>(Resource.Id.viewSaveProfileError);
            mBtnSaveProfile = view.FindViewById<Button>(Resource.Id.btnSaveProfile);

            Bundle bundle = Arguments;
            mDueDate = new DateTime(bundle.GetLong("dueDate"));
            if (mDueDate.Ticks > 0)
            {
                // Set values
                mViewDate.Text = mDueDate.ToShortDateString();
                mTxtHeight.Text = bundle.GetDouble("height").ToString();
                mTxtWeight.Text = bundle.GetDouble("weight").ToString();
                mTxtBMI.Text = bundle.GetDouble("bmi").ToString();

                // Disable EditTexts
                mTxtHeight.Enabled = false;
                mTxtWeight.Enabled = false;
            } else
            {
                // Set Text Listeners
                mBtnCalcPeriod.Click += MBtnCalcPeriod_Click;
                mTxtHeight.AfterTextChanged += MTxtHeight_AfterTextChanged;
                mTxtWeight.AfterTextChanged += MTxtWeight_AfterTextChanged;
            }

            // Set on Button Clicks
            mBtnSetADate.Click += MBtnSetADate_Click;
            mBtnSaveProfile.Click += MBtnSaveProfile_Click;

            return view;
        }

        private void MTxtHeight_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            Console.WriteLine("Height: " + e.Editable.ToString());
            setTextBMI(e.Editable.ToString(), mTxtWeight.Text);
        }

        private void MTxtWeight_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            Console.WriteLine("Weight: " + e.Editable.ToString());
            setTextBMI(mTxtHeight.Text, e.Editable.ToString());
        }

        private void setTextBMI(String heightStr, String weightStr)
        {
            if (String.IsNullOrEmpty(heightStr) || String.IsNullOrEmpty(weightStr))
            {
                mTxtBMI.Text = "";
            } else
            {
                double height;
                double weight;
                if (Double.TryParse(heightStr, out height))
                {
                    if (height <= 0)
                    {
                        mTxtBMI.Text = "Negative Height";
                        return;
                    }
                } else
                {
                    mTxtBMI.Text = "Invalid Height";
                    return;
                }
                if (Double.TryParse(weightStr, out weight))
                {
                    if (height <= 0)
                    {
                        mTxtBMI.Text = "Negative Weight";
                        return;
                    }
                }
                else
                {
                    mTxtBMI.Text = "Invalid Weight";
                    return;
                }

                mTxtBMI.Text = calculateBMI(height, weight).ToString("0.0");

            }
        }

        private void MBtnCalcPeriod_Click(object sender, EventArgs e)
        {
            Bundle bundle = new Bundle();
            bundle.PutString("title", "Select Date of Last Menstrual Period (LMP)");

            //Pull up Calendar Dialog
            dialog_Calendar calendarDialog = new dialog_Calendar();
            calendarDialog.Arguments = bundle;
            calendarDialog.Show(this.FragmentManager, "Dialog Calendar Fragment - Calculate Date");

            calendarDialog.mCalendarComplete += CalendarDialog_mCalendarComplete_Naegele;
            
        }

        private void CalendarDialog_mCalendarComplete_Naegele(object sender, CalendarEventArg e)
        {
            DateTime lmp = e.Date;
            Console.WriteLine("LMP Date: " + lmp.ToShortDateString());
            mDueDate = calcNaegelesRule(lmp);
            Console.WriteLine("Due Date: " + mDueDate.ToShortDateString());
            mViewDate.Text = mDueDate.ToShortDateString();
        }

        private void MBtnSetADate_Click(object sender, EventArgs e)
        {
            Bundle bundle = new Bundle();
            bundle.PutString("title", "Select Due Date");
            if (mDueDate.Ticks > 0)
            {
                bundle.PutLong("dueDate", mDueDate.Ticks);
            }
            else
            {
                bundle.PutLong("dueDate", DateTime.Today.AddMonths(9).Ticks);
            }

            //Pull up Calendar Dialog
            dialog_Calendar calendarDialog = new dialog_Calendar();
            calendarDialog.Arguments = bundle;
            calendarDialog.Show(this.FragmentManager, "Dialog Calendar Fragment - Set Date");

            calendarDialog.mCalendarComplete += CalendarDialog_mCalendarComplete_SelectDate;
        }

        private void CalendarDialog_mCalendarComplete_SelectDate(object sender, CalendarEventArg e)
        {
            mDueDate = e.Date;
            Console.WriteLine("Selected Date: " + mDueDate.ToShortDateString());
            mViewDate.Text = mDueDate.ToShortDateString();
        }

        private void MBtnSaveProfile_Click(object sender, EventArgs e)
        {
            DateTime dueDate;
            if (!DateTime.TryParse(mViewDate.Text, out dueDate))
            {
                mViewSaveProfileError.Text = "Specify a Due Date";
                return;
            }

            double bmi;
            if (!Double.TryParse(mTxtBMI.Text, out bmi))
            {
                mViewSaveProfileError.Text = "Calculate a BMI";
                return;
            }

            double weight;
            if (!Double.TryParse(mTxtWeight.Text, out weight))
            {
                mViewSaveProfileError.Text = "Invalid weight";
                return;
            }
            weight = Math.Round(weight, 1);

            double height;
            if (!Double.TryParse(mTxtHeight.Text, out height))
            {
                mViewSaveProfileError.Text = "Invalid height";
                return;
            }
            height = Math.Round(height, 1);

            // Save Profile Data in Database...
            ((MainToolbarActivity)Activity).saveBaseline(dueDate, weight, height, bmi);

            // Freeze Baseline View
            mTxtHeight.InputType = Android.Text.InputTypes.Null;
            mTxtWeight.InputType = Android.Text.InputTypes.Null;
            mViewSaveProfileError.Text = "Profile Saved";
            mViewSaveProfileError.SetTextColor(Android.Graphics.Color.ForestGreen);
        }

        // https://en.wikipedia.org/wiki/Naegele%27s_rule
        private DateTime calcNaegelesRule(DateTime lmp)
        {
            DateTime dueDate = lmp;
            dueDate = dueDate.AddYears(1);
            dueDate = dueDate.AddMonths(-3);
            dueDate = dueDate.AddDays(7);
            return dueDate;
        }

        // http://www.bmi-calculator.net/bmi-formula.php
        // BMI = (Weight in Pounds / (Height in inches x Height in inches)) x 703
        private double calculateBMI(double height, double weight)
        {
            return (weight / (height * height)) * 703;
        }
    }
}