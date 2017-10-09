using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GWG
{
    public class DateWeight
    {
        public long mDate { get; set; }
        public double mWeight { get; set; }

        public DateWeight(long date, double weight)
        {
            mDate = date;
            mWeight = weight;
        }

        public void toString()
        {
            Console.WriteLine("Date:Weight -> " + mDate + ":" + mWeight);
        }
        
    }
}