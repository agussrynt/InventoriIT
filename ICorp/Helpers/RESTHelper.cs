using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PlanCorp.Helpers
{
    public class RESTHelper
    {
        public static T Get<T>(string url, string token = "")
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token.Trim()))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;
                return ResultHandler<T>(responseMessage);
            }
        }

        public static T Post<T>(string url, object param, string token = "")
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token.Trim()))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string postBody = JsonConvert.SerializeObject(param);
                HttpResponseMessage responseMessage = client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                return ResultHandler<T>(responseMessage);
            }
        }

        public static T Post<T>(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.ASCII.GetBytes(param);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (!String.IsNullOrEmpty(responseString))
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            else
            {
                return default(T);
            }
        }

        public static T ResultHandler<T>(HttpResponseMessage responseMessage)
        {
            //var result = default(dynamic);
            //string responseString = responseMessage.Content.ReadAsStringAsync().Result;
            //if (responseMessage.StatusCode == HttpStatusCode.OK)
            //{
            //    if (true)
            //    {
            //        result = JsonConvert.DeserializeObject<dynamic>(responseString);
            //    }
            //}
            //return result;
            //string responseString = responseMessage.Content.ReadAsStringAsync().Result;
            //if (responseMessage.StatusCode == HttpStatusCode.OK)
            //{
            //    if (true)
            //    {
            //        result = JsonConvert.DeserializeObject<T>(responseString);
            //    }
            //}
            //return result;
            string responseString = responseMessage.Content.ReadAsStringAsync().Result;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                if (!String.IsNullOrEmpty(responseString))
                {
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
                else
                {
                    return default(T);
                }
            }
            else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return default(T);
            }
            else
            {
                return default(T);
            }
        }

        public static T PostNotJson<T>(string url, string param, string token = "")
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token.Trim()))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string postBody = param;
                HttpResponseMessage responseMessage = client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
                return ResultHandler<T>(responseMessage);
            }
        }
    }
}
