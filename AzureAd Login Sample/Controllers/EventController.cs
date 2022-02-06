using Newtonsoft.Json;
using POC.Controllers;
using POC.DataAccess;
using POC.DataAccess.Model;
using POC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AzureAd_Login_Sample.Controllers
{
    public class EventController : BaseController
    {
        // GET: Event
        public ActionResult Index(EventView model, string Coil = "")
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
            try
            {
                if (Coil != "")
                {
                    model.Coil = Convert.ToInt32(Coil);
                }
                DashboardService ds = new DashboardService();
                List<SelectListItem> items = new List<SelectListItem>();
                var _Coil = ds.PieceInfos.Where(p => p.VCId != null).Select(p => p.MES_PCE_IDENT_NO).Distinct().ToList();
                ViewBag.Coil = new SelectList(_Coil, model.Coil);
                if (model.Coil > 0)
                {
                    var data = ds.PieceInfos.Where(p => p.MES_PCE_IDENT_NO == model.Coil).FirstOrDefault();
                    if (data != null)
                    {
                        if (!string.IsNullOrEmpty(data.VCId))
                        {
                            //get list of all events for selected Coild VCId
                            //data.VCId = "73280271-74f4-4012-a0e2-5583c6f6cbdd";
                            string APIURL = ApiDomain + "/v1/products/" + data.VCId;
                            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                            //var productresult = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
                            var productresult = GetDataFromCache<Active>("Product:" + data.VCId, APIURL);
                            List<EventDisplayList> eventslst = new List<EventDisplayList>();
                            foreach (var item in productresult?.events)
                            {
                                EventDisplayList obj = new EventDisplayList();
                                obj.EventType = item.eventType;
                                obj.InitiatorName = item.eventVC?.credentialSubject?.initiator?.name;
                                obj.issuanceDate = item.createdAt;
                                obj.addressLocality = item.eventVC?.credentialSubject?.place?.address?.addressLocality;
                                obj.addressRegion = item.eventVC?.credentialSubject?.place?.address?.addressRegion;
                                obj.addressCountry = item.eventVC?.credentialSubject?.place?.address?.addressCountry;
                                obj.Latitude = item.eventVC.credentialSubject?.place?.geo?.latitude;
                                obj.Longitude = item.eventVC.credentialSubject?.place?.geo?.longitude;
                                if (item.eventType == "TransferCustody")
                                {
                                    obj.Latitude2 = item.eventVC.credentialSubject.portOfEntry?.geo?.latitude;
                                    obj.Longitude2 = item.eventVC.credentialSubject?.portOfEntry?.geo?.longitude;
                                }
                                eventslst.Add(obj);
                            }
                            model.events = eventslst;
                            ViewBag.ProductId = data.VCId;
                            if (eventslst.Where(p => p.EventType == "StorageStart").FirstOrDefault() != null)
                            {
                                ViewBag.IsStorageEndVisible = true;
                            }
                            else
                            {
                                ViewBag.IsStorageEndVisible = false;
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Product VC Id is not associated for selected coil";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error while Loading Events";
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult _CreateEvent()
        {
            try
            {
                //var Allcontractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //var Allcontractresponse = JsonConvert.DeserializeObject<List<AllContractReponse>>(Allcontractrequest.ResponseString);
                var Allcontractresponse = GetDataFromCache<List<AllContractReponse>>("AllContractResponse", GetAllContractAPIURL);
                if (Allcontractresponse != null)
                {
                    //filter only Active Contracts
                    Allcontractresponse = Allcontractresponse.Where(p => p.status != "EXPIRED").ToList();
                    Dictionary<string, string> contracts = new Dictionary<string, string>();
                    foreach (var active in Allcontractresponse)
                    {
                        contracts.Add("#" + active.id.ToString(), active.sender.did);
                    }
                    ViewBag.Contracts = PopulateDropdownListValues(contracts, null);
                }

                //var AllOrganizationrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllOrganizationAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //var AllOrganizationresponse = JsonConvert.DeserializeObject<List<AllOrganizationResponse>>(AllOrganizationrequest.ResponseString);
                var AllOrganizationresponse = GetDataFromCache<List<AllOrganizationResponse>>("AllOrganizationresponse", GetAllOrganizationAPIURL);
                if (AllOrganizationresponse != null)
                {
                    Dictionary<string, string> contracts = new Dictionary<string, string>();
                    foreach (var active in AllOrganizationresponse)
                    {
                        contracts.Add(active.name.ToString(), active.did);
                    }
                    ViewBag.Organisations = PopulateDropdownListValues(contracts, null);
                }
                ViewBag.EventTitle = "Transfer of Ownership";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error during data load";
            }
            return PartialView();
        }
        public ActionResult SaveTransferofOwnership(string serializeddata)
        {
            var model = JsonConvert.DeserializeObject<TransferofOwnerShipAPIRequest>(serializeddata);
            TransferofOwnerShipAPIRequest data = new TransferofOwnerShipAPIRequest();
            data.receiver = model.receiver;
            data.price = model.price;
            data.productId = model.productId;
            data.hasDocuments = false;
            if (model.contractId.Trim() != "-- SELECT --")
            {
                data.contractId = model.contractId;
            }
            //string CraeteContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts";
            string CraeteContractAPIURL = ApiDomain + "/v1/products/transferownership/requests";
            var postString = JsonConvert.SerializeObject(data);
            var Contractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
            //var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Contractrequest.ResponseString);

            return Content("Success");
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
                if (SelectedValue == "CAN")
                {
                    SelectedValue = "CANADA";
                }
                result.Find(c => c.Value == SelectedValue).Selected = true;
            }
            return result;
        }
        public List<SelectListItem> PopulateDropdownListValues(Dictionary<string, string> lst, string SelectedValue)
        {
            string SelectText = "-- SELECT --";
            List<SelectListItem> result = (from p in lst.AsEnumerable()
                                           select new SelectListItem
                                           {
                                               Text = p.Key.Trim(),
                                               Value = p.Value.Trim()
                                           }).ToList();
            result.Insert(0, new SelectListItem
            {
                Text = SelectText,
                Value = SelectText
            });
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                if (SelectedValue == "CAN")
                {
                    SelectedValue = "CANADA";
                }
                result.Find(c => c.Value == SelectedValue).Selected = true;
            }
            return result;
        }
        public ActionResult _Inspect(string id)
        {
            ProductDetailView model = new ProductDetailView();
            try
            {
                string APIURL = ApiDomain + "/v1/products/" + id;
                //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
                model.result = GetDataFromCache<Active>("Product:" + id, APIURL);
                ViewBag.EventTitle = "Inspect";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error during data load";
            }
            return PartialView(model);
        }
        public ActionResult _MillTest(string id)
        {
            ProductDetailView model = new ProductDetailView();
            try
            {
                string APIURL = ApiDomain + "/v1/products/" + id;
                //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
                model.result = GetDataFromCache<Active>("Product:" + id, APIURL);
                ViewBag.EventTitle = "Inspect";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error during data load";
            }
            return PartialView(model);
        }
        public ActionResult SaveInspect(string id, string value)
        {
            string APIURL = ApiDomain + "/v1/products/" + id;
            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            //var activeproduct = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            var activeproduct = GetDataFromCache<Active>("Product:" + id, APIURL);
            var product = activeproduct.productVC.credentialSubject.product;
            var facility = activeproduct.productVC.credentialSubject.facility;
            InspectEventAPIRequest data = new InspectEventAPIRequest();
            data.initiator = new InspectInitiator { name = "Steel Co." };
            data.product = new InspectProduct { id = id, name = product.name };
            data.product.weight = new InspectWeight { unit = product.weight.unitCode, value = product.weight.value };
            data.product.length = new InspectLength { unit = product.length.unitCode, value = product.length.value };
            data.product.width = new InspectWidth { unit = product.width.unitCode, value = product.width.value };
            data.product.manufacturer = new InspectManufacturer { name = product.manufacturer.name };
            data.observation = new List<InspectObservation> { new InspectObservation { name = "aluminum", type = "ChemicalProperty", description = "Aluminum", unit = "%", value = value } };
            data.place = new InspectPlace { addressCountry = facility.address.addressCountry, addressLocality = facility.address.addressLocality, addressRegion = facility.address.addressRegion, latitude = facility.geo.latitude?.ToString(), longitude = facility.geo.longitude?.ToString() };
            string CraeteContractAPIURL = ApiDomain + "/v1/products/inspect";
            var postString = JsonConvert.SerializeObject(data);
            var SaveInspectRequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);

            return Content("Success");
        }
        public ActionResult SaveMillTest(string id, string value)
        {
            string APIURL = ApiDomain + "/v1/products/" + id;
            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            var MillTestValues = JsonConvert.DeserializeObject<List<MillTestValues>>(value);
            var activeproduct = GetDataFromCache<Active>("Product:" + id, APIURL);
            var product = activeproduct.productVC.credentialSubject.product;
            var facility = activeproduct.productVC.credentialSubject.facility;
            MillTestAPIRequest data = new MillTestAPIRequest();
            data.productId = id;
            data.certifier = "Dofasco";
            data.manufacturer = new MillTestManufacturer { name = product.manufacturer.name };
            data.manufacturer.address = new MillTestAddress { addressLocality = "Hamilton", addressRegion = "Ontario", addressCountry = "BRAZIL" };
            data.specification = "39.9276";
            data.place = new MillTestPlace { addressCountry = "BRAZIL", addressLocality = "Hamilton", addressRegion = "Ontario", latitude = Convert.ToDouble("43.2557"), longitude = Convert.ToDouble("-79.8711") };
            data.originalCountryOfMeltAndPour = "CANADA";
            data.observation = new List<MillTestObservation>();
            foreach (var item in MillTestValues)
            {
                data.observation.Add(new MillTestObservation {description=item.symbol,type= "ChemicalProperty",name=item.name,value=item.value,unit="%" });
            }
            string CraeteMillTestAPIURL = ApiDomain + "/v1/products/millTest";
            var postString = JsonConvert.SerializeObject(data);
            var SaveMillTestRequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteMillTestAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);

            return Content("Success");
        }

        public ActionResult _TransferofCustody()
        {
            try
            {
                //var Allcontractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //var Allcontractresponse = JsonConvert.DeserializeObject<List<AllContractReponse>>(Allcontractrequest.ResponseString);
                var Allcontractresponse = GetDataFromCache<List<AllContractReponse>>("AllContractResponse", GetAllContractAPIURL);
                if (Allcontractresponse != null)
                {
                    //filter only Active Contracts
                    Allcontractresponse = Allcontractresponse.Where(p => p.status == "ACTIVE").ToList();
                    Dictionary<string, string> contracts = new Dictionary<string, string>();
                    foreach (var active in Allcontractresponse)
                    {
                        contracts.Add("#" + active.id.ToString(), active.sender.did);
                    }
                    ViewBag.Contracts = PopulateDropdownListValues(contracts, null);
                }
                //var AllOrganizationrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllOrganizationAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //var AllOrganizationresponse = JsonConvert.DeserializeObject<List<AllOrganizationResponse>>(AllOrganizationrequest.ResponseString);
                var AllOrganizationresponse = GetDataFromCache<List<AllOrganizationResponse>>("AllOrganizationresponse", GetAllOrganizationAPIURL);
                if (AllOrganizationresponse != null)
                {
                    Dictionary<string, string> contracts = new Dictionary<string, string>();
                    foreach (var active in AllOrganizationresponse)
                    {
                        contracts.Add(active.name.ToString(), active.did);
                    }
                    ViewBag.Organisations = PopulateDropdownListValues(contracts, null);
                }
                var _Country = new List<string> { "CANADA", "USA" };
                ViewBag.Country = PopulateDropdownListValues(_Country, ViewBag.SelectedCountry);

                DashboardService ds = new DashboardService();
                var Ports = ds.Ports.ToList();
                ViewBag.Ports = PopulateDropdownListValues(Ports.Select(p => p.Ports).ToList(), ViewBag.SelectedCountry);
                ViewBag.ReceiptLocation = PopulateDropdownListValues(Ports.Select(p => p.ReceiptLocation).ToList(), ViewBag.SelectedCountry);
                ViewBag.EventTitle = "Transfer of Custody";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error during data load";
            }
            return PartialView();
        }
        public ActionResult SaveTransferofCustody(string serializeddata)
        {
            var model = JsonConvert.DeserializeObject<TransferofCustodyData>(serializeddata);
            TransferCustodyAPIRequest data = new TransferCustodyAPIRequest();
            data.receiver = model.receiver;
            data.countryOfDestination = model.countryOfDestination;
            data.productId = model.productId;
            data.hasDocuments = false;
            if (model.contractId.Trim() != "-- SELECT --")
            {
                data.contractId = model.contractId;
            }
            DashboardService ds = new DashboardService();
            var Ports = ds.Ports.ToList();
            var portOfEntry = Ports.Where(p => p.Ports == model.portOfEntry).FirstOrDefault();
            data.portOfEntry = new TransferCustodyPortOfEntry { addressCountry = model.countryOfDestination, addressLocality = portOfEntry.Town, addressRegion = portOfEntry.Province, latitude = portOfEntry.Latitude, longitude = portOfEntry.Longitude };
            var portOfDestination = Ports.Where(p => p.Ports == model.portOfDestination).FirstOrDefault();
            data.portOfDestination = new TransferCustodyPortOfDestination { addressCountry = model.countryOfDestination, addressLocality = portOfEntry.Town, addressRegion = portOfEntry.Province, latitude = portOfEntry.Latitude, longitude = portOfEntry.Longitude };
            var receiptLocation = Ports.Where(p => p.Ports == model.portOfDestination).FirstOrDefault();
            data.receiptLocation = new TransferCustodyReceiptLocation { addressCountry = model.countryOfDestination, addressLocality = portOfEntry.Town, addressRegion = portOfEntry.Province, latitude = portOfEntry.Latitude, longitude = portOfEntry.Longitude };

            //string CraeteContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts";
            string CraeteContractAPIURL = ApiDomain + "/v1/products/transfercustody/requests";
            var postString = JsonConvert.SerializeObject(data);
            var Contractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
            //var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Contractrequest.ResponseString);

            return Content("Success");
        }

        public ActionResult _StartStorage(string Type)
        {
            try
            {
                //string APIURL = ApiDomain + "/v1/products/" + id;
                //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
                //ProductDetailView model = new ProductDetailView();
                //model.result = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
                var _UoM = new List<string> { "KG", "Tonne" };
                ViewBag.StorageEventType = Type;
                ViewBag.UoM = PopulateDropdownListValues(_UoM, ViewBag.SelectedCountry);
                ViewBag.EventTitle = Type;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error during data load";
            }
            return PartialView();
        }

        public ActionResult SaveStartStorage(string EventType, string id, string latitude, string longitude, string address, string weightUnit, string weightValue)
        {
            string APIURL = ApiDomain + "/v1/products/" + id;
            //var objResponse = WebHelper.GetWebAPIResponseWithErrorDetails(APIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            //var activeproduct = JsonConvert.DeserializeObject<Active>(objResponse.ResponseString);
            var activeproduct = GetDataFromCache<Active>("Product:" + id, APIURL);
            var product = activeproduct.productVC.credentialSubject.product;
            StartStorageAPIRequest data = new StartStorageAPIRequest();
            data.eventType = EventType;
            data.storedWeight = new InspectWidth { unit = weightUnit, value = weightValue };
            data.initiator = new InspectInitiator { name = "Steel Co." };
            data.product = new InspectProduct { id = id, name = product.name };
            data.product.weight = new InspectWeight { unit = product.weight.unitCode, value = product.weight.value };
            data.product.length = new InspectLength { unit = product.length.unitCode, value = product.length.value };
            data.product.width = new InspectWidth { unit = product.width.unitCode, value = product.width.value };
            data.product.manufacturer = new InspectManufacturer { name = product.manufacturer.name };
            data.observation = new List<InspectObservation> { new InspectObservation { name = "aluminum", type = "ChemicalProperty", description = "Aluminum", unit = "%", value = activeproduct.productSpecs[0].measurement.value } };
            data.place = new InspectPlace { addressCountry = address.Split(',')[2].Trim(), addressLocality = address.Split(',')[0].Trim(), addressRegion = address.Split(',')[1].Trim(), latitude = latitude, longitude = longitude };
            string CraeteContractAPIURL = ApiDomain + "/v1/products/storage";
            var postString = JsonConvert.SerializeObject(data);
            var SaveStorageRequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "", BearerToken);
            return Content("Success");
        }
    }
}