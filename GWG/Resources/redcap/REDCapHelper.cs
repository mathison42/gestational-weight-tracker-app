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

        public async Task<REDCapResult> GetProfile()
        {
            var fields = new String[] { "record_id", "redcapid", "height", "bmi", "json" };
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
                new KeyValuePair<string, string>("filterLogic", "[redcapid]=" + mId)
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
                    Console.WriteLine("content: " + content.ToString());

                    if (resultString.StartsWith("["))
                    {
                        resultString = resultString.Substring(1);

                    }

                    if (resultString.EndsWith("]"))
                    {
                        resultString = resultString.Substring(0, resultString.Length - 1);
                    }

                    Console.WriteLine("resultString: " + resultString);
                }
                //REDCapResult post = new REDCapResult();
                //REDCapResult post = JsonConvert.DeserializeObject<REDCapResult>(resultString);
                REDCapResult post = JsonConvert.DeserializeObject<REDCapResult>(resultString);

                mRecordID = post.record_id;

                return post;
            }

        }


        public class PostCountResult
        {
            public int Count { get; set; }
        };
    
        public async Task<bool> SaveBaseline(double height, double bmi, long duedate)
        {
            return await AddRecord(height, bmi, duedate, null);
        }

        public async Task<bool> SaveDueDate(long duedate)
        {
            return await AddRecord(Double.MinValue, Double.MinValue, duedate, null);
        }

        public async Task<bool> SaveDateWeights(string json)
        {
            return await AddRecord(Double.MinValue, Double.MinValue, Int64.MinValue, json);
        }

        private async Task<bool> AddRecord(double height, double bmi, long duedate, string json)
        {
            bool success = false;

            if (String.IsNullOrWhiteSpace(mRecordID))
            {
                Console.WriteLine("[Error] Must first set mRecordID to add new records.");
                return false;
            }

            String data = "<records><item><record_id>" + mRecordID + "</record_id>";
            if (bmi != Double.MinValue) {
                data = data + "<bmi>" + bmi.ToString("0.0") + "</bmi>";
            }
            if (height != Double.MinValue)
            {
                data = data + "<height>" + height.ToString("0.0") + "</height>";
            }
            if (duedate != Int64.MinValue)
            {
                data = data + "<duedate>" + duedate.ToString() + "</duedate>";
            }
            if (!String.IsNullOrWhiteSpace(json))
            {
                data = data + "<json>" + json.ToString() + "</json>";
            }
            data = data + "</item></records>";

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
                    Console.WriteLine("content: " + content.ToString());
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
                    Console.WriteLine("content: " + content.ToString());
                    Console.WriteLine("version: " + version);
                }

                Console.WriteLine("Post Response: " + version);
                return version;
            }

        }
    }
}