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

namespace AzureAd_Login_Sample.Controllers
{
    public class ContractController : BaseController
    {
        // GET: Contract
        public ActionResult Index(ContracListView model, string Filterby = "All")
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

            //string GetAllContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts";
            //var Allcontractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "",BearerToken);
            //Allcontractrequest.ResponseString = "[{\"id\":1,\"startDate\":\"2021-10-14\",\"endDate\":\"2021-10-15\",\"destination\":\"CAN\",\"isAccepted\":true,\"status\":\"EXPIRED\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-10-14T18:52:38.563Z\",\"updatedAt\":\"2021-10-15T00:00:00.000Z\",\"sender\":{\"id\":1,\"did\":\"did:key:z6MkhUDRZpEmsxrjLGpDDmfMzvTp2Wji1PJRqWsi4EfnjJi4\",\"name\":\"Mining Co.\",\"email\":\"mining@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":1,\"role\":\"client\"},\"receiver\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"}},{\"id\":2,\"startDate\":\"2021-11-01\",\"endDate\":\"2021-11-30\",\"destination\":\"CAN\",\"isAccepted\":true,\"status\":\"ACTIVE\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-01T20:28:41.414Z\",\"updatedAt\":\"2021-11-01T20:29:09.000Z\",\"sender\":{\"id\":1,\"did\":\"did:key:z6MkhUDRZpEmsxrjLGpDDmfMzvTp2Wji1PJRqWsi4EfnjJi4\",\"name\":\"Mining Co.\",\"email\":\"mining@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":1,\"role\":\"client\"},\"receiver\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"}},{\"id\":4,\"startDate\":\"2021-11-01\",\"endDate\":\"2021-11-30\",\"destination\":\"CAN\",\"isAccepted\":true,\"status\":\"ACTIVE\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-01T20:47:38.150Z\",\"updatedAt\":\"2021-11-09T05:46:49.000Z\",\"sender\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"},\"receiver\":{\"id\":3,\"did\":\"did:key:z6Mko5QGjK56EwM59qdjn99TrJqQFvNcokxwwCTUJ8sfiG4Z\",\"name\":\"Fabricator Co.\",\"email\":\"fabricator@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":3,\"role\":\"client\"}},{\"id\":5,\"startDate\":\"2021-11-10\",\"endDate\":\"2022-01-01\",\"destination\":\"CAN\",\"isAccepted\":false,\"status\":\"INITIATED\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-11T06:08:51.411Z\",\"updatedAt\":\"2021-11-11T06:08:51.411Z\",\"sender\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"},\"receiver\":{\"id\":1,\"did\":\"did:key:z6MkhUDRZpEmsxrjLGpDDmfMzvTp2Wji1PJRqWsi4EfnjJi4\",\"name\":\"Mining Co.\",\"email\":\"mining@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":1,\"role\":\"client\"}},{\"id\":6,\"startDate\":\"2021-11-11\",\"endDate\":\"2021-11-30\",\"destination\":\"USA\",\"isAccepted\":false,\"status\":\"INITIATED\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-11T06:18:01.881Z\",\"updatedAt\":\"2021-11-11T06:18:01.881Z\",\"sender\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"},\"receiver\":{\"id\":3,\"did\":\"did:key:z6Mko5QGjK56EwM59qdjn99TrJqQFvNcokxwwCTUJ8sfiG4Z\",\"name\":\"Fabricator Co.\",\"email\":\"fabricator@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":3,\"role\":\"client\"}},{\"id\":7,\"startDate\":\"2021-11-11\",\"endDate\":\"2021-12-11\",\"destination\":\"Canada\",\"isAccepted\":false,\"status\":\"INITIATED\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-11T06:20:44.173Z\",\"updatedAt\":\"2021-11-11T06:20:44.173Z\",\"sender\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"},\"receiver\":{\"id\":3,\"did\":\"did:key:z6Mko5QGjK56EwM59qdjn99TrJqQFvNcokxwwCTUJ8sfiG4Z\",\"name\":\"Fabricator Co.\",\"email\":\"fabricator@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":3,\"role\":\"client\"}},{\"id\":8,\"startDate\":\"2021-11-11\",\"endDate\":\"2021-12-11\",\"destination\":\"Canada\",\"isAccepted\":false,\"status\":\"INITIATED\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-11T06:28:23.736Z\",\"updatedAt\":\"2021-11-11T06:28:23.736Z\",\"sender\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"},\"receiver\":{\"id\":3,\"did\":\"did:key:z6Mko5QGjK56EwM59qdjn99TrJqQFvNcokxwwCTUJ8sfiG4Z\",\"name\":\"Fabricator Co.\",\"email\":\"fabricator@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":3,\"role\":\"client\"}},{\"id\":9,\"startDate\":\"2021-11-09\",\"endDate\":\"2021-11-19\",\"destination\":\"USA\",\"isAccepted\":false,\"status\":\"INITIATED\",\"comment\":\"\",\"s3DocumentsFolderPath\":\"\",\"createdAt\":\"2021-11-11T16:52:26.482Z\",\"updatedAt\":\"2021-11-11T16:52:26.482Z\",\"sender\":{\"id\":2,\"did\":\"did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX\",\"name\":\"Steel Co.\",\"email\":\"steel@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":2,\"role\":\"client\"},\"receiver\":{\"id\":4,\"did\":\"did:key:z6MkppW5RmMY58oYSgwypFa2bL8NgDT96gzqKwZtZwm7oC78\",\"name\":\"Carrier Co.\",\"email\":\"carrier@email.com\",\"address\":\"450 1 St SW, Calgary AB T2P 5H1, CAN\",\"phone\":\"403-920-2000\",\"mill\":0,\"role\":\"client\"}}]";
            //var Allcontractresponse = JsonConvert.DeserializeObject<List<AllContractReponse>>(Allcontractrequest.ResponseString);
            var Allcontractresponse = GetDataFromCache<List<AllContractReponse>>("AllContractResponse", GetAllContractAPIURL);
            if (Allcontractresponse != null)
            {
                model.lst = new List<ContractView>();
                foreach (var activecontract in Allcontractresponse)
                {
                    ContractView obj = new ContractView();
                    obj.ContractId = activecontract.id;
                    obj.CreatedOn = activecontract.createdAt;
                    obj.CounterParty = activecontract.receiver?.name;
                    obj.StartDate = Convert.ToDateTime(activecontract.startDate);
                    obj.EndDate = Convert.ToDateTime(activecontract.endDate);
                    obj.Status = activecontract.status;
                    model.lst.Add(obj);
                }
            }
            if (Filterby != "All")
            {
                if (Filterby == "Day")
                {
                    model.lst = model.lst.Where(p => p.CreatedOn >= DateTime.Now.Date.AddHours(-24)).ToList();
                }
                if (Filterby == "Week")
                {
                    model.lst = model.lst.Where(p => p.CreatedOn >= DateTime.Now.Date.AddDays(-7)).ToList();
                }
                if (Filterby == "Month")
                {
                    model.lst = model.lst.Where(p => p.CreatedOn >= DateTime.Now.Date.AddMonths(-1)).ToList();
                }
            }
            ViewBag.Filterby = Filterby;
            return View(model);
        }
        [HttpGet]
        public ActionResult _CreateContract(Contract model)
        {
            model.ContractRequest = new ContractAPIRequest();
            if (model?.Id == 0)
            {
                model = new Contract();
                ViewBag.SelectedCountry = "-- SELECT --";
            }
            else
            {
                //string GetContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts?id=" + model.Id;
                string GetContractAPIURL = ApiDomain + "/v1/contracts?id=" + model.Id;
                var Contractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "",BearerToken);
                var ContractresponseAll = JsonConvert.DeserializeObject<List<AllContractReponse>>(Contractrequest.ResponseString);
                var Contractresponse = ContractresponseAll.Where(p => p.id == model.Id).FirstOrDefault();
                if (Contractresponse != null)
                {
                    model.ContractRequest.endDate = Contractresponse.endDate;
                    model.ContractRequest.startDate = Contractresponse.startDate;
                    ViewBag.SelectedCountry = Contractresponse.destination?.ToUpper();
                    ViewBag.StartDate = Contractresponse?.startDate;
                    ViewBag.EndDate = Contractresponse?.endDate;
                    ViewBag.CounerParty = Contractresponse?.receiver?.name;
                }
            }
            var _Country = new List<string> { "CANADA", "USA" };
            ViewBag.Country = PopulateDropdownListValues(_Country, ViewBag.SelectedCountry);
            return PartialView(model);
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
        public ActionResult SaveContract(string serializeddata)
        {
            var model = JsonConvert.DeserializeObject<ContractAPIRequest>(serializeddata);
            ContractAPIRequest data = new ContractAPIRequest();
            data.sender = "did:key:z6MkiQ89X1yScaGBJ5sVS62wGFAhzyz1zPiwmK8GrchJ5EKX";
            data.receiver = model.receiver;
            data.destination = model.destination;
            data.startDate = Convert.ToDateTime(model.startDate).ToString("yyyy-MM-dd");
            data.endDate = Convert.ToDateTime(model.endDate).ToString("yyyy-MM-dd");
            //string CraeteContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts";
            string CraeteContractAPIURL = ApiDomain + "/v1/contracts";
            var postString = JsonConvert.SerializeObject(data);
            var Contractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(CraeteContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Post, postString, "", "", "",BearerToken);
            //var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Contractrequest.ResponseString);

            return Content("Success");
        }
        public ActionResult UpdateContract(string serializeddata, int id)
        {
            var model = JsonConvert.DeserializeObject<ContractAPIRequest>(serializeddata);
            ContractUpdateRequest data = new ContractUpdateRequest();
            data.contractId = id;
            data.startDate = Convert.ToDateTime(model.startDate).ToString("yyyy-MM-dd");
            data.endDate = Convert.ToDateTime(model.endDate).ToString("yyyy-MM-dd");
            //string UpdateContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts";
            string UpdateContractAPIURL = ApiDomain + "/v1/contracts";
            var postString = JsonConvert.SerializeObject(data);
            var Contractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(UpdateContractAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Put, postString, "", "", "",BearerToken);
            //var Allproductresponse = JsonConvert.DeserializeObject<AllProductResponse>(Contractrequest.ResponseString);

            return Content("Success");
        }
        public ActionResult DeleteContract(int id)
        {
            //string DeleteContractAPIURL = "https://www.mockachino.com/97fd072e-cfdf-45/v1/contracts";
            string DeleteContractAPIURL = ApiDomain + "/v1/contracts";
            ContractDeleteAPIRequest obj = new ContractDeleteAPIRequest();
            obj.contractIds = new List<string>();
            obj.contractIds.Add(id.ToString());
            var postString = JsonConvert.SerializeObject(obj);
            var Contractrequest = WebHelper.GetWebAPIResponseWithErrorDetails(DeleteContractAPIURL, WebHelper.ContentType.application_json, "DELETE", postString, "", "", "",BearerToken);
            return Content("Success");
        }
         
    }
}