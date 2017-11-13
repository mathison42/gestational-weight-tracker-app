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
using System.Data;
using Newtonsoft.Json;

namespace GWG.Resources.redcap
{
    public class REDCapResult
    {
        public string record_id { get; set; }
        public string redcapid { get; set; }
        public string experimental { get; set; }
        public string height { get; set; }
        public string bmi { get; set; }
        public string json { get; set; }
        public string duedate { get; set; }
        public string completed_survey { get; set; }

        private double bmiDouble = -1;
        private double heightDouble = -1;
        private long dueDateLong = -1;
        
        public List<DateWeight> dateWeights = new List<DateWeight>();

        public double getBMI()
        {
            if (bmiDouble == -1)
            {
                if (!Double.TryParse(bmi, out bmiDouble))
                {
                    Console.WriteLine("[Error] GET: BMI is not a double value.");
                }
            }
            return bmiDouble;
        }

        public void setBMI(string bmi)
        {
            if (!Double.TryParse(bmi, out bmiDouble))
            {
                Console.WriteLine("[Error] SET: BMI is not a double value.");
            }
            else
            {
                this.bmi = bmi;
            }

        }

        public double getHeight()
        {
            if (heightDouble == -1)
            {
                if (!Double.TryParse(height, out heightDouble))
                {
                    Console.WriteLine("[Error] GET: Height is not a double value.");
                }
            }
            return heightDouble;
        }

        public void setHeight(string height)
        {
            if (!Double.TryParse(height, out heightDouble))
            {
                Console.WriteLine("[Error] SET: Height is not a double value.");
            }
            else
            {
                this.height = height;
            }

        }

        public long getDueDate()
        {
            if (dueDateLong == -1)
            {
                if (!long.TryParse(duedate, out dueDateLong))
                {
                    Console.WriteLine("[Error] GET: Due Date is not an long value.");
                }
            }
            return dueDateLong;
        }

        public void setDueDate(string duedate)
        {
            if (!long.TryParse(duedate, out dueDateLong))
            {
                Console.WriteLine("[Error] SET: Due Date is not an long value.");
            }
            else
            {
                this.duedate = duedate;
            }
        }

        public bool isExperimental()
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(experimental))
            {
                string exp = experimental.ToLower();
                if (exp == "1" || exp == "y" || exp == "yes")
                {
                    result = true;
                }
            }
            return result;
        }

        public bool showSurvey()
        {
            bool result = false;
            if (getDueDate() < DateTime.Today.AddDays(70).Ticks && getDueDate() > 0) {
                if (string.IsNullOrWhiteSpace(completed_survey))
                {
                    result = true;
                } else
                {
                    string c_s = completed_survey.ToLower();
                    if (c_s == "0" || c_s == "n" || c_s == "no")
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public void parseJson2DateWeightList()
        {
            dateWeights = parseJson2DateWeightList(json);
        }
        public static List<DateWeight> parseJson2DateWeightList(string json)
        {
            List<DateWeight> result = new List<DateWeight>();

            if (!String.IsNullOrWhiteSpace(json))
            {
                // To dictionary to remove duplicates
                Dictionary<long, double> datesWeightsDict = JsonConvert.DeserializeObject<Dictionary<long, double>>(json);

                // To list for easier sorting
                List<KeyValuePair<long, double>> datesWeightsList = datesWeightsDict.ToList();
                datesWeightsList.Sort(
                    delegate (KeyValuePair<long, double> pair1,
                    KeyValuePair<long, double> pair2)
                    {
                        return pair1.Key.CompareTo(pair2.Key);
                    }
                );

                // Create List<DateWeight>
                foreach (KeyValuePair<long, double> entry in datesWeightsList)
                {
                // do something with entry.Value or entry.Key
                result.Add(new DateWeight(entry.Key, entry.Value));
                }

            }
            return result;
        }

        public String parseDateWeightList2Json()
        {
            json = parseDateWeightList2Json(dateWeights);
            return json;
        }

        public static string parseDateWeightList2Json(List<DateWeight> dateWeights)
        {
            string result;

            Dictionary<long, double> tempDic = new Dictionary<long, double>();
            foreach(DateWeight dw in dateWeights){
                tempDic.Add(dw.mDate, dw.mWeight);
            }
            result = JsonConvert.SerializeObject(tempDic, Formatting.Indented);
            return result;
        }
        
        public DateWeight minDate()
        {
            return minDate(dateWeights);
        }
        public static DateWeight minDate(List<DateWeight> dateWeights)
        {
            DateWeight min = null;

            foreach(DateWeight dw in dateWeights)
            {
                if (min == null || min.mDate > dw.mDate)
                {
                    min = dw;
                }
            }

            return min;
        }


        public DateWeight maxDate()
        {
            return maxDate(dateWeights);
        }
        public static DateWeight maxDate(List<DateWeight> dateWeights)
        {
            DateWeight max = null;

            foreach (DateWeight dw in dateWeights)
            {
                if (max == null || max.mDate < dw.mDate)
                {
                    max = dw;
                }
            }

            return max;
        }

        public DateWeight minWeight()
        {
            return minWeight(dateWeights);
        }
        public static DateWeight minWeight(List<DateWeight> dateWeights)
        {
            DateWeight min = null;

            foreach (DateWeight dw in dateWeights)
            {
                if (min == null || min.mWeight > dw.mWeight)
                {
                    min = dw;
                }
            }

            return min;
        }


        public DateWeight maxWeight()
        {
            return maxWeight(dateWeights);
        }
        public static DateWeight maxWeight(List<DateWeight> dateWeights)
        {
            DateWeight max = null;

            foreach (DateWeight dw in dateWeights)
            {
                if (max == null || max.mWeight < dw.mWeight)
                {
                    max = dw;
                }
            }

            return max;
        }

        public DateWeight getDateWeight(long date)
        {
            return getDateWeight(dateWeights, date);
        }

        public static DateWeight getDateWeight(List<DateWeight> dateWeights, long date)
        {
            DateWeight result = null;

            foreach(DateWeight dw in dateWeights)
            {
                if (dw.mDate == date)
                {
                    result = dw;
                    break;
                }
            }
            return result;
        }

        public bool setDateWeight(long date, double weight)
        {
            bool result = false;
            foreach (DateWeight dw in dateWeights)
            {
                if (dw.mDate == date)
                {
                    dw.mWeight = weight;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void addDateWeight(DateWeight dw)
        {
            dateWeights = addDateWeight(dateWeights, dw);
        }
        public static List<DateWeight> addDateWeight(List<DateWeight> dateWeights, DateWeight dw)
        {
            dateWeights.Add(dw);
            return sort(dateWeights);
        }

        public static List<DateWeight> sort(List<DateWeight> dateWeights)
        {
            dateWeights.Sort(
                delegate (DateWeight pair1,
                DateWeight pair2)
                {
                    return pair1.mDate.CompareTo(pair2.mDate);
                }
            );
            return dateWeights;
        }
 
        public void printRecord()
        {
            Console.WriteLine("===========");
            Console.WriteLine("Record ID: " + record_id);
            Console.WriteLine("REDCapID: " + redcapid);
            Console.WriteLine("Experimental: " + experimental);
            Console.WriteLine("Completed Survey: " + completed_survey);
            Console.WriteLine("Height: " + height);
            Console.WriteLine("BMI: " + bmi);
            Console.WriteLine("JSON: " + json);
            foreach (DateWeight dw in dateWeights)
            {
                dw.toString();
            }
            Console.WriteLine("===========");
        }
    }
}