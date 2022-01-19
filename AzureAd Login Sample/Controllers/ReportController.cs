﻿using Newtonsoft.Json;
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
        public ActionResult GetTimeLineData(string Filterdays = "3")
        {
            int days = Convert.ToInt32(Filterdays);
            List<object> iData = new List<object>();
            List<string> labels = new List<string>();
            List<int> data = new List<int>();
            List<Event> lstEvents = new List<Event>();

            List<AllPRoductEventsLineGraph> AllProductEvents = new List<AllPRoductEventsLineGraph>();
            DateTime EndDate = DateTime.Now, StartDate = DateTime.Now;
            if (days > 0)
            {
                if (days == 3)
                {
                    StartDate = EndDate.AddDays(-3);
                }
                else if (days == 7)
                {
                    StartDate = EndDate.AddDays(-7);
                }
                else if (days == 30)
                {
                    StartDate = EndDate.AddMonths(-1);
                }
                else if (days == 90)
                {
                    StartDate = EndDate.AddMonths(-3);
                }
            }

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
                    AllProductEvents.Add(new AllPRoductEventsLineGraph { Count = 1, Status = objevent.eventType, FilterDate = objevent.eventVC.issuanceDate.Date, IssuedDate = objevent.eventVC.issuanceDate, CreatedDate = objevent.eventVC.issuanceDate.ToString("yyyy-MM-dd") });
                }
                if (AllProductEvents.Count > 0)
                {
                    //ViewBag.TransportStartCount = AllProductEvents.Where(p => p.Status == "Create").ToList();
                    AllProductEvents = AllProductEvents.Where(c => c.FilterDate >= StartDate.Date && c.FilterDate <= EndDate.Date).ToList();
                    labels = AllProductEvents.OrderBy(c => c.IssuedDate).Select(p => p.CreatedDate).Distinct().ToList();
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
        [HttpPost]
        public ActionResult GetBarGraphData(string Filterdays = "3")
        {
            int days = Convert.ToInt32(Filterdays == "" ? "3" : Filterdays);
            List<object> iData = new List<object>();
            List<string> labels = new List<string>();
            List<int> data = new List<int>();
            List<Event> lstEvents = new List<Event>();
            List<Active> lstActiveProducts = new List<Active>();
            List<AllProductBarGraph> AllProductBarGraphdata = new List<AllProductBarGraph>();
            DateTime EndDate = DateTime.Now, StartDate = DateTime.Now;
            if (days > 0)
            {
                if (days == 3)
                {
                    StartDate = EndDate.AddDays(-3);
                    for (int i = 0; i < 3; i++)
                    {
                        labels.Add(StartDate.AddDays(i).Date.ToString("yyyy-MM-dd"));
                    }
                }
                else if (days == 7)
                {
                    StartDate = EndDate.AddDays(-7);
                    for (int i = 0; i < 7; i++)
                    {
                        labels.Add(StartDate.AddDays(i).Date.ToString("yyyy-MM-dd"));
                    }
                }
                else if (days == 30)
                {
                    StartDate = EndDate.AddMonths(-1);
                    for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                    {
                        labels.Add(StartDate.AddDays(i).Date.ToString("yyyy-MM-dd"));
                    }
                }
                else if (days == 90)
                {
                    StartDate = EndDate.AddMonths(-3);
                    for (int i = 0; i < 3; i++)
                    {
                        labels.Add(StartDate.AddMonths(i).Date.ToString("MMM"));
                    }
                }
            }
            iData.Add(labels);
            if (TempData["AllProducts"] != null)
            {
                lstActiveProducts = (List<Active>)TempData["AllProducts"];
            }
            else
            {
                var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
                if (Allproductresponse != null)
                {
                    lstActiveProducts = Allproductresponse.products?.active;
                    TempData["AllProducts"] = Allproductresponse.products?.active;
                }
                lstEvents = new List<Event>();
                if (Allproductresponse.products?.active != null)
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
            lstActiveProducts = lstActiveProducts.Where(c => c.productVC.issuanceDate.Date >= StartDate.Date && c.productVC.issuanceDate.Date <= EndDate.Date).ToList();

            foreach (var objproduct in lstActiveProducts)
            {
                AllProductBarGraphdata.Add(new AllProductBarGraph { Count = 1, Status = objproduct.status, FilterDate = objproduct.productVC.issuanceDate.Date, IssuedDate = objproduct.productVC.issuanceDate, CreatedDate = days == 90 ? objproduct.productVC.issuanceDate.ToString("MMM") : objproduct.productVC.issuanceDate.ToString("yyyy-MM-dd") });
            }
            if (AllProductBarGraphdata.Count > 0)
            {
                foreach (var datestr in labels)
                {
                    data.Add(AllProductBarGraphdata.Where(p => p.CreatedDate == datestr).Sum(p => p.Count));
                }

            }
            else
            {
                foreach (var datestr in labels)
                {
                    data.Add(0);
                }
            }
            iData.Add(data);
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
                var AllProductEvents = new List<AllPRoductEventsLineGraph>();
                foreach (var objevent in lstEvents)
                {
                    AllProductEvents.Add(new AllPRoductEventsLineGraph { Count = 1, Status = objevent.eventType, FilterDate = objevent.eventVC.issuanceDate.Date, IssuedDate = objevent.eventVC.issuanceDate, CreatedDate = days == 90 ? objevent.eventVC.issuanceDate.ToString("MMM") : objevent.eventVC.issuanceDate.ToString("yyyy-MM-dd") });
                }
                data = new List<int>();
                if (AllProductEvents.Count > 0)
                {
                    //filter events for selected filter criteria
                    AllProductEvents = AllProductEvents.Where(c => c.FilterDate >= StartDate.Date && c.FilterDate <= EndDate.Date).ToList();
                    //get only those event count whose status is TransportStart 

                    foreach (var datestr in labels)
                    {
                        //data.Add(AllProductEvents.Where(p => p.CreatedDate == datestr && p.Status == "TransportStart").Sum(p => p.Count));
                        data.Add(AllProductEvents.Where(p => p.CreatedDate == datestr && p.Status == "Create").Sum(p => p.Count));
                    }

                }
                else
                {
                    foreach (var datestr in labels)
                    {
                        //data.Add(AllProductEvents.Where(p => p.CreatedDate == datestr && p.Status == "TransportStart").Sum(p => p.Count));
                        data.Add(0);
                    }
                }
                iData.Add(data);
            }



            return new JsonResult { Data = new { categories = JsonConvert.SerializeObject(iData[0]), ProductVCIssuesCount = JsonConvert.SerializeObject(iData[1]), TransportStartEventCount = JsonConvert.SerializeObject(iData[2]) } };
        }
        [HttpPost]
        public ActionResult UpdatePercentage(string Filterdays = "3")
        {
            int days = Convert.ToInt32(Filterdays == "" ? "3" : Filterdays);
            List<object> iData = new List<object>();
            int percentage = 0;
            List<string> labels = new List<string>();
            List<int> data = new List<int>();
            List<Event> lstEvents = new List<Event>();
            List<AllPRoductEventsLineGraph> AllProductEvents = new List<AllPRoductEventsLineGraph>();
            DateTime EndDate = DateTime.Now, StartDate = DateTime.Now;
            if (days > 0)
            {
                if (days == 3)
                {
                    StartDate = EndDate.AddDays(-3);
                    for (int i = 0; i < 3; i++)
                    {
                        labels.Add(StartDate.AddDays(i).Date.ToString("yyyy-MM-dd"));
                    }
                }
                else if (days == 7)
                {
                    StartDate = EndDate.AddDays(-7);
                    for (int i = 0; i < 7; i++)
                    {
                        labels.Add(StartDate.AddDays(i).Date.ToString("yyyy-MM-dd"));
                    }
                }
                else if (days == 30)
                {
                    StartDate = EndDate.AddMonths(-1);
                    for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                    {
                        labels.Add(StartDate.AddDays(i).Date.ToString("yyyy-MM-dd"));
                    }
                }
                else if (days == 90)
                {
                    StartDate = EndDate.AddMonths(-3);
                    for (int i = 0; i < 3; i++)
                    {
                        labels.Add(StartDate.AddMonths(i).Date.ToString("MMM"));
                    }
                }
            }


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
                int TotalEventsCount = lstEvents.Count;
                foreach (var objevent in lstEvents)
                {
                    AllProductEvents.Add(new AllPRoductEventsLineGraph { Count = 1, Status = objevent.eventType, FilterDate = objevent.eventVC.issuanceDate.Date, IssuedDate = objevent.eventVC.issuanceDate, CreatedDate = objevent.eventVC.issuanceDate.ToString("yyyy-MM-dd") });
                }
                if (AllProductEvents.Count > 0)
                {
                    AllProductEvents = AllProductEvents.Where(c => c.FilterDate >= StartDate.Date && c.FilterDate <= EndDate.Date).ToList();
                    AllProductEvents = AllProductEvents.Where(p => p.Status == "TransportStart" || p.Status == "TransferCustody").ToList();

                    percentage = AllProductEvents.Count == 0 ? 0 : (TotalEventsCount / AllProductEvents.Count) * 100;
                    ViewBag.TransportStartCount = percentage.ToString();
                }
            }




            return new JsonResult { Data = new { d = JsonConvert.SerializeObject(percentage) } };
        }
    }
}