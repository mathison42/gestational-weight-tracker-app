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
    public class HistoryFragment : Android.Support.V4.App.Fragment { 
        private RecyclerView mWeightList;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerAdapter mAdapter;

        private List<DateWeight> mDateWeights;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Bundle bundle = Arguments;
            string dateWeightsStr = bundle.GetString("dateWeights");
            mDateWeights = REDCapResult.parseJson2DateWeightList(dateWeightsStr);
            mDateWeights.Reverse();

            View view = inflater.Inflate(Resource.Layout.History, container, false);
            mWeightList = view.FindViewById<RecyclerView>(Resource.Id.weightList);
            mWeightList.HasFixedSize = true;
            
            mLayoutManager = new LinearLayoutManager(this.Context);
            mWeightList.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerAdapter(mDateWeights);
            mWeightList.SetAdapter(mAdapter);

            Console.WriteLine("Done");
            return view;
        }
    }
}