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
using Java.Lang;
using GWG.Resources.fragments;

namespace GWG
{
    [Activity(Label = "MainToolbarActivity", Theme = "@style/MyTheme")]
    public class MainToolbarActivity : ActionBarActivity
    {
        private SupportToolbar mToolbar;
        private MyActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private List<string> mLeftDataSet;
        private ArrayAdapter mLeftAdapter;

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

            // Drawer Initialization
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, new GraphFragment(), "GraphFragment");
            trans.Commit();

            mLeftDataSet = mLeftDataSet = new List<string>();
            mLeftDataSet.Add("Weight Graph");
            mLeftDataSet.Add("Baseline");
            mLeftDataSet.Add("Logout");
            mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
            mLeftDrawer.Adapter = mLeftAdapter;

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