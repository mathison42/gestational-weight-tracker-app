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
    class DateWeight
    {
        public DateTime mDate { get; set; }
        public int mWeight { get; set; }

        public DateWeight(DateTime date, int weight)
        {
            mDate = date;
            mWeight = weight;
        }
    }
}