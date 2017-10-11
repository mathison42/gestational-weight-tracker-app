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

        public REDCapHelper(string id)
        {
            mId = id;
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

                return post;
            }

        }

        public async Task<JsonValue> AddBasline(double height, double bmi)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(c.API_URL));
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            var input = new
            {
                id = mId,
                height = height,
                bmi = bmi
            };
            List<Object> records = new List<Object>();
            records.Add(input);
            string json = JsonConvert.SerializeObject(records);

            string parameters = JsonConvert.SerializeObject(new
            {
                token = c.API_TOKEN,
                content = "record",
                format = "json",
                type = "flat",
                overwriteBehavior = "normal",
                data = json,
                returnFormat = "json"
            });

            Console.WriteLine("json: " + json);

            Console.WriteLine("parameters: " + parameters);

            // Add parameters body
            var newStream = request.GetRequestStream();
            var byteParams = Encoding.ASCII.GetBytes(parameters);
            newStream.Write(byteParams, 0, byteParams.Length);
            newStream.Close();

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
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