using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using POC.Helper;
namespace POC
{
    public static class ProfileHelper
    {
        /// <summary>
        /// Presently logged in profile
        /// </summary>
        public static SessionProfile Profile
        {
            get
            {
                if (HttpContext.Current == null || !HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                SessionProfile profile = null;
                //string key = string.Format(CacheKeys.SesssionProfile, HttpContext.Current.User.Identity.Name);
                var userClaims = HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;

                CacheHelper.Get<SessionProfile>(userClaims?.FindFirst("name")?.Value, out profile, CacheRegionEnum.Security);

                //var o = HttpContext.Current.Cache[HttpContext.Current.User.Identity.Name];

                if (profile == null)
                {
                    profile = new SessionProfile();

                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        
                        profile.LoggedUserProfile = new User();
                        profile.LoggedUserProfile.UserGuid = userClaims?.FindFirst("aud")?.Value;
                        profile.LoggedUserProfile.FirstName = userClaims?.FindFirst("name")?.Value;
                        profile.LoggedUserProfile.EmailAddress = userClaims?.FindFirst("preferred_username")?.Value;
                        profile.IsAdministrator = false;
                        //You get the user's first and last name below:


                        // The 'preferred_username' claim can be used for showing the username


                        // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
                        //ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                        // TenantId is the unique Tenant Id - which represents an organization in Azure AD
                        //ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
                        //Session["UserID"] = userClaims?.FindFirst("aud")?.Value;
                        CacheHelper.Add<SessionProfile>(userClaims?.FindFirst("name")?.Value, profile, CacheRegionEnum.Security, 10);
                    }

                    //UserService us = new UserService();
                    //profile.LoggedUserProfile = us.UserProfileByUserId(HttpContext.Current.User.Identity.Name.Replace("SessionProfile_", ""));
                    //CacheHelper.Add<SessionProfile>(HttpContext.Current.User.Identity.Name, profile, CacheRegionEnum.Security, 10);
                    // add object in cache so we can resolve someone on the site is active
                    System.Diagnostics.Trace.WriteLine("User profile added to cache");

                }
                //CacheHelper.Get<SessionProfile>(key, out o, CacheRegionEnum.Security);
                return profile;
                //return (SessionProfile)HttpContext.Current.Cache[HttpContext.Current.User.Identity.Name];
            }
        }
    }
    public class SessionProfile
    {
        public SessionProfile()
        {

        }
        public User LoggedUserProfile { get; set; }


        public bool IsAdministrator { get; set; }

        //CacheRegionEnum _region;
        public CacheRegionEnum CacheRegion { get; set; }


    }
    public class User
    {
        public int UserID { get; set; }
        
        public string EmailAddress { get; set; }
 
       
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNumber { get; set; }

       
        public bool RememberSignIn { get; set; }
        public string UserGuid { get; set; }
    }
}