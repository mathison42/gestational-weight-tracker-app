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

namespace GWG
{
    class WeightGain
    {
        public readonly static int TOTAL_WEEKS = 40;
        // BMI <18.5
        public readonly static double UNDERWEIGHT_DEVIATION = 6;
        public readonly static List<double> UNDERWEIGHT_WEIGHTS
            = new List<double> {
                0,
                0,
                0,
                0,
                0,
                0.05,
                0.5,
                0.95,
                1.4,
                1.85,
                2.3,
                2.75,
                3.866,
                4.982,
                6.098,
                7.214,
                8.33,
                9.446,
                10.562,
                11.678,
                12.794,
                13.91,
                15.026,
                16.142,
                17.258,
                18.374,
                19.49,
                20.606,
                21.722,
                22.838,
                23.954,
                25.07,
                26.186,
                27.302,
                28.418,
                29.534,
                30.65,
                31.766,
                32.882,
                34
            };

        // BMI 18.5-24.9
        public readonly static double NORMAL_DEVIATION = 5;
        public readonly static List<double> NORMAL_WEIGHTS
            = new List<double> {
                0,
                0,
                0,
                0,
                0,
                0.05,
                0.5,
                0.95,
                1.4,
                1.85,
                2.3,
                2.75,
                3.723,
                4.696,
                5.669,
                6.642,
                7.615,
                8.588,
                9.561,
                10.534,
                11.507,
                12.48,
                13.453,
                14.426,
                15.399,
                16.372,
                17.345,
                18.318,
                19.291,
                20.264,
                21.237,
                22.21,
                23.183,
                24.156,
                25.129,
                26.102,
                27.075,
                28.048,
                29.021,
                30
            };

        // BMI 25-29.9
        public readonly static double OVERWEIGHT_DEVIATION = 5;
        public readonly static List<double> OVERWEIGHT_WEIGHTS
            = new List<double> {
                0,
                0,
                0,
                0,
                0,
                0.05,
                0.5,
                0.95,
                1.4,
                1.85,
                2.3,
                2.75,
                3.366,
                3.982,
                4.598,
                5.214,
                5.83,
                6.446,
                7.062,
                7.678,
                8.294,
                8.91,
                9.526,
                10.142,
                10.758,
                11.374,
                11.99,
                12.606,
                13.222,
                13.838,
                14.454,
                15.07,
                15.686,
                16.302,
                16.918,
                17.534,
                18.15,
                18.766,
                19.382,
                20
            };

        // BMI 30+
        public readonly static double OBESE_DEVIATION = 4.5;
        public readonly static List<double> OBESE_WEIGHTS
            = new List<double> {
                0,
                0,
                0,
                0,
                0,
                0.05,
                0.5,
                0.95,
                1.4,
                1.85,
                2.3,
                2.75,
                3.2054,
                3.6608,
                4.1162,
                4.5716,
                5.027,
                5.4824,
                5.9378,
                6.3932,
                6.8486,
                7.304,
                7.7594,
                8.2148,
                8.6702,
                9.1256,
                9.581,
                10.0364,
                10.4918,
                10.9472,
                11.4026,
                11.858,
                12.3134,
                12.7688,
                13.2242,
                13.6796,
                14.135,
                14.5904,
                15.0458,
                15.5
            };

        // Calculate which graph to retreive based on bmi
        // https://www.nhlbi.nih.gov/health/educational/lose_wt/BMI/bmicalc.htm
        public static List<double> getWeightList(double bmi)
        {
            List<double> result = new List<double>();

            if (bmi < 18.5)
            {
                result = UNDERWEIGHT_WEIGHTS;
            }
            else if (bmi < 25)
            {
                result = NORMAL_WEIGHTS;
            }
            else if (bmi < 30)
            {
                result = OVERWEIGHT_WEIGHTS;
            }
            else
            {
                result = OBESE_WEIGHTS;
            }
            return result;
        }

        // Calculate how much deviation is allowed per weight class
        // https://www.nhlbi.nih.gov/health/educational/lose_wt/BMI/bmicalc.htm
        public static double getWeightDeviation(double bmi)
        {
            double result = 0;

            if (bmi < 18.5)
            {
                result = UNDERWEIGHT_DEVIATION;
            }
            else if (bmi < 25)
            {
                result = NORMAL_DEVIATION;
            }
            else if (bmi < 30)
            {
                result = OVERWEIGHT_DEVIATION;
            }
            else
            {
                result = OBESE_DEVIATION;
            }
            return result;
        }

        // Determine if user is within weight range
        public static bool withinWeightRange(double bmi, double weightGained,
            DateTime lastDate, DateTime dueDate)
        {
            bool result = false;

            // Calculate index to retrieve based on weeks until pregnancy
            int weeksTilDueDate = (int)(dueDate - lastDate).TotalDays / 7;
            int index = TOTAL_WEEKS - weeksTilDueDate;
            if (index < 0)
            {
                index = 0;
            }
            else  if (index > 39)
            {
                index = 39;
            }

            // Grab how much weight they should have gained
            double idealWeightGain = 0;
            if (bmi < 18.5)
            {
                idealWeightGain = UNDERWEIGHT_WEIGHTS[index];
            }
            else if (bmi < 25)
            {
                idealWeightGain = NORMAL_WEIGHTS[index];
            }
            else if (bmi < 30)
            {
                idealWeightGain = OVERWEIGHT_WEIGHTS[index];
            }
            else
            {
                idealWeightGain = OBESE_WEIGHTS[index];
            }

            // Calculate if out of range
            double dev = getWeightDeviation(bmi);
            if (idealWeightGain + dev > weightGained && idealWeightGain - dev < weightGained)
            {
                result = true;
            }

            return result;
        }

    }
}