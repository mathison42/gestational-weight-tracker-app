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

            // Get Due Date and Standard Calc Info
            long tempDate = this.Arguments.GetLong("dueDate");
            DateTime origin = new DateTime(1970, 1, 1);
            long today = (long)(DateTime.Today.AddDays(1).Date - origin).TotalMilliseconds;

            // Set Dialog Title
            string title = this.Arguments.GetString("title");
            if (title == "LMP")
            {
                mCalendarText.Text = "Select Date of Last Menstrual Period (LMP)";

                // Set Calendar Current, Min, and Max Dates
                mCalendarView.MinDate = (long)(DateTime.Today.AddMonths(-4).Date - origin).TotalMilliseconds;
                mCalendarView.MaxDate = today;
            }
            else
            {
                mCalendarText.Text = "Select Due Date";

                // Set Calendar Current, Min, and Max Dates
                mCalendarView.MinDate = today;
                mCalendarView.MaxDate = (long)(DateTime.Today.AddMonths(10).Date - origin).TotalMilliseconds;
                if (tempDate > 0)
                {
                    mSelectedDate = new DateTime(tempDate);
                    mCalendarView.UpdateDate(mSelectedDate.Year, mSelectedDate.Month - 1, mSelectedDate.Day);
                    if (tempDate < today)
                    {
                        mCalendarView.MinDate = tempDate;
                    }
                }
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