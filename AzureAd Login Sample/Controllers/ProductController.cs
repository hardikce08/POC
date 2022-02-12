using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using POC.Controllers;
using POC.DataAccess;
using POC.DataAccess.Model;
using POC.Helper;
using POC.Helpers;

namespace AzureAd_Login_Sample.Controllers
{
    public class ProductController : BaseController
    {
       
        //[OutputCache(Duration = 60, VaryByParam = "Filterby", Location = OutputCacheLocation.Client)]
        public ActionResult Index(string Filterby = "All")
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
            List<ProductListView> lst = new List<ProductListView>();
            //string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
            string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
            var Allproductresponse = GetDataFromCache<AllProductResponse>("AllProductResponse", GetAllProductAPIURL);
            if (Allproductresponse != null)
            {
                DashboardService ds = new DashboardService();
                var LstVc = ds.PieceInfos.Where(p => p.VCId != null).Distinct().ToList();

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
                        obj.Coil = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.MES_PCE_IDENT_NO.ToString();
                        obj.SerialNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.PCE_DISPLAY_NO.ToString();
                        obj.LiftNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LIFT_NO.ToString();
                        obj.CurrentLocation = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LOC_CD.ToString(); 
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
        public ActionResult IndexNew(string Filterby = "All")
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
            ViewBag.TotalRecords = 10;
            return View();
        }
        public ActionResult _ProductList(string Filterby = "All")
        {
            List<ProductListView> lst = new List<ProductListView>();
            //string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
            string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
            var Allproductresponse = GetDataFromCache<AllProductResponse>("AllProductResponse", GetAllProductAPIURL);
            if (Allproductresponse != null)
            {
                DashboardService ds = new DashboardService();
                var LstVc = ds.PieceInfos.Where(p => p.VCId != null).Distinct().ToList();

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
                        obj.Coil = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.MES_PCE_IDENT_NO.ToString();
                        obj.SerialNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.PCE_DISPLAY_NO.ToString();
                        obj.LiftNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LIFT_NO.ToString();
                        obj.CurrentLocation = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LOC_CD.ToString();
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
             
            return PartialView(lst);
        }

        public ActionResult GetData(JqueryDatatableParam param, string Filterby = "All", string TotalRecords = "10", string CurrentPage = "0")
        {
            List<ProductListView> lst = new List<ProductListView>();
            int RecordCount = 10;
            int offset = 0;
            if (CurrentPage != "0")
            {
                //var pageindex = (Convert.ToInt32(CurrentPage) + 1).ToString();
                var a = param.iDisplayLength * Convert.ToInt32(CurrentPage);
                if ((Convert.ToInt32(TotalRecords) - a) < RecordCount)
                {
                    RecordCount = Convert.ToInt32(TotalRecords) - a;
                }
            }
            string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset="+ (Convert.ToInt32(CurrentPage)).ToString()+ "&count="+ RecordCount.ToString();
            //string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active";
            var Allproductresponse = GetDataFromCache<AllProductResponse>("ProductPage:"+ (Convert.ToInt32(CurrentPage) + 1).ToString(), GetAllProductAPIURL);
            if (Allproductresponse != null)
            {
                DashboardService ds = new DashboardService();
                var LstVc = ds.PieceInfos.Where(p => p.VCId != null).Distinct().ToList();

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
                        obj.Coil = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.MES_PCE_IDENT_NO.ToString();
                        obj.SerialNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.PCE_DISPLAY_NO.ToString();
                        obj.LiftNumber = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LIFT_NO.ToString();
                        obj.CurrentLocation = LstVc?.Where(p => p.VCId == activeproduct.id).FirstOrDefault()?.LOC_CD.ToString();
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

            //var employees = lst;

            lst.ForEach(x => x.CreatedOnString = x.CreatedOn.ToString("MMM dd,yyyy"));

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                lst = lst.Where(x => x.Owner.ToLower().Contains(param.sSearch.ToLower())
                                              || x.Coil.ToLower().Contains(param.sSearch.ToLower())
                                              || x.id.ToLower().Contains(param.sSearch.ToLower())
                                              || x.Owner.ToString().Contains(param.sSearch.ToLower())
                                              || x.ProductType.ToString().Contains(param.sSearch.ToLower())
                                               || x.Origin.ToString().Contains(param.sSearch.ToLower())
                                                || x.LastEvent.ToString().Contains(param.sSearch.ToLower())
                                              || x.CreatedOn.ToString("MMM dd,yyyy").ToLower().Contains(param.sSearch.ToLower())).ToList();
            }

            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];

            if (sortColumnIndex == 3)
            {
                lst = sortDirection == "asc" ? lst.OrderBy(c => c.ProductType).ToList() : lst.OrderByDescending(c => c.ProductType).ToList();
            }
            //else if (sortColumnIndex == 4)
            //{
            //    employees = sortDirection == "asc" ? employees.OrderBy(c => c.StartDate) : employees.OrderByDescending(c => c.StartDate);
            //}
            //else if (sortColumnIndex == 5)
            //{
            //    employees = sortDirection == "asc" ? employees.OrderBy(c => c.Salary) : employees.OrderByDescending(c => c.Salary);
            //}
            //else
            //{
            //    Func<Employee, string> orderingFunction = e => sortColumnIndex == 0 ? e.Name :
            //                                                   sortColumnIndex == 1 ? e.Position :
            //                                                   e.Location;

            //    employees = sortDirection == "asc" ? employees.OrderBy(orderingFunction) : employees.OrderByDescending(orderingFunction);
            //}

            //var displayResult = lst.Skip(param.iDisplayStart)
            //    .Take(param.iDisplayLength).ToList();
            var displayResult = lst;
            var totalRecords = Allproductresponse.totalProductsCount?.active;
            ViewBag.TotalRecords = Allproductresponse.totalProductsCount?.active;
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }
        //private List<TEntity> GetFromCache<TEntity>(string key, Func<List<TEntity>> valueFactory) where TEntity : class
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    var newValue = new Lazy<List<TEntity>>(valueFactory);
        //    CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };
        //    //The line below returns existing item or adds the new value if it doesn't exist
        //    var value = cache.AddOrGetExisting(key, newValue, policy) as Lazy<List<TEntity>>;
        //    return (value ?? newValue).Value; // Lazy<T> handles the locking itself
        //}
        //private TEntity GetFromCache<TEntity>(string key, Func<TEntity> valueFactory) where TEntity : class
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    var newValue = new Lazy<TEntity>(valueFactory);
        //    CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };
        //    //The line below returns existing item or adds the new value if it doesn't exist
        //    var value = cache.AddOrGetExisting(key, newValue, policy) as Lazy<TEntity>;
        //    return (value ?? newValue).Value; // Lazy<T> handles the locking itself
        //}
        [HttpGet]
        public ActionResult _Detail(string id)
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
            ProductDetailView model = new ProductDetailView();
            //string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/dc82c0ec-7b66-41dc-a9ba-2bb1c2f9ea5";
            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");

