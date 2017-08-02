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

namespace GWG.Resources.fragments
{
    public class HistoryFragment : Android.Support.V4.App.Fragment { 
        private RecyclerView mWeightList;
        private List<DateWeight> mItems;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // History Initialization
            View view = inflater.Inflate(Resource.Layout.History, container, false);
            mWeightList = view.FindViewById<RecyclerView>(Resource.Id.weightList);
            mWeightList.HasFixedSize = true;

            mItems = new List<DateWeight>();
            mItems.Add(new DateWeight(DateTime.Today, 180));
            mItems.Add(new DateWeight(DateTime.Today.AddDays(7), 185));
            mItems.Add(new DateWeight(DateTime.Today.AddDays(14), 190));


            mLayoutManager = new LinearLayoutManager(this.Context);
            mWeightList.SetLayoutManager(mLayoutManager);
            //mAdapter = new RecyclerAdapter(mItems);
            mAdapter = new RecyclerAdapter(mItems);
            mWeightList.SetAdapter(mAdapter);

            Console.WriteLine("Done");
            return view;
        }
    }
}