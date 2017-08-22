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
    public class CalendarEventArg : EventArgs
    {
        private DateTime mDate;

        public DateTime Date
        {
            get { return mDate; }
            set { mDate = value; }
        }

        public CalendarEventArg(DateTime date) : base()
        {
            Date = date;
        }

    }
    public class dialog_Calendar : Android.Support.V4.App.DialogFragment
    {
        private TextView mCalendarText;
        private DatePicker mCalendarView;
        private Button mBtnSetCalendar;

        private DateTime mSelectedDate;

        public event EventHandler<CalendarEventArg> mCalendarComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_calendar, container, false);

            mCalendarText = view.FindViewById<TextView>(Resource.Id.viewCalendarText);
            mCalendarView = view.FindViewById<DatePicker>(Resource.Id.viewCalendar);
            //mCalendarView += MCalendarView_DateChange;
            //mSelectedDate = new DateTime(mCalendarView.Date * 10000); // Initialize Default Value

            mBtnSetCalendar = view.FindViewById<Button>(Resource.Id.btnSetCalendar);
            mBtnSetCalendar.Click += MBtnSaveCalendar_Click;

            // Set Dialog Title
            mCalendarText.Text = this.Arguments.GetString("title");

            // Set Calendar Date
            long tempDate = this.Arguments.GetLong("dueDate");
            if (tempDate > 0)
            {
                mSelectedDate = new DateTime(tempDate);
                mCalendarView.UpdateDate(mSelectedDate.Year, mSelectedDate.Month-1, mSelectedDate.Day);
            }

            return view;
        }

        private void MCalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            mSelectedDate = new DateTime(e.Year, e.Month+1, e.DayOfMonth);
        }

        private void MBtnSaveCalendar_Click(object sender, EventArgs e)
        {
            mCalendarComplete.Invoke(this, new CalendarEventArg(mCalendarView.DateTime));
            this.Dismiss();
        }
    }
}