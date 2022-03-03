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
        DashboardService ds = new DashboardService();
        public int TotalActiveProducts, History, SharedWithMe, Consumed = 0;
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
                bool HasAccess = us.HasUserHangfireAccess(cookies["UserEmail"].Value);
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
                        case HangFireJobsViewModel.JobEnum.LoadVCandCreateProduct:
                            RecurringJob.AddOrUpdate<AdminController>("Load VC and Create Products", ss => ss.CreateNewVC(), Cron.HourInterval(2));
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
            //DashboardService dss = new DashboardService();
            int TotalCount = ds.ProductsListfromDB().Count;
            int RecordCount = 20;
            InsertProducts(0, 20, TotalCount > 0 ? true : false);
            ds.InsertProductSummaryData(TotalActiveProducts, Consumed, History, SharedWithMe);
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

        public void CreateNewVC()
        {
            // Load All CoilId whose VCId is null to Identify its New Imported and Product creation is pending
            var lstcoil = ds.PieceInfos.Where(p => p.VCId == null).Select(c => c.MES_PCE_IDENT_NO);
            foreach (var coil in lstcoil)
            {
                try
                {
                    var info = ds.PieceInfos.Where(p => p.MES_PCE_IDENT_NO == coil).FirstOrDefault();
                    string APIURL = ApiDomain + "/v1/products";
                    VCRequest data = new VCRequest();
                    data.productName = "Carbon and alloy long product";
                    data.hsCode = ds.GetHSCode(info.MES_PCE_IDENT_NO)?.Trim();
                    data.facility = new VCFacility
                    {
                        addressLocality = "Hamilton",
                        addressRegion = "Ontario",
                        //addressLocality = "",
                        //addressRegion = "",
                        postalCode = "",
                        addressCountry = info.ORIGN_COUNTRY_CD_TEXT?.Trim(),
                        streetAddress = "",
                        latitude = "43.2557",
                        longitude = "-79.8711",
                        globalLocationNumber = info.LOC_CD
                    };
                    data.manufacturer = new VCManufacturer { name = "Steel Co." };
                    data.weight = new UnitofMeasure { unit = "KG", value = info.PCE_WT.ToString() };
                    data.length = new UnitofMeasure { unit = "CM", value = info.PCE_LGT.ToString() };
                    data.width = new UnitofMeasure { unit = "CM", value = info.PCE_WDT.ToString() };
                    data.technologyType = info.GRD_CD.Trim().StartsWith("EA") ? "ElectricArcFurnace" : "BlastFurnace";
                    data.observation = new List<VCObservation> { new VCObservation { description = "Aluminum", name = "aluminum", type = "ChemicalProperty", unit = "%", value = "0.05" } };
                    data.grade = info.GRD_CD.Trim();
                    data.heatNumber = info.HT_NO.ToString();
                    var postString = JsonConvert.SerializeObject(data);
                    //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "Authorization:Bearer " + token, "", "");
                    var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
                    var res = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
                    //var res = JsonConvert.DeserializeObject<VCResponse>(objResponse.ResponseString);
                    ViewBag.VCId = res.id;
                    if (res.id == null)
                    {
                        TempData["Error"] = "Error in Product API";//res.message;
                        ds.LogError(objResponse.ResponseString, TempData["Error"].ToString(), APIURL, postString, objResponse.ResponseCode.ToString(), coil);
                    }
                    else
                    {
                        //update VC number in Database from above result
                        ds.UpdateVCId(coil, res.id);
                        //var Allproductresponse = GetDataFromCache<AllProductResponse>("AllProductResponse", GetAllProductAPIURL);
                        double CoilWeight = info.PCE_WT;
                        var Co2EmissionCalculated = (CoilWeight * AnnualCo2value) / 1000;
                        ProductCarbonFootPrint eventdata = new ProductCarbonFootPrint();
                        eventdata.productId = res.id;
                        eventdata.carbonFootprintDetails = new CarbonFootprintDetailsNew();
                        eventdata.carbonFootprintDetails.startDate = Convert.ToDateTime(res.productVC.issuanceDate);
                        eventdata.carbonFootprintDetails.endDate = Convert.ToDateTime(res.productVC.issuanceDate);
                        eventdata.carbonFootprintDetails.role = "Steel Co.";
                        eventdata.carbonFootprintDetails.carbonFootprintEvents = new CarbonFootprintEventsNew();
                        eventdata.carbonFootprintDetails.carbonFootprintEvents.co2eEmissionsInTonnes = Convert.ToInt32(Co2EmissionCalculated);
                        eventdata.carbonFootprintDetails.carbonFootprintEvents.events = new List<EventNew>();
                        eventdata.carbonFootprintDetails.carbonFootprintEvents.events.Add(new EventNew { @event = "Manufacturing (Selection from Scrap Yard, Electric Arc Furnace, Blast Furnace, Ladle Furnace, Continuous Casting)", co2eProduced = new Co2eProducedNew { type = new List<string> { "MeasuredValue" }, unitCode = "Tonne", value = Co2EmissionCalculated.ToString() } });
                        postString = JsonConvert.SerializeObject(eventdata);
                        string CarbonFootprintAPIURL = ApiDomain + "/v1/products/carbonfootprint";
                        objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(CarbonFootprintAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
                        if (objResponse.ResponseCode != HttpStatusCode.Created)
                        {
                            TempData["Error"] = "Error in CarbonFoorPrint API Request for New VC: " + res.id;//res.message;
                            ds.LogError(objResponse.ResponseString, TempData["Error"].ToString(), CarbonFootprintAPIURL, postString, objResponse.ResponseCode.ToString(), coil);
                        }
                        // Insert Mill Test data at the time of Produt Creation
                        List<MillTestDataAPIRequestModel> MillTestValues = ds.GetCoilMillTestDatra(coil);
                        if (MillTestValues != null)
                        {
                            var product = res.productVC.credentialSubject.product;
                            var facility = res.productVC.credentialSubject.facility;
                            MillTestAPIRequest milltestdata = new MillTestAPIRequest();
                            milltestdata.productId = res.id;
                            milltestdata.certifier = "Dofasco";
                            milltestdata.manufacturer = new MillTestManufacturer { name = product.manufacturer.name };
                            milltestdata.manufacturer.address = new MillTestAddress { addressLocality = "Hamilton", addressRegion = "Ontario", addressCountry = info.LABEL_COUNTRY_CD_TEXT };
                            milltestdata.specification = "39.9276";
                            milltestdata.place = new MillTestPlace { addressCountry = info.LABEL_COUNTRY_CD_TEXT, addressLocality = "Hamilton", addressRegion = "Ontario", latitude = Convert.ToDouble("43.2557"), longitude = Convert.ToDouble("-79.8711") };
                            milltestdata.originalCountryOfMeltAndPour = info.LABEL_COUNTRY_CD_TEXT;
                            milltestdata.observation = new List<MillTestObservation>();
                            foreach (var item in MillTestValues)
                            {
                                if (!string.IsNullOrEmpty(item.ChemicalPropertyName))
                                {
                                    milltestdata.observation.Add(new MillTestObservation { description = item.CHEM_ELEM_CD, type = "ChemicalProperty", name = item.ChemicalPropertyName, value = item.CHEM_ELEM_PCT.ToString(), unit = "%" });
                                }
                            }
                            string CraeteMillTestAPIURL = ApiDomain + "/v1/products/millTest";
                            postString = JsonConvert.SerializeObject(data);
                            var SaveMillTestRequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteMillTestAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
                            if (SaveMillTestRequest.ResponseCode != HttpStatusCode.Created)
                            {
                                TempData["Error"] = "Error in MillTest API Submission Request for New VC: " + res.id;//res.message;
                                ds.LogError(objResponse.ResponseString, TempData["Error"].ToString(), CarbonFootprintAPIURL, postString, objResponse.ResponseCode.ToString(), coil);
                            }
                        }
                        // Get the Latest created Product using above VC and insert data in Product Master table
                        if (res != null)
                        {
                            var activeproduct = res;
                            var credentialsubject = res.productVC.credentialSubject;
                            List<ProductListViewV2> lst = new List<ProductListViewV2>();
                            ProductListViewV2 obj = new ProductListViewV2();
                            obj.Owner = res.owner?.name;
                            obj.id = res.id;
                            obj.CreatedOn = res.createdAt;
                            obj.ProductType = credentialsubject?.product?.name;
                            obj.Origin = res.origin?.address?.addressLocality + "," + res.origin?.address?.addressRegion + "," + res.origin?.address?.addressCountry;
                            obj.Status = res.status;
                            obj.LastEvent = res.events?.OrderByDescending(p => p.createdAt).FirstOrDefault()?.eventType + " Product";
                            obj.HsCode = credentialsubject?.HSCode;
                            obj.IssuanceDate = res.productVC.issuanceDate;
                            obj.ProductionDate = Convert.ToDateTime(credentialsubject?.productionDate);
                            obj.TechnologyType = credentialsubject?.technologyType;
                            //obj.Coil = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.MES_PCE_IDENT_NO.ToString();
                            //obj.SerialNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.PCE_DISPLAY_NO.ToString();
                            //obj.LiftNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LIFT_NO.ToString();
                            //obj.CurrentLocation = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LOC_CD.ToString();
                            lst.Add(obj);
                            if (lst.Count > 0)
                            {
                                DataTable dt = lst.ToDataTable<ProductListViewV2>();
                                ds.InsertProductData(dt);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ds.LogError("", ex.Message, "", "", "", coil);

                }

            }
        }
    }
}