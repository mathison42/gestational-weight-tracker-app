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

        private List<long> mDates;
        private List<int> mWeights;
        private long mDueDate;
        private double mHeight;
        private double mBMI;

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

            // Retrieve Data
            getData();

            // Set initial graph data
            Bundle args = new Bundle();
            args.PutLongArray("dates", mDates.ToArray());
            args.PutIntArray("weights", mWeights.ToArray());
            args.PutDouble("bmi", mBMI);
            args.PutLong("dueDate", mDueDate);
            mGraphFragment.Arguments = args;

            // Show initial Fragment
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, mGraphFragment, "GraphFragment");
            trans.Commit();

            mCurrentFragment = mGraphFragment;


            // Drawer Initialization
            mLeftDataSet = mLeftDataSet = new List<string>();
            mLeftDataSet.Add("Weight Graph");
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
                // Weight Graph
                Console.WriteLine("Loading weight graph...");

                if (!mGraphFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    args.PutLongArray("dates", mDates.ToArray());
                    args.PutIntArray("weights", mWeights.ToArray());
                    args.PutDouble("bmi", mBMI);
                    args.PutLong("dueDate", mDueDate);
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
                    args.PutLongArray("dates", mDates.ToArray());
                    args.PutIntArray("weights", mWeights.ToArray());
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
                    args.PutInt("weight", mWeights[0]);
                    args.PutDouble("height", mHeight);
                    args.PutDouble("bmi", mBMI);
                    args.PutLong("dueDate", mDueDate);
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

        public void getData()
        {
            // Retrieve data from database... for right now, falsify data
            mDates = new List<long>();
            mDates.Add(DateTime.UtcNow.AddDays(-14).Ticks);
            mDates.Add(DateTime.UtcNow.AddDays(-7).Ticks);
            mDates.Add(DateTime.UtcNow.AddDays(-1).Ticks);
            /**mDates.Add(DateTime.Today.Ticks / TimeSpan.TicksPerMillisecond);
            mDates.Add(DateTime.Today.AddDays(7).Ticks / TimeSpan.TicksPerMillisecond);
            mDates.Add(DateTime.Today.AddDays(14).Ticks / TimeSpan.TicksPerMillisecond);*/

            mWeights = new List<int>();
            mWeights.Add(180);
            mWeights.Add(185);
            mWeights.Add(190);

            mHeight = 76.5;
            mBMI = 25;
            mDueDate = DateTime.UtcNow.AddYears(1).AddMonths(-3).AddDays(7).Ticks;

        }

    }
}