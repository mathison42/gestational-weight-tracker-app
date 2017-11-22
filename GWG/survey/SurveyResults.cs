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

namespace GWG.survey
{
    class SurveyResults
    {
        public String q1 { get; set; } // 1. Have you been weighed during this pregnancy?
        public String q2 { get; set; } // 2. At any time during this pregnancy has a healthcare provider talked to you about how much weight you should gain during pregnancy?
        public String q3 { get; set; } // 3. Have you heard of the term BMI?
        public String q4 { get; set; } // 4. What does BMI stand for?
        public String q5 { get; set; } // 5. What would you consider your weight just before this pregnancy?
        public String q6 { get; set; } // 6. How much weight do you think you should ideally gain during this pregnancy?
        public String q7 { get; set; } // 7. How much weight do you think you will gain during this pregnancy?

        public SurveyResults()
        {
            q1 = "";
            q2 = "";
            q3 = "";
            q4 = "";
            q5 = "";
            q6 = "";
            q7 = "";
        }

        public bool surveyCompleted()
        {
            bool result = false;
            if (!String.IsNullOrWhiteSpace(q1) && !String.IsNullOrWhiteSpace(q2) && !String.IsNullOrWhiteSpace(q3) && !String.IsNullOrWhiteSpace(q5))
            {
                result = true;
            }
            return result;
        }

        public void toString()
        {
            Console.WriteLine("Q1. " + q1);
            Console.WriteLine("Q2. " + q2);
            Console.WriteLine("Q3. " + q3);
            Console.WriteLine("Q4. " + q4);
            Console.WriteLine("Q5. " + q5);
            Console.WriteLine("Q6. " + q6);
            Console.WriteLine("Q7. " + q7);
        }
    }
}