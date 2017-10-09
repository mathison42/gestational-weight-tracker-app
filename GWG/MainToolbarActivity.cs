﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using OxyPlot.Axes;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Java.Lang;
using GWG.Resources.fragments;
using Newtonsoft.Json;
using GWG.Resources.redcap;

namespace GWG
{
    [Activity(Label = "MainToolbarActivity", Theme = "@style/MyTheme")]
    public class MainToolbarActivity : ActionBarActivity, AdapterView.IOnItemClickListener
    {
        private SupportToolbar mToolbar;
        private MyActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private List<string> mLeftDataSet;
        private ArrayAdapter mLeftAdapter;
        private SupportFragment mCurrentFragment;
        private GraphFragment mGraphFragment;
        private HistoryFragment mHistoryFragment;
        private BaselineFragment mBaselineFragment;
        private Stack<SupportFragment> mStackFragment;

        private REDCapResult mRecord;
        private List<long> mDates = new List<long>();
        private List<int> mWeights = new List<int>();
        private long mDueDate;
        private double mHeight;
        private double mBMI;

        public List<long> getDates()
        {
            return mDates;
        }

        public List<int> getWeights()
        {
            return mWeights;
        }

        public List<DateWeight> getDateWeights()
        {
            return mRecord.dateWeights;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MainToolbar);

            // Toolbar Initialization
            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

            mGraphFragment = new GraphFragment();
            mHistoryFragment = new HistoryFragment();
            mBaselineFragment = new BaselineFragment();

            mStackFragment = new Stack<SupportFragment>();

            SetSupportActionBar(mToolbar);

            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                       // Host Activity
                mDrawerLayout,              // DrawerLayout
                Resource.String.openDrawer, // Opened Message
                Resource.String.closeDrawer // Closed Message
            );

            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            if (savedInstanceState != null)
            {
                if (savedInstanceState.GetString("DrawerState") == "Opened")
                {
                    SupportActionBar.SetTitle(Resource.String.openDrawer);
                }
                else
                {
                    SupportActionBar.SetTitle(Resource.String.closeDrawer);

                }
            }
            else
            {
                // This the first time the activity is ran
                SupportActionBar.SetTitle(Resource.String.closeDrawer);
            }

            // Retrieve Test Data
            createTestData();

            // Retrieve Actual Data
            mRecord = JsonConvert.DeserializeObject<REDCapResult>(Intent.GetStringExtra("record"));
            mRecord.parseDatesAndWeights();
            mRecord.printRecord();
            mRecord.parseJson2DateWeightList();
            mRecord.printRecord();

            // Set initial graph data
            Bundle args = new Bundle();
            args.PutLongArray("dates", mDates.ToArray());
            args.PutIntArray("weights", mWeights.ToArray());
            args.PutDouble("bmi", mRecord.getBMI());
            args.PutLong("dueDate", mRecord.getDueDate());
            args.PutString("dateWeights", REDCapResult.parseDateWeightList2Json(mRecord.dateWeights));
            mGraphFragment.Arguments = args;

