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
        private double mPreWeight;

        private Button mBtnAddWeight;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Retreive Due Date and BMI
            mDueDate = DateTime.Today.AddMonths(9);
            mBMI = 25.0;
            mPreWeight = 130;
     
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

            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(mDueDate.AddYears(-1).AddMonths(3).AddDays(-7)), mPreWeight));
            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(mDueDate.AddYears(-1).AddMonths(3).AddDays(-7).AddDays(7)), mPreWeight + 3));
            series1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(mDueDate.AddYears(-1).AddMonths(3).AddDays(-7).AddDays(14)), mPreWeight + 3));

            var plotModel = new PlotModel { Title = "" };

            DateTime startDate = mDueDate.AddYears(-1).AddMonths(3).AddDays(-7); // Reverse Naegele's Rule
            DateTime endDate = DateTime.Today;

            // Will later need to calculate based on new data
            int startWeight = (int)mPreWeight - 5;
            int endWeight = (int)mPreWeight + 30;

            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = DateTimeAxis.ToDouble(startDate), Maximum = DateTimeAxis.ToDouble(endDate), StringFormat = "M/d",
                AbsoluteMinimum = DateTimeAxis.ToDouble(startDate), AbsoluteMaximum = DateTimeAxis.ToDouble(endDate)
            });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = startWeight, Maximum = endWeight,
                AbsoluteMinimum = startWeight - 10,
                AbsoluteMaximum = endWeight
            });

            plotModel.Series.Add(series1);

            return plotModel;
        }
    }
}