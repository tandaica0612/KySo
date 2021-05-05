using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace Vnpt.iLIS.SignThueXMLApp
{
    public class WebService
    {
        public static string GetLienThongThueXml(string token, string id)
        {
            try
            {
                var address = "http://10.159.21.211:8088/api/lienthongthue/GetLienThongThueXML";
                var clientConnect2Api = new HttpClient();
                //clientConnect2Api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                clientConnect2Api.DefaultRequestHeaders.Add("Authorization", token);
                clientConnect2Api.BaseAddress = new Uri(string.Format("{0}/{1}", address, id));
                var respPost = clientConnect2Api.GetAsync(string.Empty).Result;
                if (respPost.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = null;
                    var strJson = respPost.Content.ReadAsStringAsync().Result;
                    //result = JsonConvert.DeserializeObject<string>(strJson);
                    result = strJson;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public static bool SignLienThongThueXml(string token, string id, string xml)
        {
            try
            {
                var address = "http://10.159.21.211:8088/api/lienthongthue/SignLienThongThueXML";
                var clientConnect2Api = new HttpClient();
                //clientConnect2Api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                clientConnect2Api.DefaultRequestHeaders.Add("Authorization", token);
                clientConnect2Api.BaseAddress = new Uri(string.Format("{0}", address));
                var body = new 
                { 
                    idBienDong = id,
                    xml = xml
                };
                var bodyJson = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var respPost = clientConnect2Api.PostAsync(string.Empty, bodyJson).Result;
                if (respPost.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = false;
                    var strJson = respPost.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<bool>(strJson);
                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}
