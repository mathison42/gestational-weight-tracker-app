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
        public static bool withinWeightRange(double bmi, double actualGain, DateTime lastDate, DateTime dueDate)
        {
            bool result = false;
            double weeks = 40 - (dueDate - lastDate).TotalDays / 7;
            if (weeks > 40)
            {
                weeks = 40;
            } else if (weeks < 0)
            {
                weeks = 0;
            }

            double expectedGain = getExpectedGain(bmi, weeks);
            double deviation = getWeightDeviation(bmi);

            //Console.WriteLine("weeks: " + weeks);
            //Console.WriteLine("expectedGain: " + expectedGain);
            //Console.WriteLine("Math.Abs(actualGain): " + Math.Abs(actualGain));
            //Console.WriteLine("deviation: " + deviation);

            if (expectedGain-deviation < actualGain && actualGain < expectedGain + deviation)
            {
                result = true;
            }
            return result;
        }

        public static double getExpectedGain(double bmi, double weeks)
        {
            double result = 0;
            double deviation = getWeightDeviation(bmi);
            if (bmi < 18.5)
            {
                result = underweightCalc(weeks);
            }
            else if (bmi < 25)
            {
                result = normalweightCalc(weeks);
            }
            else if (bmi < 30)
            {
                result = overweightCalc(weeks);
            }
            else
            {
                result = obeseCalc(weeks);
            }

            return result;
        }

        // y = -1.052686895·10^-7 x6 + 1.369386823·10^-5 x5 - 6.62842444·10^-4 x4 + 1.389435657·10^-2 x3 - 8.568140272·10^-2 x2 + 1.926622195·10^-1 x - 0.132085101
        public static double underweightCalc(double weeks)
        {
            double result = 0;
            result = result - 1.052686895 * Math.Pow(10, -7) * Math.Pow(weeks, 6);
            result = result + 1.369386823 * Math.Pow(10, -5) * Math.Pow(weeks, 5);
            result = result - 6.62842444  * Math.Pow(10, -4) * Math.Pow(weeks, 4);
            result = result + 1.389435657 * Math.Pow(10, -2) * Math.Pow(weeks, 3);
            result = result - 8.568140272 * Math.Pow(10, -2) * Math.Pow(weeks, 2);
            result = result + 1.926622195 * Math.Pow(10, -1) * weeks;
            result = result - 0.132085101;
            return result;
        }

        // y = -7.813190673·10^-8 x^6 + 1.00370615·10^-5 x^5 - 4.751214689·10^-4 x^4 + 9.446875773·10^-3 x^3 - 4.219931529·10^-2 x^2 + 3.439708576·10^-2 x + 2.018593893·10^-2
        public static double normalweightCalc(double weeks)
        {
            double result = 0;
            result = result - 7.813190673 * Math.Pow(10, -8) * Math.Pow(weeks, 6);
            result = result + 1.00370615  * Math.Pow(10, -5) * Math.Pow(weeks, 5);
            result = result - 4.751214689 * Math.Pow(10, -4) * Math.Pow(weeks, 4);
            //result = result - 5.0 * Math.Pow(10, -4) * Math.Pow(weeks, 4);
            result = result + 9.446875773 * Math.Pow(10, -3) * Math.Pow(weeks, 3);
            result = result - 4.219931529 * Math.Pow(10, -2) * Math.Pow(weeks, 2);
            result = result + 3.439708576 * Math.Pow(10, -2) * weeks;
            result = result + 2.018593893 * Math.Pow(10, -2);
            return result;
        }

        // y = -1.100759936·10^-8 x6 + 9.778200104·10^-7 x5 - 9.453655366·10^-6 x4 - 1.596620154·10^-3 x3 + 6.578722611·10^-2 x2 - 3.584762929·10^-1 x + 3.978835699·10^-1
        public static double overweightCalc(double weeks)
        {
            double result = 0;
            result = result - 1.100759936 * Math.Pow(10, -8) * Math.Pow(weeks, 6);
            result = result + 9.778200104 * Math.Pow(10, -7) * Math.Pow(weeks, 5);
            result = result - 9.453655366 * Math.Pow(10, -6) * Math.Pow(weeks, 4);
            result = result - 1.596620154 * Math.Pow(10, -3) * Math.Pow(weeks, 3);
            result = result + 6.578722611 * Math.Pow(10, -2) * Math.Pow(weeks, 2);
            result = result - 3.584762929 * Math.Pow(10, -1) * weeks;
            result = result + 3.978835699 * Math.Pow(10, -1);
            return result;
        }

        // y = 1.912657401·10^-8 x6 - 3.09056113·10^-6 x5 + 1.997333984·10^-4 x4 - 6.558672715·10^-3 x3 + 0.114309302 x2 - 5.349903678·10^-1 x + 0.567549507
        public static double obeseCalc(double weeks)
        {
            double result = 0;
            result = result + 1.912657401 * Math.Pow(10, -8) * Math.Pow(weeks, 6);
            result = result - 3.09056113  * Math.Pow(10, -6) * Math.Pow(weeks, 5);
            result = result + 1.997333984 * Math.Pow(10, -4) * Math.Pow(weeks, 4);
            result = result - 6.558672715 * Math.Pow(10, -3) * Math.Pow(weeks, 3);
            result = result + 0.114309302 * Math.Pow(weeks, 2);
            result = result - 5.349903678 * Math.Pow(10, -1) * weeks;
            result = result + 0.567549507;
            return result;
        }

    }
}