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
using GWG.Resources.redcap;

namespace GWG.Resources.fragments
{
    public class GraphFragment : Android.Support.V4.App.Fragment
    {
        private PlotView mPlotViewGraph;
        public PlotModel MyGraph { get; set; }

        private DateTime mDueDate;
        private double mBMI;
        private List<DateWeight> mDateWeights;

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
            mBMI = bundle.GetDouble("bmi");
            mDueDate = new DateTime(bundle.GetLong("dueDate"));
            String dateWeightsStr = bundle.GetString("dateWeights");
            mDateWeights = REDCapResult.parseJson2DateWeightList(dateWeightsStr);

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
            String resultColor = "#fbfabe";

            // Get Initial Weight and Date
            DateWeight init = REDCapResult.minDate(mDateWeights);

            // Get Last Weight and Date
            DateWeight last = REDCapResult.maxDate(mDateWeights);


            
            if (WeightGain.withinWeightRange( mBMI, last.mWeight - init.mWeight,  new DateTime(last.mDate), mDueDate))
            {
                resultColor = "#bae2e0";
                mViewOnTrack.Visibility = Android.Views.ViewStates.Visible;
                mViewOnTrack.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 5);
            } 
            else
            {
                resultColor = "#fbfabe";
                mViewOnTrack.Visibility = Android.Views.ViewStates.Invisible;
                mViewOnTrack.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 0);
            }
            mViewGainGoal.SetBackgroundColor(Color.ParseColor(resultColor));
        }

        private void MBtnAddWeight_Click(object sender, EventArgs e)
        {
            if (mDueDate.Ticks == 0)
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
            mDateWeights = ((MainToolbarActivity)Activity).getDateWeights();

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
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColor.FromRgb(105, 151, 145),
                Color = OxyColor.FromRgb(105, 151, 145)
            };

            var guideSeries = new AreaSeries
            {
                Color = OxyColor.FromRgb(186, 226, 224),
                Fill = OxyColor.FromRgb(186, 226, 224)

            };
            
            if (mDateWeights.Count == 0)
            {
                Console.WriteLine("[Info] No dates found.");
            }
            else
            {
                // Set main weight line
                for (int i = 0; i < mDateWeights.Count; i++)
                {
                    weightSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(mDateWeights[i].mDate)), mDateWeights[i].mWeight));
                }

                Console.WriteLine("mDueDate!!!!: " + mDueDate);
                if (mDueDate.Ticks > 0)
                {
                    int TOTAL_WEEKS = 40;
                    // Set trendline area
                    List<double> guide = WeightGain.getWeightList(mBMI);
                    double deviation = WeightGain.getWeightDeviation(mBMI);
                    DateWeight init = REDCapResult.minDate(mDateWeights);
                    long conceptionDate = mDueDate.AddDays(-7*TOTAL_WEEKS).Ticks;
                    for (double week = 0; week < TOTAL_WEEKS; week = week + 0.1)
                    {
                        // Console.WriteLine("Guide Weight: " + guide[i]);
                        double expectedGain = WeightGain.getExpectedGain(mBMI, week);
                        guideSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(conceptionDate).AddDays(week * 7)), init.mWeight + expectedGain + deviation));
                        guideSeries.Points2.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(conceptionDate).AddDays(week * 7)), init.mWeight + expectedGain - deviation));
                    }

                }
            }

            var plotModel = new PlotModel { Title = "" };


            // Get the min and max weights for the initial bounds
            double minWeight = 0;
            double maxWeight = 50;
            if (mDateWeights.Count > 0)
            {
                minWeight = REDCapResult.minWeight(mDateWeights).mWeight;
                maxWeight = REDCapResult.maxWeight(mDateWeights).mWeight;
            }

            DateTime startDate;
            DateTime endDate;
            if (mDateWeights.Count > 0)
            {
                startDate = new DateTime(REDCapResult.minDate(mDateWeights).mDate).AddDays(-1);
                endDate = new DateTime(REDCapResult.maxDate(mDateWeights).mDate).AddDays(1);

            }
            else
            {
                startDate = DateTime.Today.AddDays(-5);
                endDate = DateTime.Today.AddDays(5);

            }

            DateTime absoluteEndDate = endDate;
            if (mDueDate.Ticks > 0)
            {
                absoluteEndDate = mDueDate;
            }
            Console.WriteLine("startDate: " + startDate);
            Console.WriteLine("endDate: " + endDate);
            Console.WriteLine("absoluteEndDate: " + absoluteEndDate);
            Console.WriteLine("minWeight: " + minWeight);
            Console.WriteLine("maxWeight: " + maxWeight);

            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = DateTimeAxis.ToDouble(startDate), Maximum = DateTimeAxis.ToDouble(endDate), StringFormat = "M/d",
                AbsoluteMinimum = DateTimeAxis.ToDouble(startDate),
                AbsoluteMaximum = DateTimeAxis.ToDouble(absoluteEndDate),
                Title = "Date"
            });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minWeight-3, Maximum = maxWeight+3,
                AbsoluteMinimum = minWeight - 10,
                AbsoluteMaximum = maxWeight + 10,
                Title = "Weight ( lbs )"
            });

            plotModel.Series.Add(guideSeries);
            plotModel.Series.Add(weightSeries);

            return plotModel;
        }
    }
}