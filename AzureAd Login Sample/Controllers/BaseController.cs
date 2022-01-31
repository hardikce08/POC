using Newtonsoft.Json;
using POC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace POC.Controllers
{
    public class BaseController : Controller
    {
        //If the data exists in cache, pull it from there, otherwise make a call to database to get the data
        ObjectCache cache = MemoryCache.Default;
        public BaseController()
        {
        }
        private string _bearertoken;
        public string BearerToken
        {
            get
            {
                if (_bearertoken == null)
                {
                    _bearertoken = System.Configuration.ConfigurationManager.AppSettings["BearerToken"].ToString();
                }

                return _bearertoken;
            }
        }
        private string _apidomin;
        public string ApiDomain
        {
            get
            {
                if (_apidomin == null)
                {
                    _apidomin = System.Configuration.ConfigurationManager.AppSettings["APIURL"].ToString();
                }

                return _apidomin;
            }
        }
        public string GetAllOrganizationAPIURL => ApiDomain + "/v1/organizations/all";
        public string GetAllContractAPIURL => ApiDomain + "/v1/contracts";
        public string GetAllTransferRequestAPIURL => ApiDomain + "/v1/products/transfer/requests";
        public TEntity GetDataFromCache<TEntity>(string key, string APIURL) where TEntity : class
        {
            var Data = cache.Get(key) as TEntity;
            if (Data != null)
            {
                return Data;
            }
            else
            {
                var APIrequest = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                Data = JsonConvert.DeserializeObject<TEntity>(APIrequest.ResponseString);
            }
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(120) };
            cache.Add(key, Data, policy);
            return Data;
        }
        public void RemoveDataFromCache(string key)
        {
            if (key == "All" || key == string.Empty)
            {
                foreach (var objcache in cache.ToArray())
                {
                    cache.Remove(objcache.Key);
                }
            }
            else
            {
                cache.Remove(key);
            }
        }
        public enum CacheType
        {
            AllProductResponse
        }
        //private SessionProfile _currentProfile;
        //public SessionProfile CurrentProfile
        //{
        //    get
        //    {
        //        if (_currentProfile == null)
        //        {
        //            _currentProfile = ProfileHelper.Profile;
        //        }

        //        return _currentProfile;
        //    }
        //}
    }
}