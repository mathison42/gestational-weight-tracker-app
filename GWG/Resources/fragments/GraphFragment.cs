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
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using static Android.Support.V4.App.FragmentManager;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using OxyPlot.Axes;
using Android.Graphics;

namespace GWG.Resources.fragments
{
    public class GraphFragment : Android.Support.V4.App.Fragment
    {
        private PlotView mPlotViewGraph;
        public PlotModel MyGraph { get; set; }

        private DateTime mDueDate;
        private double mBMI;
        private List<long> mDates;
        private List<int> mWeights;

        private TextView mViewOnTrack;
        private TextView mViewGainGoal;
        private Button mBtnAddWeight;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Retreive Due Date, BMI, dates, and weights
            Bundle bundle = Arguments;
            long[] tempDates = bundle.GetLongArray("dates");
            mDates = tempDates.ToList();
            int[] tempWeights = bundle.GetIntArray("weights");
            mWeights = tempWeights.ToList();
            mBMI = bundle.GetDouble("bmi");
            mDueDate = new DateTime(bundle.GetLong("dueDate"));
     
            // Graph Initialization
            View view = inflater.Inflate(Resource.Layout.Graph, container, false);
            mPlotViewGraph = view.FindViewById<PlotView>(Resource.Id.plotViewGraph);
            mPlotViewGraph.SetScrollContainer(false);
            MyGraph = CreatePlotModel();
            mPlotViewGraph.Model = MyGraph;

            // Set the "On Track" 
            mViewOnTrack = view.FindViewById<TextView>(Resource.Id.viewOnTrack);
            mViewGainGoal = view.FindViewById<TextView>(Resource.Id.viewGainGoal);
            setWeightGainGoalHeader();
            setOnTrackHeader();

            mBtnAddWeight = view.FindViewById<Button>(Resource.Id.btnAddWeight);
            mBtnAddWeight.Click += MBtnAddWeight_Click;
            return view;
        }

        private void setWeightGainGoalHeader()
        {
            List<double> guide = WeightGain.getWeightList(mBMI);
            double max = guide.Max();
            double dev = WeightGain.getWeightDeviation(mBMI);
            mViewGainGoal.Text = "Your weight gain goal is " + (max - dev)  + " - " + (max + dev) + " lbs.";
        }

        private void setOnTrackHeader()
        {
            String resultStr = "";
            String resultColor = "#fbfabe";

            // Get Initial Weight and Date
            long initDate = mDates.Min();
            double initWeight = mWeights[mDates.IndexOf(initDate)];

            // Get Last Weight and Date
            long lastDate = mDates.Max();
            double lastWeight = mWeights[mDates.IndexOf(lastDate)];

            if (WeightGain.withinWeightRange( mBMI, lastWeight-initWeight,  new DateTime(lastDate), mDueDate))
            {
                resultStr = "You are on track!";
                resultColor = "#bae2e0";
            }
            mViewOnTrack.Text = resultStr;
            mViewOnTrack.SetBackgroundColor(Color.ParseColor(resultColor));
            mViewGainGoal.SetBackgroundColor(Color.ParseColor(resultColor));
        }

        private void MBtnAddWeight_Click(object sender, EventArgs e)
        {
            if (mDates.Count == 0)
            {
                Console.WriteLine("[Error] Complete Baseline first.");
                return;
            }
            //Pull up Calendar Dialog
            dialog_weight weightDialog = new dialog_weight();
            weightDialog.Show(this.FragmentManager, "Add Weight Fragment");
            
            weightDialog.mDailyWeightComplete += WeightDialog_mDailyWeightComplete;
        }

        private void WeightDialog_mDailyWeightComplete(object sender, DailyWeightEventArg e)
        {
            long timestamp = e.timestamp;
            double weight = e.weight;

            // Save weight and timestamp with database
            ((MainToolbarActivity)Activity).saveDateAndWeight(timestamp, weight);

            // Get updated lists
            mDates = ((MainToolbarActivity)Activity).getDates();
            mWeights = ((MainToolbarActivity)Activity).getWeights();

            // Update graph
            mPlotViewGraph.Model = CreatePlotModel();

            // Update Headers
            setOnTrackHeader();
        }

        private PlotModel CreatePlotModel()
        {

            var weightSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            var guideSeries = new AreaSeries
            {
                Color = OxyColors.LightGoldenrodYellow,
                Fill = OxyColor.FromRgb(251, 250, 190)

            };

            if (mDates.Count == 0)
            {
                Console.WriteLine("[Info] No dates found.");
            }
            else if (mDates.Count != mWeights.Count)
            {
                Console.WriteLine("[Error] Dates and Weights do not equal in size!");
            }
            else
            {
                // Set main weight line
                for (int i = 0; i < mDates.Count; i++)
                {
                    weightSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(mDates[i])), mWeights[i]));
                }

                // Get Initial Weight and Date
                long initDate = mDates.Min();
                double initWeight = mWeights[mDates.IndexOf(initDate)];

                // Set trendline area
                List<double> guide = WeightGain.getWeightList(mBMI);
                double deviation = WeightGain.getWeightDeviation(mBMI);

                for (int i = 0; i < guide.Count; i++)
                {
                    Console.WriteLine("Guide Weight: " + guide[i]);
                    guideSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(initDate).AddDays(i * 7)), initWeight + guide[i] + deviation));
                    guideSeries.Points2.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(initDate).AddDays(i * 7)), initWeight + guide[i] - deviation));
                }

            }

            var plotModel = new PlotModel { Title = "" };


            // Get the min and max weights for the initial bounds
            int minWeight = 0;
            int maxWeight = 50;
            if (mWeights.Count > 0)
            {
                minWeight = (int)mWeights.Min();
                maxWeight = (int)mWeights.Max();
            }

            DateTime startDate;
            DateTime endDate;
            if (mDates.Count > 0)
            {
                startDate = new DateTime(mDates.Min()).AddDays(-1);
                endDate = new DateTime(mDates.Max()).AddDays(1);

            } else
            {
                startDate = DateTime.UtcNow.AddDays(-5);
                endDate = DateTime.UtcNow.AddDays(5);

            }

            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = DateTimeAxis.ToDouble(startDate), Maximum = DateTimeAxis.ToDouble(endDate), StringFormat = "M/d",
                AbsoluteMinimum = DateTimeAxis.ToDouble(startDate),
                AbsoluteMaximum = DateTimeAxis.ToDouble(mDueDate)
            });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minWeight-3, Maximum = maxWeight+3,
                AbsoluteMinimum = minWeight - 10,
                AbsoluteMaximum = maxWeight + 10
            });

            plotModel.Series.Add(guideSeries);
            plotModel.Series.Add(weightSeries);

            return plotModel;
        }
    }
}