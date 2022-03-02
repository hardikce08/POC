using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POC.DataAccess;
using POC.DataAccess.Model;
using POC.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace POC.Controllers
{
    public class HomeController : BaseController
    {
        public string GetAllProductAPIURL { get; set; } = string.Empty;
        public int TotalActiveProducts = 0;
        public HomeController()
        {
            GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
        }
        public ActionResult Index()
        {
            if (Request.Cookies["UserToken"] != null)
            {
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var parameters = new Dictionary<string, string>
                {
                    { "response_mode", "form_post" },
                     { "response_type", "token" },
                    { "client_id", System.Configuration.ConfigurationManager.AppSettings["ClientId"] },
                    { "redirect_uri", System.Configuration.ConfigurationManager.AppSettings["redirectUri"] },
                    { "prompt", "login"},
                    {"nonce","678910" },
                      {"state","12345" },
                    { "scope", "openid"}
                };

                var requestUrl = string.Format("{0}/authorize?{1}", EndPointUrl, BuildQueryString(parameters));

                Response.Redirect(requestUrl);
            }
            return View();

        }
        public ActionResult CallBack()
        {
            if (Request.Params["access_token"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(Request.Params["access_token"]);
                var userClaims = decodedValue.Payload;
                //ViewBag.Name = userClaims["name"]?.Value;
                //ViewBag.UserGuid = userClaims["aud"]?.Value;
                //ViewBag.Username = userClaims["unique_name"]?.Value;
                DateTime expiredate = DateTime.Now.AddHours(12);
                var UserToken = new HttpCookie("UserToken");
                UserToken.Expires = expiredate;
                UserToken.Value = Request.Params["access_token"].ToString();
                HttpContext.Response.Cookies.Add(UserToken);
                var UserEmail = new HttpCookie("UserEmail");
                UserEmail.Expires = expiredate;
                UserEmail.Value = userClaims["unique_name"]?.ToString();
                HttpContext.Response.Cookies.Add(UserEmail);
                var UserName = new HttpCookie("UserName");
                UserName.Expires = expiredate;
                UserName.Value = userClaims["name"]?.ToString();
                HttpContext.Response.Cookies.Add(UserName);
                var UserGuid = new HttpCookie("UserGuid");
                UserGuid.Expires = expiredate;
                UserGuid.Value = userClaims["aud"]?.ToString();
                HttpContext.Response.Cookies.Add(UserGuid);
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        public ActionResult Dashboard(DashBoardView model, string Id = "")
        {
            //int RecordCount = 20;
            //InsertProducts(0, 20);
            //if (TotalActiveProducts > RecordCount)
            //{
            //    decimal TotalPages = Math.Ceiling(Convert.ToDecimal(TotalActiveProducts) / Convert.ToDecimal(RecordCount));
            //    for (int i = 1; i <= TotalPages - 1; i++)
            //    {
            //        InsertProducts((i * RecordCount), 20);
            //    }
            //}

            if (Request.Cookies["UserToken"] != null)
            {
                //You get the user's first and last name below:
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            DashboardService ds = new DashboardService();
            ViewBag.WeightKg = "";
            ViewBag.Lengthcm = "";
            ViewBag.Widthcm = "";
            ViewBag.SelectedProductName = "Carbon and alloy long product";
            if (Request.HttpMethod == "POST" && model.Coil > 0)
            {
                var info = ds.PieceInfos.Where(p => p.MES_PCE_IDENT_NO == model.Coil).FirstOrDefault();
                if (info != null)
                {
                    ViewBag.SelectedCountry = info.ORIGN_COUNTRY_CD_TEXT?.Trim();
                    ViewBag.SelectedCountryofMeltPourlst = info.LABEL_COUNTRY_CD_TEXT?.Trim();
                    ViewBag.SelectedTypeofTechnology = info.FIELD_SHORT_DESC?.Trim();
                    ViewBag.SelectedGrade = info.GRD_CD?.Trim();
                    ViewBag.SelectedHSCode10Digits = ds.GetHSCode(info.MES_PCE_IDENT_NO)?.Trim();
                    ViewBag.SelectedGuage = info.PCE_IMP_THK.ToString().Trim();
                    ViewBag.WeightKg = (int)info.PCE_WT;
                    ViewBag.Lengthcm = info.PCE_LGT;
                    ViewBag.Widthcm = (int)info.PCE_WDT;
                    ViewBag.HeatNo = info.HT_NO;
                    ViewBag.Location = info.LOC_CD;
                    ViewBag.SelectedProductName = Request["ddlProductName"];
                }
            }
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.VCId = Id;
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var _Coil = ds.PieceInfos.Where(p => p.VCId == null).Select(p => p.MES_PCE_IDENT_NO).Distinct().ToList();
            ViewBag.Coil = new SelectList(_Coil);

            //var _CountryofOrigin = ds.PieceInfos.Select(p => p.ORIGN_COUNTRY_CD_TEXT).Distinct().ToList();
            //ViewBag.CountryofOrigin = PopulateDropdownListValues(_CountryofOrigin, ViewBag.SelectedCountry);
            //var _CountryofMeltPour = ds.PieceInfos.Select(p => p.LABEL_COUNTRY_CD_TEXT).Distinct().ToList();
            //ViewBag.CountryofMeltPour = PopulateDropdownListValues(_CountryofMeltPour, ViewBag.SelectedCountryofMeltPourlst);
            //var _TypeofTechnology = ds.PieceInfos.Select(p => p.FIELD_SHORT_DESC).Distinct().ToList();
            //ViewBag.TypeofTechnology = PopulateDropdownListValues(_TypeofTechnology, ViewBag.SelectedTypeofTechnology);
            //var _Grade = ds.PieceInfos.Select(p => p.GRD_CD).Distinct().ToList();
            //ViewBag.Grade = PopulateDropdownListValues(_Grade, ViewBag.SelectedGrade);
            //var _HSCode10Digits = ds.HSCodes.Select(p => p.HSCODE).Distinct().ToList();
            //ViewBag.HSCode10digits = PopulateDropdownListValues(_HSCode10Digits, ViewBag.SelectedHSCode10Digits);
            //var _Guage = ds.PieceInfos.Select(p => p.TYP).Distinct().ToList();
            //ViewBag.Guage = PopulateDropdownListValues(_Guage, ViewBag.SelectedGuage);
            var _ProductNames = new List<string> { "Metallurgical Coke", "Iron Ore", "Scrap Steel", "Carbon and alloy semi-finished products", "Carbon and alloy flat product", "Carbon and alloy long product", "Stainless steel products", "Carbon and alloy pipe and tube products" };
            ViewBag.ProductNames = PopulateDropdownListValues(_ProductNames, ViewBag.SelectedProductName, false);
            if (Request.HttpMethod == "POST" && model.Coil > 0 && Request["btnfilter"] != null)
            {
                //MAKE API call to get VCNumber
                //string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products";
                string APIURL = ApiDomain + "/v1/products";
                VCRequest data = new VCRequest();
                data.productName = Request["ddlProductName"] == "-- SELECT --" ? "Carbon and alloy long product" : Request["ddlProductName"];
                data.hsCode = Request["txtHSCode10digits"];
                data.facility = new VCFacility
                {
                    addressLocality = "Hamilton",
                    addressRegion = "Ontario",
                    postalCode = "",
                    addressCountry = Request["txtCountryofOrigin"],
                    streetAddress = "",
                    latitude = "43.2557",
                    longitude = "-79.8711",
                    globalLocationNumber = Request["txtLocation"]
                };
                data.manufacturer = new VCManufacturer { name = "Steel Co." };
                data.weight = new UnitofMeasure { unit = "KG", value = Request["myRange"] == string.Empty ? "0" : Request["myRange"] };
                data.length = new UnitofMeasure { unit = "CM", value = Request["LengthRange"] == string.Empty ? "0" : Request["LengthRange"] };
                data.width = new UnitofMeasure { unit = "CM", value = Request["WidthRange"] == string.Empty ? "0" : Request["WidthRange"] };
                data.technologyType = Request["txtGrade"].StartsWith("EA") ? "ElectricArcFurnace" : "BlastFurnace";
                data.observation = new List<VCObservation> { new VCObservation { description = "Aluminum", name = "aluminum", type = "ChemicalProperty", unit = "%", value = "0.05" } };
                data.grade = Request["txtGrade"];
                data.heatNumber = Request["HeatNo"];
                var postString = JsonConvert.SerializeObject(data);
                //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "Authorization:Bearer " + token, "", "");
                var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
                var res = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
                //var res = JsonConvert.DeserializeObject<VCResponse>(objResponse.ResponseString);
                ViewBag.VCId = res.id;
                if (res.id == null)
                {
                    TempData["Error"] = "Error in Product API";//res.message;
                }
                else
                {
                    //update VC number in Database from above result
                    ds.UpdateVCId(model.Coil, res.id);
                    RemoveCache("AllProductResponse");
                    //var Allproductresponse = GetDataFromCache<AllProductResponse>("AllProductResponse", GetAllProductAPIURL);
                    var info = ds.PieceInfos.Where(p => p.MES_PCE_IDENT_NO == model.Coil).FirstOrDefault();
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
                    }
                    // Insert Mill Test data at the time of Produt Creation

                    List<MillTestDataAPIRequestModel> MillTestValues = ds.GetCoilMillTestDatra(model.Coil);
                    var product = res.productVC.credentialSubject.product;
                    var facility = res.productVC.credentialSubject.facility;
                    MillTestAPIRequest milltestdata = new MillTestAPIRequest();
                    milltestdata.productId = res.id;
                    milltestdata.certifier = "Dofasco";
                    milltestdata.manufacturer = new MillTestManufacturer { name = product.manufacturer.name };
                    milltestdata.manufacturer.address = new MillTestAddress { addressLocality = "Hamilton", addressRegion = "Ontario", addressCountry = "BRAZIL" };
                    milltestdata.specification = "39.9276";
                    milltestdata.place = new MillTestPlace { addressCountry = "BRAZIL", addressLocality = "Hamilton", addressRegion = "Ontario", latitude = Convert.ToDouble("43.2557"), longitude = Convert.ToDouble("-79.8711") };
                    milltestdata.originalCountryOfMeltAndPour = "CANADA";
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
                    if(SaveMillTestRequest.ResponseCode !=HttpStatusCode.Created)
                    {
                        TempData["Error"] = "Error in MillTest API Submission Request for New VC: " + res.id;//res.message;
                    }
                }
                //Get All Active Product API
                //string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
                //string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
                //var SpecificProductDetailURL = ApiDomain + "/v1/products/" + res.id;
                //var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(SpecificProductDetailURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
                //if (Allproductresponse != null)
                //{
                //    if (Allproductresponse.statusCode != 404)
                //    {
                //        model.lstActiveProducts = new List<ProductResult>();
                //        foreach (var activeproduct in Allproductresponse.products?.active)
                //        {
                //            if (activeproduct.productVC != null)
                //            {
                //                ProductResult obj = new ProductResult();
                //                obj.id = activeproduct.id;
                //                obj.issuanceDate = activeproduct.productVC.issuanceDate;
                //                obj.Grade = activeproduct.productVC.credentialSubject?.grade;
                //                obj.HSCode = activeproduct.productVC.credentialSubject?.HSCode;
                //                obj.ProductName = activeproduct.productVC.credentialSubject?.product?.name;
                //                obj.Description = activeproduct.productVC.credentialSubject?.product?.description;
                //                obj.Manufacturer = activeproduct.productVC.credentialSubject?.product?.manufacturer?.name;
                //                obj.Technology = "ElectricArcFurnace";
                //                obj.FacilityAddressCountry = activeproduct.productVC.credentialSubject?.facility?.address?.addressCountry;
                //                obj.Width = activeproduct.productVC.credentialSubject?.product?.width?.value + " (" + activeproduct.productVC.credentialSubject?.product?.width?.unitCode + ")";
                //                obj.Length = activeproduct.productVC.credentialSubject?.product?.length?.value + " (" + activeproduct.productVC.credentialSubject?.product?.length?.unitCode + ")";
                //                obj.Weight = activeproduct.productVC.credentialSubject?.product?.weight?.value + " (" + activeproduct.productVC.credentialSubject?.product?.weight?.unitCode + ")";
                //                model.lstActiveProducts.Add(obj);
                //            }
                //        }
                //        model.lstActiveProducts = model.lstActiveProducts.OrderByDescending(p => p.issuanceDate).ToList();
                //    }
                //    else
                //    {
                //        throw new System.Exception("Get All Product List API Error:" + Allproductresponse.message);
                //    }
                //}home
                return RedirectToAction("Dashboard", "Home", new { Id = res.id });
            }
            return View(model);
        }

        public PartialViewResult _ProductDetail(string id)
        {
            List<ProductDetail> lstProductDetail = new List<ProductDetail>();
            //string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/";
            var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(ApiDomain + "/v1/products/" + id, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            var res = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            if (res.productVC != null)
            {
                foreach (var observation in res.productVC?.credentialSubject?.inspection?.observation)
                {
                    ProductDetail obj = new ProductDetail();
                    obj.ObservationDescription = observation.property?.description;
                    obj.Unit = observation.measurement?.unitCode;
                    obj.Value = observation.measurement?.value;
                    lstProductDetail.Add(obj);
                }

                //if (!string.IsNullOrEmpty(res.message))
                //{
                //    TempData["Error"] = res.message;
                //}
                //else
                //{
                //    model.result = new ProductResult();
                //    //model.result.HSCode=res.
                //}
            }
            return PartialView(lstProductDetail);
        }
        public List<SelectListItem> PopulateDropdownListValues(List<string> lst, string SelectedValue, bool AddDefaultValue = true)
        {

            List<SelectListItem> result = (from p in lst.AsEnumerable()
                                           select new SelectListItem
                                           {
                                               Text = p.Trim(),
                                               Value = p.Trim()
                                           }).ToList();

            if (AddDefaultValue)
            {
                string SelectText = "-- SELECT --";
                result.Insert(0, new SelectListItem
                {
                    Text = SelectText,
                    Value = SelectText
                });
            }
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                result.Find(c => c.Value == SelectedValue).Selected = true;
            }
            return result;
        }
        /// <summary>
        /// Send an OpenID Connect sign-in request.
        /// Alternatively, you can just decorate the SignIn method with the [Authorize] attribute
        /// </summary>
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                var t = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = t },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            Response.Cookies.Clear();
            HttpCookie myCookie = Request.Cookies["UserGuid"];
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now;
                Response.Cookies.Add(myCookie);
            }
            HttpCookie UserName = Request.Cookies["UserEmail"];
            if (UserName != null)
            {
                UserName.Expires = DateTime.Now;
                Response.Cookies.Add(UserName);
            }
            HttpCookie UsernameCookie = Request.Cookies["UserName"];
            if (UsernameCookie != null)
            {
                UsernameCookie.Expires = DateTime.Now;
                Response.Cookies.Add(UsernameCookie);
            }
            HttpCookie UserTokenCookie = Request.Cookies["UserToken"];
            if (UserTokenCookie != null)
            {
                UserTokenCookie.Expires = DateTime.Now;
                Response.Cookies.Add(UserTokenCookie);
            }
            var parameters = new Dictionary<string, string>
                {
                    { "post_logout_redirect_uri", System.Configuration.ConfigurationManager.AppSettings["redirectUri"] }
                };
            var requestUrl = string.Format("{0}/logout?{1}", EndPointUrl, BuildQueryString(parameters));

            Response.Redirect(requestUrl);
        }

        public ActionResult Test()
        {
            ProductDetailView model = new ProductDetailView();
            // string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/dc82c0ec-7b66-41dc-a9ba-2bb1c2f9ea5";
            // var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");
            //model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            return View(model);
        }
        public ActionResult RemoveCache(string Key = "")
        {
            RemoveDataFromCache(Key);
            return Content("Success");
        }
        public ActionResult HighChart()
        {
            if (Request.Cookies["UserToken"] != null)
            {
                //You get the user's first and last name below:
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Page = "report";
            var _DateFilters = new List<string> { "Last 3 Days", "Last 7 Days", "Last 1 Month", "Last 3 Months" };
            ViewBag.DateFilters = PopulateDropdownListValues(_DateFilters, null, false);

            // string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
            //string GetAllProductAPIURL = ApiDomain + "/v1/products?category=active&offset=0&count=100";
            //var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            //var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
            var Allproductresponse = GetDataFromCache<AllProductResponse>("AllProductResponse", GetAllProductAPIURL);
            if (Allproductresponse != null)
            {
                ViewBag.Active = (int)Allproductresponse.totalProductsCount?.active;
                ViewBag.Consumed = (int)Allproductresponse.totalProductsCount?.consumed;
                ViewBag.History = (int)Allproductresponse.totalProductsCount?.history;
                ViewBag.SharedWithme = (int)Allproductresponse.totalProductsCount?.sharedWithMe;
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
                //ViewBag.TransportStartCount = lstEvents.Where(p => p.eventType == "Create").ToList().Count();
                TempData["Events"] = lstEvents;
                TempData["AllProducts"] = Allproductresponse.products.active;
            }
            return View();
        }

        protected string EndPointUrl
        {
            get
            {
                return string.Format("{0}/{1}/{2}", "https://login.microsoftonline.com", "common", @"oauth2/v2.0");
            }
        }
        private string BuildQueryString(IDictionary<string, string> parameters)
        {
            var list = new List<string>();

            foreach (var parameter in parameters)
            {
                list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
            }

            return string.Join("&", list);
        }
        public string AcquireTokenWithResource(string resource)
        {
            var code = Request.Params["code"];
            AuthenticationContext ac =
        new AuthenticationContext(string.Format("https://login.microsoftonline.com/{0}", "common"
                                  ));
            ClientCredential clcred =
                new ClientCredential(System.Configuration.ConfigurationManager.AppSettings["ClientId"], System.Configuration.ConfigurationManager.AppSettings["ClientSecret"]);
            var r = ac.AcquireTokenAsync(resource, clcred).Result;


            var token = r.AccessToken;
            var b = r.UserInfo;

            //HttpCookie c = new HttpCookie("AccessToken");
            //c.Expires = r.ExpiresOn.DateTime;
            //c.Value = token;
            //Response.Cookies.Add(c);
            var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails("https://graph.microsoft.com/oidc/userinfo", WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", token);
            var Allproductresponse = Allproductrequest.ResponseString;

            var list = new List<string>();
            for (int i = 0; i < Request.Params.AllKeys.Count(); i++)
            {
                list.Add(Request.Params.AllKeys[i].ToString());
            }


            return token;
        }
    }
}