using Newtonsoft.Json;
using POC.Controllers;
using POC.DataAccess.Model;
using POC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace AzureAd_Login_Sample.Controllers
{
    public class ReportController : BaseController
    {
        public string GetAllProductAPIURL { get; set; } = string.Empty;
        public ReportController()
        {
            GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
        }
        // GET: Report
        public ActionResult Index(ReportView model)
        {
            if (Request.Cookies["UserToken"] != null)
            {
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            //TempData["Message"] = "test";
            ViewBag.Page = "report";

            //string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
            //string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
            var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
            if (Allproductresponse != null)
            {
                model.Active = (int)Allproductresponse.totalProductsCount?.active;
                model.Consumed = (int)Allproductresponse.totalProductsCount?.consumed;
                model.History = (int)Allproductresponse.totalProductsCount?.history;
                model.SharedWithme = (int)Allproductresponse.totalProductsCount?.sharedWithMe;

                ViewBag.Active = model.Active;
                ViewBag.Consumed = model.Consumed;
                // ViewBag.History = model.History;
                ViewBag.SharedWithme = model.SharedWithme;
                List<Event> lstEvents = new List<Event>();
                if (Allproductresponse.products.active != null)
                {
                    foreach (var activeprod in Allproductresponse.products.active)
                    {
                        if (activeprod.events != null)
                        {
                            foreach (var objevent in activeprod.events)
                            {
                                lstEvents.Add(objevent);
                            }
                        }
                    }
                }
                TempData["Events"] = lstEvents;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetTimeLineData()
        {
            List<object> iData = new List<object>();
            List<string> labels = new List<string>();
            List<int> data = new List<int>();
            List<Event> lstEvents = new List<Event>();
            List<AllPRoductEventsLineGraph> AllProductEvents = new List<AllPRoductEventsLineGraph>();
            if (TempData["Events"] == null)
            {
                //string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
                var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
                if (Allproductresponse.products.active != null)
                {
                    foreach (var activeprod in Allproductresponse.products.active)
                    {
                        if (activeprod.events != null)
                        {
                            foreach (var objevent in activeprod.events)
                            {
                                lstEvents.Add(objevent);
                            }
                        }
                    }
                }
            }
            else
            {
                lstEvents = (List<Event>)TempData["Events"];
            }
            if (lstEvents.Count > 0)
            {
                foreach (var objevent in lstEvents)
                {
                    AllProductEvents.Add(new AllPRoductEventsLineGraph { Count = 1, IssuedDate = objevent.eventVC.issuanceDate, CreatedDate = objevent.eventVC.issuanceDate.ToString("yyyy-MM-dd") });
                }
                if (AllProductEvents.Count > 0)
                {
                    labels = AllProductEvents.OrderBy(c => c.IssuedDate).Select(p => p.CreatedDate).Distinct().Take(7).ToList();
                    iData.Add(labels);
                    foreach (var datestr in labels)
                    {
                        data.Add(AllProductEvents.Where(p => p.CreatedDate == datestr).Sum(p => p.Count));
                    }
                    iData.Add(data);
                }
            }

            //labels.Add("2021-11-05");
            //labels.Add("2021-11-06");
            //labels.Add("2021-11-07");
            //iData.Add(labels);

            //data.Add(1);
            //data.Add(2);
            //data.Add(3);
            //iData.Add(data);
            string jsonGraphdata = JsonConvert.SerializeObject(iData);
            return new JsonResult { Data = new { data = jsonGraphdata } };
        }
    }
}