using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //HttpClient _httpClient = new HttpClient();
            //var SerialId = "KAN522H216";
            //var response = _httpClient.GetAsync("http://172.21.128.84:801/api/Automate/GetInfoToSN?SerialId="+ SerialId);
            //var result = response.Result.Content.ReadAsStringAsync();
            //if (result != null)
            //{
            //    if(!string.IsNullOrEmpty(result.Result))
            //    {
            //        var res = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result.Result);
            //        var needresult = res["orderId"].ToString();
            //    }
            //}





            HttpClient _httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>();
            parameters.Add("passed", "1"); //结果（0/1）
            parameters.Add("stationType", "T0"); //站别（T0 / T1）  先返回T0数据再抛T1数据
            parameters.Add("userCode", "Q-19568");//人员工号
            parameters.Add("finishDate", DateTime.Now.ToString());//结束时间
            parameters.Add("serialNumber", "KAN522H216");//整机序号
            parameters.Add("errorCode", "");//当结果返回0时返回错误代码
            var response = _httpClient.PostAsync("http://172.21.128.84:801/api/Automate/AutoTempResult", new FormUrlEncodedContent(parameters));
            var result = response.Result.Content.ReadAsStringAsync();
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.Result))
                {
                    var res = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result.Result);
                    var succeed = res["succeed"].ToString();
                    var message = res["message"].ToString();
                }
            }


        }
    }
}
