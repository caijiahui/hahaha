using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string ch = "。；，？！、“”‘’（）—";//中文字符
            const string en = @".;,?!\""""''()-";//英文字符
            var text = "Schottky Diode, BAT54,single, SOT23";
            char[] c = text.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                int n = ch.IndexOf(c[i]);
                if (n != -1) c[i] = en[n];
            }
            var cc = new string(c);
            //TBSopPath models = new TBSopPath();
            //try
            //{
            //    HttpClient _httpClient = new HttpClient();
            //    var parameters = new Dictionary<string, string>();
            //    parameters.Add("token", "050b5cc7-0194-4b0b-9a12-9c7fcf458a3a_Q-19568");
            //    var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = _httpClient.PostAsync(" http://job.advantech.com.cn/HRConsole/SSO/ValidateUserFromExam", byteContent);
            //    var result = response.Result.Content.ReadAsStringAsync();
            //   // var Data = Newtonsoft.Json.JsonConvert.DeserializeObject<QuerySerialNumberPartsResponse>(result.Result);
            //    //if (Data != null)
            //    //{
            //    //    if (Data.Data.Where(x => x.SN == PartSN).Count() != 0)
            //    //    {
            //    //        PartName = Data.Data.Where(x => x.SN == PartSN).FirstOrDefault().PartNo;
            //    //    }
            //    //}
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}


        }

        public class TBSopPath
        {
            public string Path { get; set; }
            public string FileName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool Flag { get; set; }
            public string Message { get; set; }

        }
    }
}
