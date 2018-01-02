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
using Newtonsoft.Json;
using GWG.Resources.redcap;
using Android.Content.PM;
using GWG.survey;

namespace GWG
{
    [Activity(Label = "MainToolbarActivity", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainToolbarActivity : AppCompatActivity, AdapterView.IOnItemClickListener
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
        private InformationFragment mInformationFragment;
        private BaselineFragment mBaselineFragment;
        private SurveyIntroFragment mSurveyIntroFragment;
        private Stack<SupportFragment> mStackFragment;

        private REDCapResult mRecord;
        private REDCapHelper mRCH;
        private long mDueDate;
        private double mHeight;
        private double mBMI;

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
            mInformationFragment = new InformationFragment();
            mBaselineFragment = new BaselineFragment();
            mSurveyIntroFragment = new SurveyIntroFragment();

            mStackFragment = new Stack<SupportFragment>();

            SetSupportActionBar(mToolbar);

            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                       // Host Activity
                mDrawerLayout,              // DrawerLayout
                Resource.String.openDrawer, // Opened Message
                Resource.String.closeDrawer // Closed Message
            );

            mDrawerLayout.AddDrawerListener(mDrawerToggle);
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

            // Retrieve Actual Data
            mRecord = JsonConvert.DeserializeObject<REDCapResult>(Intent.GetStringExtra("record"));
            mRecord.parseJson2DateWeightList();

            // Set REDCapHelper
            mRCH = new REDCapHelper(mRecord.redcapid, mRecord.record_id);

            // Drawer Initialization
            mLeftDataSet = mLeftDataSet = new List<string>();
            if (mRecord.showSurvey())
            {
                mLeftDataSet.Add("Survey");
            } else
            {
                mLeftDataSet.Add("Resources"); // Information
                if (mRecord.isExperimental())
                {
                    mLeftDataSet.Add("Tracker");
                    mLeftDataSet.Add("History");
                }
                mLeftDataSet.Add("Baseline");
            }
            mLeftDataSet.Add("Logout");
            mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
            mLeftDrawer.Adapter = mLeftAdapter;
            mLeftDrawer.OnItemClickListener = this;

            var trans = SupportFragmentManager.BeginTransaction();
            if (mRecord.showSurvey())
            {
                Bundle args = new Bundle();
                args.PutString("record", Intent.GetStringExtra("record"));
                mSurveyIntroFragment.Arguments = args;

                // Show initial Fragment
                trans.Add(Resource.Id.fragmentContainer, mSurveyIntroFragment, "SurveyIntroFragment");

                mCurrentFragment = mSurveyIntroFragment;

            } else
            {
                if (mRecord.isExperimental())
                {
                    // Set initial graph data
                    Bundle args = new Bundle();
                    args.PutDouble("bmi", mRecord.getBMI());
                    args.PutLong("dueDate", mRecord.getDueDate());
                    args.PutString("dateWeights", REDCapResult.parseDateWeightList2Json(mRecord.dateWeights));
                    mGraphFragment.Arguments = args;

                    // Show initial Fragment
                    trans.Add(Resource.Id.fragmentContainer, mGraphFragment, "GraphFragment");

                    mCurrentFragment = mGraphFragment;
                }
                else
                {
                    Bundle args = new Bundle();
                    mInformationFragment.Arguments = args;

                    // Show initial Fragment
                    trans.Add(Resource.Id.fragmentContainer, mInformationFragment, "InformationFragment");

                    mCurrentFragment = mInformationFragment;
                }
            }
            trans.Commit();

            // If mBMI is empty, show disclaimer
            if (mRecord.getBMI() <= 0)
            {
                //Pull up Disclaimer Dialog
                dialog_disclaimer disclaimerDialog = new dialog_disclaimer();
                disclaimerDialog.Show(SupportFragmentManager, "Show App Disclaimer");
            }

        }