            //string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/" + id;
            string APIURL = ApiDomain + "/v1/products/" + id;
            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            // model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            model.result = GetDataFromCache<Active>("Product:"+id, APIURL);
            DashboardService ds = new DashboardService();
            var vcInfo = ds.PieceInfos.Where(p => p.VCId == id).FirstOrDefault();
            ViewBag.Coil = vcInfo?.MES_PCE_IDENT_NO.ToString();
            ViewBag.SerialNumber = vcInfo?.PCE_DISPLAY_NO.ToString();
            ViewBag.LiftNumber = vcInfo?.LIFT_NO.ToString();
            ViewBag.CurrentLocation = vcInfo?.LOC_CD.ToString();
            var _events = new List<String> { "Raw Material Supply", "Transportation of Raw Materials", "Manufacturing(Selection from Scrap Yard,Electric Arc Furnace,Blast Furnace,Ladle Furnace,Continuous Casting)", "Transportation", "Downstream Processing(Rolling,Finishing,Fabrication)" };
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
            data.carbonFootprintDetails.processEmission.processMaterialsDetails.Add(new ProcessMaterialsDetail { processMaterial = "Dolomite", amount = new Amount { type = new List<string> { "MeasuredValue" }, unitCode = "Kg", value = "25" } });
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
            data.carbonFootprintDetails.mobileCombustionUsage.fuelUsage = new FuelUsage { type = new List<string> { "MeasuredValue" }, unitCode = "Gallon", value = "23" };

            data.carbonFootprintDetails.mobileCombustionDistance = new MobileCombustionDistance();
            data.carbonFootprintDetails.mobileCombustionDistance.co2eEmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.co2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.ch4EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.no2EmissionsInTonnes = 0;
            data.carbonFootprintDetails.mobileCombustionDistance.distanceDetails = new List<DistanceDetail>();
            data.carbonFootprintDetails.mobileCombustionDistance.distanceDetails.Add(new DistanceDetail { vehicleType = "Gasoline Passenger Cars", fuelType = "Motor Gasoline", unitCode = "Mile", value = "110" });

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

public class JqueryDatatableParam
{
    public string sEcho { get; set; }
    public string sSearch { get; set; }
    public int iDisplayLength { get; set; }
    public int iDisplayStart { get; set; }
    public int iColumns { get; set; }
    public int iSortCol_0 { get; set; }
    public string sSortDir_0 { get; set; }
    public int iSortingCols { get; set; }
    public string sColumns { get; set; }

    public string Filter { get; set; }
    public string name { get; set; }
}