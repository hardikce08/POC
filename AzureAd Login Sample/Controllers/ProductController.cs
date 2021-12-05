using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using POC.Controllers;
using POC.DataAccess.Model;
using POC.Helpers;

namespace AzureAd_Login_Sample.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult Index(string Filterby = "All")
        {
            if (Request.IsAuthenticated)
            {
                var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

                //You get the user's first and last name below:
                ViewBag.Name = userClaims?.FindFirst("name")?.Value;
                ViewBag.UserGuid = userClaims?.FindFirst("aud")?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = userClaims?.FindFirst("preferred_username")?.Value;

                // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
                ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // TenantId is the unique Tenant Id - which represents an organization in Azure AD
                ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
                //Session["UserID"]= userClaims?.FindFirst("aud")?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            List<ProductListView> lst = new List<ProductListView>();
            //string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
            string GetAllProductAPIURL = "https://api-prod.producer.metaltrail.com/v1/products?category=active&offset=0&count=100";
            var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", "eyJraWQiOiJHVWlKaWJoV2lYemtBOVwvV1c5Y25kN2NUcFJQWmZMaXVQRHV1WXRsTEtJUT0iLCJhbGciOiJSUzI1NiJ9.eyJjdXN0b206b3JnYW5pemF0aW9uIjoiU3RlZWwgQ28uIiwic3ViIjoiYmRhNTM5MTMtYTllMS00N2NiLTkyMTctOTU4MzMzNjgwZTU0IiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0yX1ZYcjNXbGQ3diIsInBob25lX251bWJlcl92ZXJpZmllZCI6ZmFsc2UsImNvZ25pdG86dXNlcm5hbWUiOiJiZGE1MzkxMy1hOWUxLTQ3Y2ItOTIxNy05NTgzMzM2ODBlNTQiLCJnaXZlbl9uYW1lIjoiQ29yZXkiLCJvcmlnaW5fanRpIjoiMGJkMDg3ZGItMmMyNS00ZTlmLTg0MzMtMTJhYmEyOTQ4ZjY3IiwiYXVkIjoiNXIzZmFraGVmMmlmNjNwaDNzMG1vbW9wZCIsImV2ZW50X2lkIjoiMDFlZDI5YzctMjRhYy00ZDk3LWE5NDUtNTM5YzQ0N2RlZmY2IiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2Mzg3MjExNjEsImV4cCI6MTYzODcyNDc2MSwiY3VzdG9tOnJvbGUiOiJnZW5lcmFsIiwiaWF0IjoxNjM4NzIxMTYxLCJmYW1pbHlfbmFtZSI6IkdyYXZlbGxlIiwianRpIjoiNmNiY2ZhOGYtNzlmMy00MDJhLWE1NTktN2RmYTRiODQzZmYyIiwiZW1haWwiOiJjb3JleS5ncmF2ZWxsZUBhcmNlbG9ybWl0dGFsLmNvbSJ9.hfiP0ce-tg4J4v55DinWyNcp0iNg_iaFXKPmnjcGx-8Z71zjM5bSJ4UDp1_G95KiI5wY_nZxa3ro2lYwf1PiaKRH_eRULnoBf_ZMaGPhnYqGzKAMEePfL8_Hh0CHktInH37PrUD9ddJLxVLJZ3umXSp3xlW2igHLxHcmOIAS-eSU7J9GPlZFQEMAZfzPZbGwiFeNsZXtix-b80LFKRvW75iWtfZA-1qfesRuwRCve4ipDumjIfQmn9XEa1t8nvWb-zXfyaIoScy2QYHo0WTG-XGcKC1DY8FNGe9DxRtdWWHQLGKyuEDyBkbw4OciH3XCIGwpwxum8F7GE86mE3hx4Q");
            var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
            if (Allproductresponse != null)
            {

                foreach (var activeproduct in Allproductresponse.products?.active)
                {
                    ProductListView obj = new ProductListView();
                    obj.Owner = activeproduct.owner?.name;
                    obj.id = activeproduct.id;
                    obj.CreatedOn = activeproduct.createdAt;
                    obj.ProductType = activeproduct?.productVC?.credentialSubject?.product?.name;
                    obj.Origin = activeproduct.origin?.address?.addressLocality + "," + activeproduct.origin?.address?.addressRegion + "," + activeproduct.origin?.address?.addressCountry;
                    obj.Status = activeproduct.status;
                    obj.LastEvent = activeproduct.events?.OrderByDescending(p => p.createdAt).FirstOrDefault()?.eventType + " Product";
                    lst.Add(obj);
                }
            }
            if (Filterby != "All")
            {
                if (Filterby == "Day")
                {
                    lst = lst.Where(p => p.CreatedOn >= DateTime.Now.Date.AddHours(-24)).ToList();
                }
                if (Filterby == "Week")
                {
                    lst = lst.Where(p => p.CreatedOn >= DateTime.Now.Date.AddDays(-7)).ToList();
                }
                if (Filterby == "Month")
                {
                    lst = lst.Where(p => p.CreatedOn >= DateTime.Now.Date.AddMonths(-1)).ToList();
                }
            }
            ViewBag.Filterby = Filterby;
            //if (!string.IsNullOrEmpty(model.id))
            //{
            //    string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/";
            //    var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL + model.id, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");
            //    var res = JsonConvert.DeserializeObject<AllProductResponse>(objResponse.ResponseString);
            //    if (res != null)
            //    {
            //        //if (!string.IsNullOrEmpty(res.message))
            //        //{
            //        //    TempData["Error"] = res.message;
            //        //}
            //        //else
            //        //{
            //        //    model.result = new ProductResult();
            //        //    //model.result.HSCode=res.
            //        //}
            //    }
            //}
            return View(lst);
        }

        [HttpGet]
        public ActionResult _Detail(string id)
        {
            if (Request.IsAuthenticated)
            {
                var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

                //You get the user's first and last name below:
                ViewBag.Name = userClaims?.FindFirst("name")?.Value;
                ViewBag.UserGuid = userClaims?.FindFirst("aud")?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = userClaims?.FindFirst("preferred_username")?.Value;

                // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
                ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // TenantId is the unique Tenant Id - which represents an organization in Azure AD
                ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
                //Session["UserID"]= userClaims?.FindFirst("aud")?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            ProductDetailView model = new ProductDetailView();
            //string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/dc82c0ec-7b66-41dc-a9ba-2bb1c2f9ea5";
            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");

            string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/";
            var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL + id, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");

            model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);

            return View(model);
        }
    }
}