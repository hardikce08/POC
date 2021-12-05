using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POC.Controllers
{
    public class BaseController : Controller
    {
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