using POC.Models;
using Hangfire;
using Newtonsoft.Json;
using POC.Controllers;
using POC.DataAccess;
using POC.DataAccess.Model;
using POC.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AzureAd_Login_Sample.Controllers
{
    public class AdminController : BaseController
    {
        public int TotalActiveProducts,History,SharedWithMe,Consumed = 0;
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/HangfireJobs
        public ActionResult HangfireJobs()
        {
            HttpCookieCollection cookies = System.Web.HttpContext.Current.Request.Cookies;
            if (cookies["UserToken"] != null)
            {
                UserService us = new UserService();
                bool HasAccess= us.HasUserHangfireAccess(cookies["UserEmail"].Value);
                if (HasAccess == false)
                {
                    return Redirect("/Error/NotFound");
                }
            }
            return View(new POC.Models.HangFireJobsViewModel());
        }

        [HttpPost]
        public ActionResult HangfireJobs(List<int> selected)
        {
            if (selected != null && selected.Any())
            {
                foreach (var item in selected)
                {
                    // minutes, hours, days, months, days of week
                    switch ((HangFireJobsViewModel.JobEnum)item)
                    {
                        case HangFireJobsViewModel.JobEnum.LoadProductData:
                            RecurringJob.AddOrUpdate<AdminController>("Load All Products", ss => ss.LoadProducts(), Cron.Daily(2));
                            break;
                            //case HangFireJobsViewModel.JobEnum.SendEmail:
                            //    RecurringJob.AddOrUpdate<Functions>("Send Email", fs => fs.SendAlertNotificationAsync(), Cron.Hourly());
                            //    break;
                    }
                }

                return RedirectToAction("recurring", "hangfire", new { area = "" });
            }


            return Content("Completed");
        }

        public void LoadProducts()
        {
            DashboardService dss = new DashboardService();
            int TotalCount = dss.ProductsListfromDB().Count;
            int RecordCount = 20;
            InsertProducts(0, 20, TotalCount > 0 ? true : false);
            dss.InsertProductSummaryData(TotalActiveProducts, Consumed, History, SharedWithMe);
            if (TotalActiveProducts > RecordCount)
            {
                decimal TotalPages = Math.Ceiling(Convert.ToDecimal(TotalActiveProducts) / Convert.ToDecimal(RecordCount));
                for (int i = 1; i <= TotalPages - 1; i++)
                {
                    InsertProducts((i * RecordCount), 20, TotalCount > 0 ? true : false);
                }
            }
        }
        public void InsertProducts(int offset, int RecordCount, bool ApplyDateFilter)
        {
            List<ProductListViewV2> lst = new List<ProductListViewV2>();
            DashboardService dss = new DashboardService();

            string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=" + (offset.ToString()) + "&count=" + RecordCount.ToString();
            var allproductresponse = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(allproductresponse.ResponseString);
            if (Allproductresponse != null)
            {
                if (Allproductresponse.statusCode == 200 || Allproductresponse.statusCode == 0)
                {
                    TotalActiveProducts = (int)Allproductresponse.totalProductsCount?.active;
                    Consumed = (int)Allproductresponse.totalProductsCount?.consumed;
                    History = (int)Allproductresponse.totalProductsCount?.history;
                    SharedWithMe = (int)Allproductresponse.totalProductsCount?.sharedWithMe;

                    var ActilveProductsList = Allproductresponse.products?.active;
                    
                    if (ActilveProductsList != null)
                    {
                        if (ApplyDateFilter)
                        {
                            DateTime LastPRoductCreatedDate = (DateTime)dss.DBProducts.Max(p => p.CreatedOn);
                            if (LastPRoductCreatedDate != null)
                            {
                                ActilveProductsList = ActilveProductsList.Where(p => p.createdAt >= LastPRoductCreatedDate).ToList();
                            }
                        }
                        foreach (var activeproduct in ActilveProductsList)
                        {
                            var credentialsubject = activeproduct.productVC.credentialSubject;
                            ProductListViewV2 obj = new ProductListViewV2();
                            obj.Owner = activeproduct.owner?.name;
                            obj.id = activeproduct.id;
                            obj.CreatedOn = activeproduct.createdAt;
                            obj.ProductType = credentialsubject?.product?.name;
                            obj.Origin = activeproduct.origin?.address?.addressLocality + "," + activeproduct.origin?.address?.addressRegion + "," + activeproduct.origin?.address?.addressCountry;
                            obj.Status = activeproduct.status;
                            obj.LastEvent = activeproduct.events?.OrderByDescending(p => p.createdAt).FirstOrDefault()?.eventType + " Product";
                            obj.HsCode = credentialsubject?.HSCode;
                            obj.IssuanceDate = activeproduct.productVC.issuanceDate;
                            obj.ProductionDate = Convert.ToDateTime(credentialsubject?.productionDate);
                            obj.TechnologyType = credentialsubject?.technologyType;
                            //obj.Coil = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.MES_PCE_IDENT_NO.ToString();
                            //obj.SerialNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.PCE_DISPLAY_NO.ToString();
                            //obj.LiftNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LIFT_NO.ToString();
                            //obj.CurrentLocation = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LOC_CD.ToString();
                            lst.Add(obj);
                        }

                        if (lst.Count > 0)
                        {
                            DataTable dt = lst.ToDataTable<ProductListViewV2>();

                            dss.InsertProductData(dt);
                        }
                    }
                }
            }

        }
    }
}