        public void OnItemClick(AdapterView container, View view, int position, long id)
        {
            string name = mLeftAdapter.GetItem(position).ToString();
            
            if (name == "Tracker")
            {
                // Tracker
                Console.WriteLine("Loading Tracker...");

                if (!mGraphFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    args.PutDouble("bmi", mRecord.getBMI());
                    args.PutLong("dueDate", mRecord.getDueDate());
                    args.PutString("dateWeights", REDCapResult.parseDateWeightList2Json(mRecord.dateWeights));
                    mGraphFragment.Arguments = args;
                }

                ReplaceFragment(mGraphFragment);
            }
            else if (name == "History")
            {
                // History
                Console.WriteLine("Loading History...");

                if (!mHistoryFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    args.PutString("dateWeights", REDCapResult.parseDateWeightList2Json(mRecord.dateWeights));
                    mHistoryFragment.Arguments = args;
                }
                ReplaceFragment(mHistoryFragment);
            }
            else if (name == "Resources")
            {
                // History
                Console.WriteLine("Loading Information and Resources...");

                if (!mInformationFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    mInformationFragment.Arguments = args;
                }
                ReplaceFragment(mInformationFragment);
            }
            else if (name == "Baseline")
            {
                // Baseline
                Console.WriteLine("Loading Baseline...");

                if (!mBaselineFragment.IsVisible)
                {
                    Bundle args = new Bundle();
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
            else if (name == "Survey")
            {
                // Load Survey
                Console.WriteLine("Loading survey...");

                if (!mSurveyIntroFragment.IsVisible)
                {
                    Bundle args = new Bundle();
                    args.PutString("record", Intent.GetStringExtra("record"));
                    mSurveyIntroFragment.Arguments = args;
                }
                ReplaceFragment(mSurveyIntroFragment);
            }
            else if (name == "Logout")
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

        public async void saveBaseline(DateTime dueDate, double weight, double height, double BMI)
        {
            mDueDate = dueDate.Ticks;
            mRecord.setDueDate(dueDate.Ticks.ToString());
            if (mRecord.dateWeights.Count == 0)
            {
                mBMI = BMI;
                mHeight = height;
                saveDateAndWeight(dueDate.AddDays(-280).Ticks, weight);

                // Update Database with Height, BMI, and Due Date
                mRecord.setBMI(BMI.ToString("0.0"));
                mRecord.setHeight(height.ToString("0.0"));
                await mRCH.SaveInitBaseline(height, BMI, dueDate.Ticks);
            }
            else
            {
                // Update Database with new Due Date
                await mRCH.SaveDueDate(dueDate.Ticks);
            }
        }

        public async void saveDateAndWeight(long timestamp, double weight)
        {
            if (mRecord.dateWeights.Count > 0)
            {
                DateTime newDate = new DateTime(timestamp);
                DateTime today = new DateTime(DateTime.Today.Ticks);
                DateTime minDate = new DateTime(mRecord.minDate().mDate);
                // No dates prior to inital date
                if (newDate.Date <= minDate.Date) {
                    Console.WriteLine("[User Error] Unable to add a date prior to the init date.");
                }
                // No dates after today
                else if (today.Date < newDate.Date)
                {
                    Console.WriteLine("[User Error] Unable to add a date after today's date.");
                }
                // Add if newDate doesn't exist
                else if (mRecord.getDateWeight(timestamp) == null)
                {
                    mRecord.addDateWeight(new DateWeight(timestamp, weight));
                }
                // Update if newDate is today
                else if (newDate.Date == today.Date)
                {
                    mRecord.setDateWeight(timestamp, weight);
                }
                // Nothing if value already exists at this date
                else
                {
                    Console.WriteLine("[User Error] Unable to add a date that already exists.");
                }
            }
            else
            {
                mRecord.addDateWeight(new DateWeight(timestamp, weight));
            }

            // Update Database with Weights and Dates
            await mRCH.SaveDateWeights(mRecord.parseDateWeightList2Json(), mRecord.parseDateWeightList2JsonShort());
        }
    }
}