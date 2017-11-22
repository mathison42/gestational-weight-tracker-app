extern alias json;

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

using GWG.Resources.hidden;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using GWG.survey;
using System.Threading;

namespace GWG.Resources.redcap
{
    class REDCapHelper
    {
        private readonly Config c = new Config();
        private string mId;
        private string mRecordID;

        public REDCapHelper(string id)
        {
            mId = id;
        }

        public REDCapHelper(string id, string record_id)
        {
            mId = id;
            mRecordID = record_id;
        }

        public async Task<REDCapResult> GetMinimumProfile()
        {
            String[] fields = new String[] { "record_id" };
            return await GetRecord(fields);
        }

        public async Task<REDCapResult> GetProfile()
        {
            String[] fields = new String[] { "record_id", "control", "redcapid", "initdate", "count", "height", "bmi", "json" };
            REDCapResult result = await GetRecord(fields);
            mRecordID = result.record_id;
            return result;
        }

        public async Task<REDCapResult> GetRecord(String[] fields)
        {
            var forms = new String[] { c.DATABASE };

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", c.API_TOKEN),
                new KeyValuePair<string, string>("content", "record"),
                new KeyValuePair<string, string>("format", "json"),
                new KeyValuePair<string, string>("type", "flat"),
                new KeyValuePair<string, string>("forms", forms.ToString()),
                new KeyValuePair<string, string>("fields", fields.ToString()),
                new KeyValuePair<string, string>("rawOrLabel", "raw"),
                new KeyValuePair<string, string>("rawOrLabelHeaders", "raw"),
                new KeyValuePair<string, string>("exportCheckboxLabel", "false"),
                new KeyValuePair<string, string>("exportSurveyFields", "false"),
                new KeyValuePair<string, string>("exportDataAccesGroups", "false"),
                new KeyValuePair<string, string>("returnFormat", "json"),
                new KeyValuePair<string, string>("filterLogic", "[redcapid]=" + mId) // Automatically gets everything if specified. Can't get specific fields.
            });