            // Show initial Fragment
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, mGraphFragment, "GraphFragment");
            trans.Commit();

            mCurrentFragment = mGraphFragment;
            
            // Drawer Initialization
            mLeftDataSet = mLeftDataSet = new List<string>();
            mLeftDataSet.Add("Tracker");
            mLeftDataSet.Add("History");
            mLeftDataSet.Add("Baseline");
            mLeftDataSet.Add("Logout");
            mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
            mLeftDrawer.Adapter = mLeftAdapter;
            mLeftDrawer.OnItemClickListener = this;

        }

        public void OnItemClick(AdapterView container, View view, int position, long id)
        {
            if (id == 0)
            {
                // Tracker
                Console.WriteLine("Loading Tracker...");

                if (!mGraphFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    args.PutLongArray("dates", mDates.ToArray());
                    args.PutIntArray("weights", mWeights.ToArray());
                    args.PutDouble("bmi", mRecord.getBMI());
                    args.PutLong("dueDate", mRecord.getDueDate());
                    args.PutString("dateWeights", REDCapResult.parseDateWeightList2Json(mRecord.dateWeights));
                    mGraphFragment.Arguments = args;
                }

                ReplaceFragment(mGraphFragment);
            }
            else if (id == 1)
            {
                // History
                Console.WriteLine("Loading History...");

                if (!mHistoryFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    //args.PutLongArray("dates", mDates.ToArray());
                    //args.PutIntArray("weights", mWeights.ToArray());
                    args.PutString("dateWeights", REDCapResult.parseDateWeightList2Json(mRecord.dateWeights));
                    mHistoryFragment.Arguments = args;
                }
                ReplaceFragment(mHistoryFragment);
                

            }
            else if (id == 2)
            {
                // Baseline
                Console.WriteLine("Loading Baseline...");
                
                if (!mBaselineFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    /**if (mDates.Count > 0)
                    {
                        int index = mDates.IndexOf(mDates.Min());
                        args.PutInt("weight", mWeights[index]);
                    }*/
                    if (mRecord.dateWeights.Count > 0)
                    {
                        args.PutDouble("weight", mRecord.minDate().mWeight);
                    }
                    args.PutDouble("height", mRecord.getHeight());
                    args.PutDouble("bmi", mRecord.getBMI());
                    args.PutLong("dueDate", mRecord.getDueDate());
                    mBaselineFragment.Arguments = args;
                }
                ReplaceFragment(mBaselineFragment);
            }
            else if (id == 3)
            {
                // Logout
                Console.WriteLine("Logging out...");
                Finish();

            }
            else
            {
                Console.WriteLine("Failed Loading: " + id);
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public void ReplaceFragment(SupportFragment fragment)
        {
            if (fragment.IsVisible)
            {
                return;
            }

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Replace(Resource.Id.fragmentContainer, fragment);
            trans.Commit();

            mCurrentFragment = fragment;
            mDrawerLayout.CloseDrawer((int)GravityFlags.Left);

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            mDrawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
            {
                outState.PutString("DrawerState", "Opened");
                mCurrentFragment = mStackFragment.Pop();
            }
            else
            {
                outState.PutString("DrawerState", "Closed");
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public void saveBaseline(DateTime dueDate, double weight, double height, double BMI)
        {
            mDueDate = dueDate.Ticks;

            //if (mDates.Count == 0)
            if (mRecord.dateWeights.Count == 0)
            {
                mBMI = BMI;
                mHeight = height;
                saveDateAndWeight(DateTime.Today.Ticks, weight);

                // Update Database with BMI and Height
            }

            // Update Database with Due Date

        }

        public void saveDateAndWeight(long timestamp, double weight)
        {
            /*long maxTimestamp = 0;
            if (mDates.Count> 0)
            {
                maxTimestamp = mDates.Max();
            }
            if (new DateTime(timestamp).Date == new DateTime(maxTimestamp).Date)
            {
                int index = mDates.IndexOf(maxTimestamp);
                mDates[index] = timestamp;
                mWeights[index] = (int)weight;
            }
            else
            {
                mDates.Add(timestamp);
                mWeights.Add((int)weight);
            }*/

            if (mRecord.dateWeights.Count > 0)
            {
                DateWeight maxDWDate = mRecord.maxDate();
                Console.WriteLine("new DateTime(timestamp).Date: " + new DateTime(timestamp).Date);
                Console.WriteLine("new DateTime(maxDWDate.mDate).Date: " + new DateTime(maxDWDate.mDate).Date);
                Console.WriteLine("new DateTime(DateTime.Today.Ticks).Date: " + new DateTime(DateTime.Today.Ticks).Date);
                if (new DateTime(timestamp).Date == new DateTime(DateTime.Today.Ticks).Date && 
                    new DateTime(timestamp).Date == new DateTime(maxDWDate.mDate).Date)
                {
                    //int index = mRecord.dateWeights.IndexOf(maxDWDate);
                    //mRecord.dateWeights[index].mWeight = weight;
                    mRecord.setDateWeight(maxDWDate.mDate, weight);
                }
                else if (mRecord.getDateWeight(timestamp) == null)
                {
                    mRecord.addDateWeight(new DateWeight(timestamp, weight));
                } 
                else
                {
                    // value already exists at this date
                }
            }
            else
            {
                /** mDates.Add(timestamp);
                 mWeights.Add((int)weight);*/
                mRecord.dateWeights.Add(new DateWeight(timestamp, weight));
            }

            // Update Database with Weights and Dates
        }

        public void createTestData()
        {
            // Retrieve data from database... for right now, falsify data
            mDates = new List<long>();
            mDates.Add(DateTime.Today.AddDays(-14).Ticks);
            mDates.Add(DateTime.Today.AddDays(-7).Ticks);
            mDates.Add(DateTime.Today.AddDays(-1).Ticks);
            /**mDates.Add(DateTime.Today.Ticks / TimeSpan.TicksPerMillisecond);
            mDates.Add(DateTime.Today.AddDays(7).Ticks / TimeSpan.TicksPerMillisecond);
            mDates.Add(DateTime.Today.AddDays(14).Ticks / TimeSpan.TicksPerMillisecond);
            Console.WriteLine("TIMES: " + mDates[0].ToString());
            Console.WriteLine("TIMES: " + mDates[1].ToString());
            Console.WriteLine("TIMES: " + mDates[2].ToString());*/
            mWeights = new List<int>();
            mWeights.Add(205);
            mWeights.Add(206);
            mWeights.Add(209);

            mHeight = 76.5;
            mBMI = 25;
            //mDueDate = DateTime.Today.AddYears(1).AddMonths(-3).AddDays(7).Ticks;

        }

    }
}