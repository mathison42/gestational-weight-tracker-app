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

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using OxyPlot.Axes;

namespace GWG.Resources.fragments
{
    public class GraphFragment : Android.Support.V4.App.Fragment
    {
        private PlotView plotViewGraph;
        public PlotModel MyGraph { get; set; }

        private DateTime mDueDate;
        private double mBMI;
        private long[] mDates;
        private int[] mWeights;

        private Button mBtnAddWeight;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Retreive Due Date, BMI, dates, and weights
            Bundle bundle = Arguments;
            mDates = bundle.GetLongArray("dates");
            mWeights = bundle.GetIntArray("weights");
            mBMI = bundle.GetDouble("bmi");
            mDueDate = new DateTime(bundle.GetLong("dueDate"));
     
            // Graph Initialization
            View view = inflater.Inflate(Resource.Layout.Graph, container, false);
            plotViewGraph = view.FindViewById<PlotView>(Resource.Id.plotViewGraph);
            plotViewGraph.SetScrollContainer(false);
            MyGraph = CreatePlotModel();
            plotViewGraph.Model = MyGraph;

            mBtnAddWeight = view.FindViewById<Button>(Resource.Id.btnAddWeight);
            mBtnAddWeight.Click += MBtnAddWeight_Click;
            return view;
        }

        private void MBtnAddWeight_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private PlotModel CreatePlotModel()
        {

            var series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            if (mDates.Length != mWeights.Length)
            {
                Console.WriteLine("[Error] Dates and Weights do not equal in size!");
            }
            else
            {
                for (int i = 0; i < mDates.Length; i++)
                {
                    series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(mDates[i])), mWeights[i]));
                }
            }
            /**series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(mDueDate.AddYears(-1).AddMonths(3).AddDays(-7)), mPreWeight));
            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(mDueDate.AddYears(-1).AddMonths(3).AddDays(-7).AddDays(7)), mPreWeight + 3));
            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(mDueDate.AddYears(-1).AddMonths(3).AddDays(-7).AddDays(14)), mPreWeight + 3));*/

            var plotModel = new PlotModel { Title = "" };

            DateTime startDate = new DateTime(mDates[0]).AddDays(-1);
            DateTime endDate = new DateTime(mDates[mDates.Length-1]).AddDays(1);

            // Get the min and max weights for the initial bounds
            int minWeight = (int)mWeights.Min();
            int maxWeight = (int)mWeights.Max();

            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = DateTimeAxis.ToDouble(startDate), Maximum = DateTimeAxis.ToDouble(endDate), StringFormat = "M/d",
                AbsoluteMinimum = DateTimeAxis.ToDouble(startDate), AbsoluteMaximum = DateTimeAxis.ToDouble(endDate)
            });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minWeight-3, Maximum = maxWeight+3,
                AbsoluteMinimum = minWeight - 10,
                AbsoluteMaximum = maxWeight + 10
            });

            plotModel.Series.Add(series1);

            return plotModel;
        }
    }
}