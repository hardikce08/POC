﻿using System;
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
    public class ProductController : BaseController
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
            string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
            var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
            if (Allproductresponse != null)
            {
                if (Allproductresponse.statusCode == 200 || Allproductresponse.statusCode == 0)
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
                else
                {
                    TempData["Error"] = "Get All Product List API Error:" + Allproductresponse.message;
                    //throw new System.Exception("Get All Product List API Error:" + Allproductresponse.message);
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

            //string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/" + id;
            string APIURL= ApiDomain + "/v1/products/" + id;
            var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);

            model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            var _events = new List<String> { "Raw Material Supply","Transportation of Raw Materials","Manufacturing(Selection from Scrap Yard,Electric Arc Furnace,Blast Furnace,Ladle Furnace,Continuous Casting)","Transportation","Downstream Processing(Rolling,Finishing,Fabrication)" };
            ViewBag.Events = new SelectList(_events);
            ViewBag.ProductId = id;
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveEmissionEvent(string serializeddata)
        {
            EmissionEventRequest data = new EmissionEventRequest();
            var model = JsonConvert.DeserializeObject<SaveEmissionRequest>(serializeddata);
            data.productId = model.productid;
            data.carbonFootprintDetails = new EmissionCarbonFootprintDetails();
            data.carbonFootprintDetails.startDate = Convert.ToDateTime(model.startDate);
            data.carbonFootprintDetails.endDate = Convert.ToDateTime(model.endDate);
            data.carbonFootprintDetails.role = "Steel Co.";
            data.carbonFootprintDetails.processEmission = new ProcessEmission();
            data.carbonFootprintDetails.processEmission.co2EmissionsInTonnes = 55;
            data.carbonFootprintDetails.processEmission.co2eEmissionsInTonnes = 55;
            data.carbonFootprintDetails.processEmission.processMaterialsDetails = new List<ProcessMaterialsDetail>();
            data.carbonFootprintDetails.processEmission.processMaterialsDetails.Add(new ProcessMaterialsDetail { processMaterial= "Dolomite",amount= new Amount { type= new List<string> { "MeasuredValue" },unitCode="Kg",value="25" } });
            data.carbonFootprintDetails.stationaryCombustion = new StationaryCombustion();
            data.carbonFootprintDetails.stationaryCombustion.co2eEmissionsInTonnes = 0;
            data.carbonFootprintDetails.stationaryCombustion.co2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.stationaryCombustion.ch4EmissionsInTonnes = 0;
            data.carbonFootprintDetails.stationaryCombustion.no2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.stationaryCombustion.fuelTypesDetails = new List<FuelTypesDetail>();
            data.carbonFootprintDetails.stationaryCombustion.fuelTypesDetails.Add(new FuelTypesDetail { fuelType = "Lignite Coal", fuelUsage = new FuelUsage { type = new List<string> { "MeasuredValue" }, unitCode = "mmBtu", value = "30" } });

            data.carbonFootprintDetails.mobileCombustionUsage = new MobileCombustionUsage();
            data.carbonFootprintDetails.mobileCombustionUsage.co2eEmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionUsage.co2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionUsage.ch4EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionUsage.no2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionUsage.fuelType = "string";
           data.carbonFootprintDetails.mobileCombustionUsage.fuelUsage=  new FuelUsage { type = new List<string> { "MeasuredValue" }, unitCode = "Gallon", value = "23" };

            data.carbonFootprintDetails.mobileCombustionDistance = new MobileCombustionDistance();
            data.carbonFootprintDetails.mobileCombustionDistance.co2eEmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.co2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.ch4EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.no2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.distanceDetails = new List<DistanceDetail>();
            data.carbonFootprintDetails.mobileCombustionDistance.distanceDetails.Add(new DistanceDetail {vehicleType= "Gasoline Passenger Cars", fuelType = "Motor Gasoline", unitCode = "Mile", value = "110" });

            data.carbonFootprintDetails.purchasedElectricity = new PurchasedElectricity();
            data.carbonFootprintDetails.purchasedElectricity.co2eEmissionsInTonnes = 0;
            data.carbonFootprintDetails.purchasedElectricity.co2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.purchasedElectricity.subregion = "Ontario";
            data.carbonFootprintDetails.purchasedElectricity.electricityConsumption = new ElectricityConsumption { type = new List<string> { "MeasuredValue" }, unitCode = "kwh", value = "55" };

            data.carbonFootprintDetails.carbonFootprintEvents = new EmissionCarbonFootprintEvents();
            data.carbonFootprintDetails.carbonFootprintEvents.co2eEmissionsInTonnes = Convert.ToInt32(model.co2emissionintonnes);
            data.carbonFootprintDetails.carbonFootprintEvents.events = new List<string> { model.Event };
            var postString = JsonConvert.SerializeObject(data);
            string CarbonFootprintAPIURL = ApiDomain + "/v1/products/carbonfootprint";
            var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(CarbonFootprintAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);

            return Json(new { ResponseCode = objResponse.ResponseCode }, JsonRequestBehavior.AllowGet);
        }
    }
}