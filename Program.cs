using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebEndJSONTest
{
    class Program
    {

        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {

            string postObject = "[{\"id\":1,\"userid\":\"72777\",\"terminalid\":1,\"name\":\"72777\",\"checktime\":\"2019-07-17 13:48:15\",\"cloud_upload\":0},{\"id\":2,\"userid\":\"57116\",\"terminalid\":1,\"name\":\"57116\",\"checktime\":\"2019-07-14 09:22:13\",\"cloud_upload\":0}]";
            string cloudUrl = ConfigurationManager.AppSettings["CloudUrl"];
            uploadCloudData(postObject,cloudUrl);
            Console.ReadLine();

        }

        static async void uploadCloudData(string postObject, string cloudUrl)
        {

            List<CDeviceLog> logModelList = new List<CDeviceLog>();

            if (postObject != null)
            {
                //Program.writeErrorLog("JSON INSIDE POST:" + postObject);               
                try
                {
                    var response = await client.PostAsync(
                                        cloudUrl,
                                            new StringContent(postObject, Encoding.UTF8, "application/json"));

                    //Program.writeErrorLog("RESPONSE :" + response);
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("RESPONSE FORM :" + content);
                    logModelList = JsonConvert.DeserializeObject<List<CDeviceLog>>(@content);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(cloudUrl + ":" + ex.ToString());
                }


            }

            if (logModelList.Count > 0)
            {
                foreach (CDeviceLog logModel in logModelList)
                {
                    Console.WriteLine("userid:"+logModel.userid+"| time:"+logModel.checktime+"|cloud_upload:"+logModel.cloud_upload);
                }
            }

         }

    }
}