            using (var client = new HttpClient())
            {
                var result = await client.PostAsync(c.API_URL, content);

                // handling the answer  
                var resultString = await result.Content.ReadAsStringAsync();

                try
                {
                    // on error throw a exception  
                    result.EnsureSuccessStatusCode();
                } finally
                {
                    //Console.WriteLine("content: " + content.ToString());

                    if (resultString.StartsWith("["))
                    {
                        resultString = resultString.Substring(1);

                    }

                    if (resultString.EndsWith("]"))
                    {
                        resultString = resultString.Substring(0, resultString.Length - 1);
                    }

                    //Console.WriteLine("resultString: " + resultString);
                }
                REDCapResult post = JsonConvert.DeserializeObject<REDCapResult>(resultString);

                return post;
            }

        }


        public class PostCountResult
        {
            public int Count { get; set; }
        };

        public async Task<bool> SaveCount(int count)
        {
            return await AddCount(count);
        }

        public async Task<bool> SaveInitBaseline(double height, double bmi, long duedate)
        {
            return await AddRecord(height, bmi, true, duedate, null, null);
        }

        public async Task<bool> SaveDueDate(long duedate)
        {
            return await AddRecord(Double.MinValue, Double.MinValue, false, duedate, null, null);
        }

        public async Task<bool> SaveDateWeights(string json, string jsonshort)
        {
            return await AddRecord(Double.MinValue, Double.MinValue, false, Int64.MinValue, json, jsonshort);
        }

        private async Task<bool> AddCount(int count)
        {
            String data = "<records><item><record_id>" + mRecordID + "</record_id>";
            if (count > 0)
            {
                data = data + "<count>" + count.ToString() + "</count>";
            }
            data = data + "</item></records>";

            return await AddRecord(data);
        }

        private async Task<bool> AddRecord(double height, double bmi, bool setInitDate, long duedate, string json, string jsonshort)
        {
            String data = "<records><item><record_id>" + mRecordID + "</record_id>";
            if (bmi != Double.MinValue)
            {
                data = data + "<bmi>" + bmi.ToString("0.0") + "</bmi>";
            }
            if (height != Double.MinValue)
            {
                data = data + "<height>" + height.ToString("0.0") + "</height>";
            }
            if (setInitDate)
            {
                data = data + "<initdate>" + DateTime.Today.Ticks + "</initdate>";
                data = data + "<initdateshort>" + DateTime.Today.ToShortDateString() + "</initdateshort>";
                Console.WriteLine("SETTTING INIT DATE: " + DateTime.Today.Ticks + " _ " + DateTime.Today.ToShortDateString());
            }
            if (duedate != Int64.MinValue)
            {
                data = data + "<duedate>" + duedate.ToString() + "</duedate>";
                data = data + "<duedateshort>" + new DateTime(duedate).ToShortDateString() + "</duedateshort>";
            }
            if (!String.IsNullOrWhiteSpace(json))
            {
                data = data + "<json>" + json.ToString() + "</json>";
            }
            if (!String.IsNullOrWhiteSpace(jsonshort))
            {
                data = data + "<jsonshort>" + jsonshort.ToString() + "</jsonshort>";
            }
            data = data + "</item></records>";

            return await AddRecord(data);
        }

        public Task<bool> AddRecord(SurveyResults survey)
        {
            String data = "<records><item><record_id>" + mRecordID + "</record_id>";
            if (!String.IsNullOrWhiteSpace(survey.q1))
            {
                data = data + "<q1>" + survey.q1.ToString() + "</q1>";
            }
            if (!String.IsNullOrWhiteSpace(survey.q2))
            {
                data = data + "<q2>" + survey.q2.ToString() + "</q2>";
            }
            if (!String.IsNullOrWhiteSpace(survey.q3))
            {
                data = data + "<q3>" + survey.q3.ToString() + "</q3>";
            }
            if (!String.IsNullOrWhiteSpace(survey.q4))
            {
                data = data + "<q4>" + survey.q4.ToString() + "</q4>";
            }
            if (!String.IsNullOrWhiteSpace(survey.q5))
            {
                data = data + "<q5>" + survey.q5.ToString() + "</q5>";
            }
            if (!String.IsNullOrWhiteSpace(survey.q6))
            {
                data = data + "<q6>" + survey.q6.ToString() + "</q6>";
            }
            if (!String.IsNullOrWhiteSpace(survey.q7))
            {
                data = data + "<q7>" + survey.q7.ToString() + "</q7>";
            }

            data = data + "<completed_survey>1</completed_survey>";
            data = data + "</item></records>";
            return AddRecord(data);
        }

        private async Task<bool> AddRecord(string data)
        {
            bool success = false;

            if (String.IsNullOrWhiteSpace(mRecordID))
            {
                Console.WriteLine("[Error] Must first set mRecordID to add new records.");
                return false;
            }

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", c.API_TOKEN),
                new KeyValuePair<string, string>("content", "record"),
                new KeyValuePair<string, string>("format", "xml"),
                new KeyValuePair<string, string>("type", "flat"),
                new KeyValuePair<string, string>("overwriteBehavior", "normal"),
                new KeyValuePair<string, string>("forceAutoNumber", "false"),
                new KeyValuePair<string, string>("data", data),
                new KeyValuePair<string, string>("returnContent", "count"),
                new KeyValuePair<string, string>("returnFormat", "json")
            });


            using (var client = new HttpClient())
            {
                var result = await client.PostAsync(c.API_URL, content);

                // handling the answer  
                var resultString = await result.Content.ReadAsStringAsync();

                try
                {
                    // on error throw a exception  
                    result.EnsureSuccessStatusCode();
                }
                finally
                {
                    //Console.WriteLine("content: " + resultString.ToString());
                }
                PostCountResult pcr = JsonConvert.DeserializeObject<PostCountResult>(resultString);

                if (pcr.Count == 1)
                {
                    success = true;
                }

                return success;
            }

        }
        
        public async Task<string> GetVersion()
        {
            
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", c.API_TOKEN),
                new KeyValuePair<string, string>("content", "version")
            });


            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var result = await client.PostAsync(c.API_URL, content);

                // handling the answer  
                var version = await result.Content.ReadAsStringAsync();

                try
                {
                    // on error throw a exception  
                    result.EnsureSuccessStatusCode();
                }
                finally
                {
                    //Console.WriteLine("content: " + content.ToString());
                    //Console.WriteLine("version: " + version);
                }

                //Console.WriteLine("Post Response: " + version);
                return version;
            }

        }


        public async Task<bool> DoesREDCapIdExistAsync()
        {
            bool result = false;
            //////////////////////////////
            // Verify REDCap ID here...
            REDCapResult profile = null;
            profile = await GetMinimumProfile();
            if (profile != null)
            {
                int redcapid;
                if (int.TryParse(profile.redcapid, out redcapid) && profile.redcapid == mId)
                {
                    result = true;
                    //Console.WriteLine("Results Here: " + redcapid);
                }
            }
            //result = true;
            //////////////////////////////
            return result;
        }
    }
}