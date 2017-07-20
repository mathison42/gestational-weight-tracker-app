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
        private BaselineFragment mBaselineFragment;
        private Stack<SupportFragment> mStackFragment;

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

            // Show initial Fragment
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, mBaselineFragment, "BaselineFragment");
            trans.Hide(mBaselineFragment);
            trans.Add(Resource.Id.fragmentContainer, mGraphFragment, "GraphFragment");
            trans.Commit();

            mCurrentFragment = mGraphFragment;


            // Drawer Initialization
            mLeftDataSet = mLeftDataSet = new List<string>();
            mLeftDataSet.Add("Weight Graph");
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

                ShowFragment(mGraphFragment);
            }
            else if (id == 1)
            {
                // Baseline
                Console.WriteLine("Loading Baseline...");

                ShowFragment(mBaselineFragment);

            }
            else if (id == 2)
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
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();

            }
            else
            {
                base.OnBackPressed();
            }
        }
        private void ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(mCurrentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            mStackFragment.Push(mCurrentFragment);
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

    }
}