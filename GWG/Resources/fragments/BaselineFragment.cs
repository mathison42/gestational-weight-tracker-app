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
        private Button mBtnSaveProfile;
        private TextView mViewDate;
        private TextView mViewSaveProfileError;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Baseline, container, false);

            mBtnCalcPeriod = view.FindViewById<Button>(Resource.Id.btnCalcPeriod);
            mBtnSetADate = view.FindViewById<Button>(Resource.Id.btnSetADate);
            mBtnSaveProfile = view.FindViewById<Button>(Resource.Id.btnSaveProfile);
            mViewDate = view.FindViewById<TextView>(Resource.Id.viewDate);
            mViewSaveProfileError = view.FindViewById<TextView>(Resource.Id.viewSaveProfileError);

            mBtnCalcPeriod.Click += MBtnCalcPeriod_Click;
            mBtnSetADate.Click += MBtnSetADate_Click;
            mBtnSaveProfile.Click += MBtnSaveProfile_Click;

            return view;
        }

        private void MBtnCalcPeriod_Click(object sender, EventArgs e)
        {
            //Pull up Calendar Dialog
            dialog_Calendar calendarDialog = new dialog_Calendar();
            calendarDialog.Show(this.FragmentManager, "Dialog Calendar Fragment");

            calendarDialog.mCalendarComplete += CalendarDialog_mCalendarComplete_Naegele;
            
        }

        private void CalendarDialog_mCalendarComplete_Naegele(object sender, CalendarEventArg e)
        {
            DateTime lmp = e.Date;
            Console.WriteLine("LMP Date: " + lmp.ToShortDateString());
            DateTime dueDate = calcNaegelesRule(lmp);
            Console.WriteLine("Due Date: " + dueDate.ToShortDateString());
            mViewDate.Text = dueDate.ToShortDateString();
        }

        private void MBtnSetADate_Click(object sender, EventArgs e)
        {
            //Pull up Calendar Dialog
            dialog_Calendar calendarDialog = new dialog_Calendar();
            calendarDialog.Show(this.FragmentManager, "Dialog Calendar Fragment");

            calendarDialog.mCalendarComplete += CalendarDialog_mCalendarComplete_SelectDate;
        }

        private void CalendarDialog_mCalendarComplete_SelectDate(object sender, CalendarEventArg e)
        {
            DateTime date = e.Date;
            Console.WriteLine("Selected Date: " + date.ToShortDateString());
            mViewDate.Text = date.ToShortDateString();
        }

        private void MBtnSaveProfile_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
    }
}