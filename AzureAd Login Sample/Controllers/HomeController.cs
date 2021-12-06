using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using POC.DataAccess;
using POC.DataAccess.Model;
using POC.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace POC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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
                return RedirectToAction("Dashboard", "Home");
            }
            // return View();
            return RedirectToAction("SignIn", "Home");
        }
        public ActionResult Dashboard(DashBoardView model)
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

            DashboardService ds = new DashboardService();
            ViewBag.WeightKg = 0;
            ViewBag.Lengthcm = 0;
            ViewBag.Widthcm = 0;
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
                    ViewBag.SelectedGuage = info.TYP?.Trim();
                    ViewBag.WeightKg = (int)info.PCE_WT;
                    ViewBag.Lengthcm = info.PCE_THK;
                    ViewBag.Widthcm = (int)info.PCE_WDT;
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var _Coil = ds.PieceInfos.Select(p => p.MES_PCE_IDENT_NO).Distinct().ToList();
            ViewBag.Coil = new SelectList(_Coil);

            var _CountryofOrigin = ds.PieceInfos.Select(p => p.ORIGN_COUNTRY_CD_TEXT).Distinct().ToList();
            ViewBag.CountryofOrigin = PopulateDropdownListValues(_CountryofOrigin, ViewBag.SelectedCountry);
            var _CountryofMeltPour = ds.PieceInfos.Select(p => p.LABEL_COUNTRY_CD_TEXT).Distinct().ToList();
            ViewBag.CountryofMeltPour = PopulateDropdownListValues(_CountryofMeltPour, ViewBag.SelectedCountryofMeltPourlst);
            var _TypeofTechnology = ds.PieceInfos.Select(p => p.FIELD_SHORT_DESC).Distinct().ToList();
            ViewBag.TypeofTechnology = PopulateDropdownListValues(_TypeofTechnology, ViewBag.SelectedTypeofTechnology);
            var _Grade = ds.PieceInfos.Select(p => p.GRD_CD).Distinct().ToList();
            ViewBag.Grade = PopulateDropdownListValues(_Grade, ViewBag.SelectedGrade);
            var _HSCode10Digits = ds.HSCodes.Select(p => p.HSCODE).Distinct().ToList();
            ViewBag.HSCode10digits = PopulateDropdownListValues(_HSCode10Digits, ViewBag.SelectedHSCode10Digits);
            var _Guage = ds.PieceInfos.Select(p => p.TYP).Distinct().ToList();
            ViewBag.Guage = PopulateDropdownListValues(_Guage, ViewBag.SelectedGuage);
            if (Request.HttpMethod == "POST" && model.Coil > 0 && Request["btnfilter"] != null)
            {
                //MAKE API call to get VCNumber
                string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products";
                VCRequest data = new VCRequest();
                data.productName = "Stainless steel products";
                data.hsCode = Request["ddlHSCode10digits"];
                data.facility = new VCFacility
                {
                    addressLocality = "Hamilton",
                    addressRegion = "Ontario",
                    postalCode = "",
                    addressCountry = Request["ddlCountryofOrigin"],
                    streetAddress = "",
                    latitude = "43.2557",
                    longitude = "-79.8711"
                };
                data.manufacturer = new VCManufacturer { name = "ArcelorMittal Dofasco" };
                data.weight = new UnitofMeasure { unit = "KG", value = Request["myRange"] };
                data.length = new UnitofMeasure { unit = "CM", value = Request["LengthRange"] };
                data.width = new UnitofMeasure { unit = "CM", value = Request["WidthRange"] };
                data.technologyType = "ElectricArcFurnace";
                data.observation = new List<VCObservation> { new VCObservation { description = "Aluminum", name = "aluminum", type = "ChemicalProperty", unit = "%", value = "0.05" } };

                var postString = JsonConvert.SerializeObject(data);
                //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "Authorization:Bearer " + token, "", "");
                var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "");
                var res = JsonConvert.DeserializeObject<VCResponse>(objResponse.ResponseString);
                ViewBag.VCId = res.id;

                //update VC number in Database from above result
                ds.UpdateVCId(model.Coil, res.id);

                //Get All Active Product API
                string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
                var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");
                var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
                if (Allproductresponse != null)
                {
                    model.lstActiveProducts = new List<ProductResult>();
                    foreach (var activeproduct in Allproductresponse.products?.active)
                    {
                        if (activeproduct.productVC != null)
                        {
                            ProductResult obj = new ProductResult();
                            obj.id = activeproduct.id;
                            obj.issuanceDate = activeproduct.productVC.issuanceDate;
                            obj.Grade = activeproduct.productVC.credentialSubject?.grade;
                            obj.HSCode = activeproduct.productVC.credentialSubject?.HSCode;
                            obj.ProductName = activeproduct.productVC.credentialSubject?.product?.name;
                            obj.Description = activeproduct.productVC.credentialSubject?.product?.description;
                            obj.Manufacturer = activeproduct.productVC.credentialSubject?.product?.manufacturer?.name;
                            obj.Technology = "ElectricArcFurnace";
                            obj.FacilityAddressCountry = activeproduct.productVC.credentialSubject?.facility?.address?.addressCountry;
                            obj.Width = activeproduct.productVC.credentialSubject?.product?.width?.value + " (" + activeproduct.productVC.credentialSubject?.product?.width?.unitCode + ")";
                            obj.Length = activeproduct.productVC.credentialSubject?.product?.length?.value + " (" + activeproduct.productVC.credentialSubject?.product?.length?.unitCode + ")";
                            obj.Weight = activeproduct.productVC.credentialSubject?.product?.weight?.value + " (" + activeproduct.productVC.credentialSubject?.product?.weight?.unitCode + ")";
                            model.lstActiveProducts.Add(obj);
                        }
                    }
                    model.lstActiveProducts = model.lstActiveProducts.OrderByDescending(p => p.issuanceDate).ToList();
                }
            }
            return View(model);
        }
        public PartialViewResult _ProductDetail(string id)
        {
            List<ProductDetail> lstProductDetail = new List<ProductDetail>();
            string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/";
            var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL + id, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");
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
        public List<SelectListItem> PopulateDropdownListValues(List<string> lst, string SelectedValue)
        {
            string SelectText = "-- SELECT --";
            List<SelectListItem> result = (from p in lst.AsEnumerable()
                                           select new SelectListItem
                                           {
                                               Text = p.Trim(),
                                               Value = p.Trim()
                                           }).ToList();
            result.Insert(0, new SelectListItem
            {
                Text = SelectText,
                Value = SelectText
            });
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
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult Test()
        {
            ProductDetailView model = new ProductDetailView();
            string APIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products/dc82c0ec-7b66-41dc-a9ba-2bb1c2f9ea5";
            var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");
            model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            return View(model);
        }
        public ActionResult HighChart()
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
            //TempData["Message"] = "test";
            ViewBag.Page = "report";

            string GetAllProductAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/products?category=active&offset=0&count=100";
            var Allproductrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllProductAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "");
            var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Allproductrequest.ResponseString);
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
                TempData["Events"] = lstEvents;
            }
            return View();
        }
        }
}