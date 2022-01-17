using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POC.Helpers
{
    public static class WebHelper
    {
        public class ContentType
        {
            public const string form_urlencoded = "application/x-www-form-urlencoded";
            public const string application_json = "application/json; charset=utf-8";
            public const string application_xml = "application/xml";
            public const string text_xml = "text/xml; encoding='utf-8'";
            public const string application_pdf = "application/pdf";
            public const string application_vnd = "application/vnd.cpc.authreturn-v2+xml";
            public const string application_vnd_shipment = "application/vnd.cpc.shipment-v8+xml";
            public const string application_vnd_manifest = "application/vnd.cpc.manifest-v8+xml";
            public const string all = "*/*";
        }
        public class ImagePath
        {
            public static string no_image = "//cdn.omniparcel.com/public/images/NoImage_New.png";
        }
        public class WebResponse
        {
            public string ResponseString { get; set; }
            public System.Net.HttpStatusCode ResponseCode { get; set; }
        }

        public class WebResponseInBytes
        {
            public string ResponseString { get; set; }
            public System.Net.HttpStatusCode ResponseCode { get; set; }
            public byte[] ResponseBytes { get; set; }
        }

        public static string GetWebAPIResponseString(string url, string contentType, string methodType = "GET", string postData = "", string header = "")
        {
            string responseString = string.Empty;
            var response = CallWebAPI(url, contentType, out responseString, methodType, postData, header);
            return responseString;
        }

        public static WebResponse GetWebAPIResponse(string url, string contentType, string methodType = "GET", string postData = "", string header = "")
        {
            WebResponse objWebResponse = new WebResponse();
            string responseString = string.Empty;
            var response = CallWebAPI(url, contentType, out responseString, methodType, postData, header);
            if (response != null)
            {
                objWebResponse.ResponseCode = ((System.Net.HttpWebResponse)(response)).StatusCode;
                objWebResponse.ResponseString = responseString;
            }
            return objWebResponse;
        }

        public static WebResponse GetWebAPIResponseWithErrorDetails(string url, string contentType, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "",string BrarerToken="")
        {
            WebResponse objWebResponse = new WebResponse();
            string responseString = string.Empty;
            var response = CallWebAPIResponseWithErrorDetailsNew(url, contentType, out responseString, methodType, postData, header, header1, accept, BrarerToken);
            if (response != null)
            {
                objWebResponse.ResponseCode = ((System.Net.HttpWebResponse)(response)).StatusCode;
                objWebResponse.ResponseString = responseString;
            }
            return objWebResponse;
        }

        public static WebResponse GetWebAPIResponseWithErrorDetailsV2(string url, string contentType, string methodType = "GET", string postData = "", string header1 = "", string header2 = "", string header3 = "", string accept = "")
        {
            WebResponse objWebResponse = new WebResponse();
            string responseString = string.Empty;
            var response = CallWebAPIResponseWithErrorDetails(url, contentType, out responseString, methodType, postData, header1, header2, header3, accept);
            if (response != null)
            {
                objWebResponse.ResponseCode = ((System.Net.HttpWebResponse)(response)).StatusCode;
                objWebResponse.ResponseString = responseString;
            }
            return objWebResponse;
        }
        public static WebResponse GetWebAPIResponseWithErrorDetails_HttpClient(string url, string contentType, string methodType = "GET", string postData = "", Dictionary<string, string> headers = null, string accept = "")
        {
            WebResponse objWebResponse = new WebResponse();
            var response = CallWebAPIResponseWithErrorDetails_HttpClient(url, contentType, methodType, postData, headers, accept);
            if (response != null)
            {
                objWebResponse = response.Result;
            }
            return objWebResponse;
        }

        private static HttpWebResponse CallWebAPI(string url, string contentType, out string responseString, string methodType = "GET", string postData = "", string header = "")
        {
           // ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            responseString = string.Empty;
            string responseCode = string.Empty;
            methodType = string.IsNullOrWhiteSpace(methodType) ? WebRequestMethods.Http.Get : methodType;

            HttpWebResponse response = null;
            try
            {
                request.Method = methodType;
                request.ContentType = contentType;
                request.Proxy = null;

                if (!string.IsNullOrEmpty(header))
                {
                    request.Headers.Add(header);
                }

                if (!string.IsNullOrWhiteSpace(postData))
                {
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response)).StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }
            return response;
        }

        private static HttpWebResponse CallWebAPIResponseWithErrorDetailsNew(string url, string contentType, out string responseString, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "",string BearerToken="")
        {
            System.Net.WebResponse response1 = null;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            responseString = string.Empty;
            string responseCode = string.Empty;
            methodType = string.IsNullOrWhiteSpace(methodType) ? WebRequestMethods.Http.Get : methodType;

            
            HttpWebResponse response = null;
            try
            {
                request.Method = methodType;
                request.ContentType = contentType;
                request.Proxy = null;

                if (!string.IsNullOrEmpty(accept))
                {
                    request.Accept = accept;
                }
                if (!string.IsNullOrEmpty(header))
                {
                    request.Headers.Add(header);
                }
                if (!string.IsNullOrEmpty(header1))
                {
                    request.Headers.Add(header1);
                }
                if (!string.IsNullOrEmpty(BearerToken))
                {
                    request.Headers.Add("Authorization", "Bearer "+ BearerToken);
                }
                if (!string.IsNullOrWhiteSpace(postData))
                {
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response)).StatusCode.ToString();
                }

            }
            catch (WebException e)
            {
                

                response1 = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response1;
                using (Stream data = response1.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    responseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response1)).StatusCode.ToString();

                }
                response = httpResponse;

            }
            finally
            {
                 
            }
            return response;
        }

        private static HttpWebResponse CallWebAPIResponseWithErrorDetails(string url, string contentType, out string responseString, string methodType = "GET", string postData = "", string header1 = "", string header2 = "", string header3 = "", string accept = "")
        {
            System.Net.WebResponse response1 = null;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var startTime = DateTime.UtcNow;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            responseString = string.Empty;
            string responseCode = string.Empty;
            methodType = string.IsNullOrWhiteSpace(methodType) ? WebRequestMethods.Http.Get : methodType;

            
            HttpWebResponse response = null;
            try
            {
                request.Method = methodType;
                request.ContentType = contentType;
                request.Proxy = null;

                if (!string.IsNullOrEmpty(accept))
                {
                    request.Accept = accept;
                }
                if (!string.IsNullOrEmpty(header1))
                {
                    request.Headers.Add(header1);
                }
                if (!string.IsNullOrEmpty(header2))
                {
                    request.Headers.Add(header2);
                }
                if (!string.IsNullOrEmpty(header3))
                {
                    request.Headers.Add(header3);
                }
                if (!string.IsNullOrWhiteSpace(postData))
                {
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response)).StatusCode.ToString();
                }

            }
            catch (WebException e)
            {
                

                response1 = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response1;
                using (Stream data = response1.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    responseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response1)).StatusCode.ToString();

                }
                response = httpResponse;

            }
            finally
            {
                 
            }
            return response;
        }

        private static async Task<WebResponse> CallWebAPIResponseWithErrorDetails_HttpClient(string url, string contentType, string methodType = "GET", string postData = "", Dictionary<string, string> headers = null, string accept = "")
        {

            WebResponse objWebResponse = new WebResponse();
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var startTime = DateTime.UtcNow;
            HttpWebRequest requestForLogEntry = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            string responseCodeForLogEntry = string.Empty;
            string responseStringForLogEntry = string.Empty;
            methodType = string.IsNullOrWhiteSpace(methodType) ? WebRequestMethods.Http.Get : methodType;

             

            var _httpClient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                requestForLogEntry.Method = methodType;
                requestForLogEntry.ContentType = contentType;

                if (!string.IsNullOrEmpty(accept))
                {
                    requestForLogEntry.Accept = accept;
                    _httpClient.DefaultRequestHeaders.Add("Accept", accept);
                }

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        string header = item.Key.ToString() + ":" + item.Value.ToString();
                        requestForLogEntry.Headers.Add(header);
                        _httpClient.DefaultRequestHeaders.Add(item.Key.ToString(), item.Value.ToString());
                    }
                }

                if (methodType == "POST")
                {
                    HttpContent httpContent = new StringContent(postData, Encoding.UTF8, contentType);
                    response = await _httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
                }
                else if (methodType == "GET")
                {
                    response = await _httpClient.GetAsync(url);
                }
                if (response != null)
                {
                    objWebResponse.ResponseCode = response.StatusCode;
                    responseCodeForLogEntry = response.StatusCode.ToString();
                    using (HttpContent content = response.Content)
                    {
                        responseStringForLogEntry = await content.ReadAsStringAsync();
                        objWebResponse.ResponseString = responseStringForLogEntry;
                    }
                }
            }
            catch (WebException e)
            {
               
                System.Net.WebResponse response1 = null;
                response1 = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response1;
                using (Stream data = response1.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    responseStringForLogEntry = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    responseCodeForLogEntry = httpResponse.StatusCode.ToString();
                    objWebResponse.ResponseString = responseStringForLogEntry;
                    objWebResponse.ResponseCode = httpResponse.StatusCode;
                }
                //response = httpResponse;

                //responseStringForLogEntry = "Exception";
                //if (e?.InnerException != null)
                //{
                //    responseStringForLogEntry = e.InnerException.ToString();
                //}
                //else if (e?.Message != null)
                //{
                //    responseStringForLogEntry = e.Message;
                //}

                ////HttpResponseMessage httpResponse = new HttpResponseMessage();
                //objWebResponse.ResponseCode = (HttpStatusCode)e.ErrorCode;
                //objWebResponse.ResponseString = responseStringForLogEntry;              
                //responseCodeForLogEntry = e.ErrorCode.ToString();


            }
            finally
            { 
            }
            return objWebResponse;
        }


        public static WebResponseInBytes CallWebAPIResponseWithBytes(string url, string contentType, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "")
        {
            WebResponseInBytes objWebResponse = new WebResponseInBytes();
            System.Net.WebResponse response1 = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            methodType = string.IsNullOrWhiteSpace(methodType) ? WebRequestMethods.Http.Get : methodType;

            

            HttpWebResponse response = null;
            try
            {
                request.Method = methodType;
                request.ContentType = contentType;
                request.Proxy = null;

                if (!string.IsNullOrEmpty(accept))
                {
                    request.Accept = accept;
                }
                if (!string.IsNullOrEmpty(header))
                {
                    request.Headers.Add(header);
                }
                if (!string.IsNullOrEmpty(header1))
                {
                    request.Headers.Add(header1);
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    byte[] buffer = new byte[4096];
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            int count = 0;
                            do
                            {
                                count = responseStream.Read(buffer, 0, buffer.Length);
                                memoryStream.Write(buffer, 0, count);

                            } while (count != 0);

                            objWebResponse.ResponseCode = ((System.Net.HttpWebResponse)(response)).StatusCode;
                            objWebResponse.ResponseBytes = memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                

                response1 = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response1;
                using (Stream data = response1.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    objWebResponse.ResponseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    objWebResponse.ResponseCode = ((System.Net.HttpWebResponse)(response1)).StatusCode;
                }
            }
            finally
            {
                 
            }

            return objWebResponse;
        }

        #region API call with non english character
        public static WebResponse CallWebAPIResponseWithErrorDetailsInEncodingUTF8(string url, string contentType, HttpMethod methodType, string postData, List<string> headers, string accept)
        {
            WebResponse webResponse = null;
           // LogAPICall(url, contentType, methodType.ToString(), postData, headers.ToString(), null);
            HttpWebRequest request = GetHttpWebRequest(url, contentType, methodType, postData, headers, accept);
            webResponse = GetHttpWebResponse(request);
            return webResponse;
        }
        public static WebResponse CallWebAPIResponseWithErrorDetailsInEncodingUTF8ContentLengthZero(string url, string contentType, HttpMethod methodType, List<string> headers, string accept)
        {
            WebResponse webResponse = null;
            //LogAPICall(url, contentType, methodType.ToString(), null, headers.ToString(), null);
            HttpWebRequest request = GetHttpWebRequest(url, contentType, methodType, null, headers, accept);
            request.ContentLength = 0;
            webResponse = GetHttpWebResponse(request);
            return webResponse;
        }
        public static WebResponse GetWebAPIResponseWithErrorDetailsInEncodingUTF8(string url, string contentType, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "")
        {
            WebResponse webResponse = new WebResponse();
            try
            {
                webResponse = CallWebAPIResponseWithErrorDetailsInEncodingUTF8(url, contentType, methodType, postData, header, header1, accept);
            }
            catch (WebException ex)
            {
                System.Net.WebResponse response1 = ex.Response;
                using (Stream data = response1.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    webResponse.ResponseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    webResponse.ResponseCode = ((System.Net.HttpWebResponse)(response1)).StatusCode;
                }

               // LogAPICall(url, contentType, methodType, postData, header, ex);
            }
            catch (Exception ex)
            {
               // LogAPICall(url, contentType, methodType, postData, header, ex);
            }
            finally
            {
               // LogAPICall(url, contentType, methodType, postData, header, null);
            }

            return webResponse;
        }
        public static WebResponse CallWebAPIResponseWithErrorDetailsInEncodingUTF8(string url, string contentType, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "")
        {
            WebResponse webResponse = null;
           // LogAPICall(url, contentType, methodType, postData, header, null);
            HttpWebRequest request = GetHttpWebRequest(url, contentType, methodType, postData, header, header1, accept);
            webResponse = GetHttpWebResponse(request);
            return webResponse;
        }

        private static WebResponse GetHttpWebResponse(HttpWebRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            WebResponse webResponse = new WebResponse();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                webResponse.ResponseCode = response.StatusCode;
                webResponse.ResponseString = streamReader.ReadToEnd();
                streamReader.Close();
            }
            response.Close();
            return webResponse;
        }
        private static HttpWebRequest GetHttpWebRequest(string url, string contentType, HttpMethod methodType, string postData, List<string> headers, string accept)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            request.Method = methodType.ToString();
            request.ContentType = contentType;
            if (!string.IsNullOrEmpty(accept))
            {
                request.Accept = accept;
            }

            foreach (var header in headers)
            {
                if (!string.IsNullOrEmpty(header))
                {
                    request.Headers.Add(header);
                }
            }

            if (!string.IsNullOrWhiteSpace(postData))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            return request;
        }
        private static HttpWebRequest GetHttpWebRequest(string url, string contentType, string methodType, string postData, string header, string header1, string accept)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            request.Method = methodType;
            request.ContentType = contentType;
            if (!string.IsNullOrEmpty(accept))
            {
                request.Accept = accept;
            }
            if (!string.IsNullOrEmpty(header))
            {
                request.Headers.Add(header);
            }
            if (!string.IsNullOrEmpty(header1))
            {
                request.Headers.Add(header1);
            }
            if (!string.IsNullOrWhiteSpace(postData))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = contentType;
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            return request;
        }

        //private static void LogAPICall(string url, string contentType, string methodType, string postData, string header, Exception ex)
        //{
        //    //TODO: Need to apply caching for finding log flag.
        //    var enableLog = SS.Freight.DataAccess.Config.GetValue(Model.Config.ConfigKey.EnableWebServiceInfoLog);
        //    if (enableLog == "1")
        //    {
        //        using (var ts1 = new TransactionScope(TransactionScopeOption.RequiresNew))
        //        {
        //            if (ex != null)
        //            {
        //                SS.Freight.ExceptionManager.ExceptionTracker.Publish(ex, SS.Freight.ExceptionManager.EventSource.Portal, SS.Freight.ExceptionManager.EventType.Error, "", "ThridParyt-Api-Call");
        //            }
        //            string logMsg = string.Format("Url:{0},contentType:{1}, methodType:{2}, postData:{3},header:{4}", url, contentType, methodType, postData, header);
        //            SS.Freight.ExceptionManager.ExceptionTracker.Publish("CallWebAPI", logMsg, SS.Freight.ExceptionManager.EventSource.Portal, SS.Freight.ExceptionManager.EventType.Info, "", "");
        //            ts1.Complete();
        //        }
        //    }
        //}

        #endregion
        // BOTH METHODS ARE SPECIFIC TO DHL TRACKING TO SKIP THE LOG ENTRIES FOR STATUS CODE 400 ( NO DATA FOUND )
        public static WebResponse GetWebAPIResponseWithErrorDetails_DHL(string url, string contentType, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "")
        {
            WebResponse objWebResponse = new WebResponse();
            string responseString = string.Empty;
            var response = CallWebAPIResponseWithErrorDetails_DHL(url, contentType, out responseString, methodType, postData, header, header1, accept);
            if (response != null)
            {
                objWebResponse.ResponseCode = ((System.Net.HttpWebResponse)(response)).StatusCode;
                objWebResponse.ResponseString = responseString;
            }
            return objWebResponse;
        }

        private static HttpWebResponse CallWebAPIResponseWithErrorDetails_DHL(string url, string contentType, out string responseString, string methodType = "GET", string postData = "", string header = "", string header1 = "", string accept = "")
        {
            System.Net.WebResponse response1 = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            responseString = string.Empty;
            string responseCode = string.Empty;
            methodType = string.IsNullOrWhiteSpace(methodType) ? WebRequestMethods.Http.Get : methodType;

            

            HttpWebResponse response = null;
            try
            {
                request.Method = methodType;
                request.ContentType = contentType;
                request.Proxy = null;

                if (!string.IsNullOrEmpty(accept))
                {
                    request.Accept = accept;
                }
                if (!string.IsNullOrEmpty(header))
                {
                    request.Headers.Add(header);
                }
                if (!string.IsNullOrEmpty(header1))
                {
                    request.Headers.Add(header1);
                }
                if (!string.IsNullOrWhiteSpace(postData))
                {
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response)).StatusCode.ToString();
                }

            }
            catch (WebException e)
            {
                response1 = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response1;
                using (Stream data = response1.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    responseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    responseCode = ((System.Net.HttpWebResponse)(response1)).StatusCode.ToString();

                }
                response = httpResponse;

                
            }
            finally
            {
                 
            }
            return response;
        }
        // END DHL
    }
}
