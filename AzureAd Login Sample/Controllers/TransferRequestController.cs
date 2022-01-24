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
    public class TransferRequestController : BaseController
    {
        // GET: TransferRequest
        public ActionResult Index(TransferRequestViewModel model, string Filterby = "ALL", string Status = "ALL", string Type = "ALL")
        {
            //if (Request.Cookies["UserToken"] != null)
            //{
            //    ViewBag.Name = Request.Cookies["UserName"]?.Value;
            //    ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
            //    // The 'preferred_username' claim can be used for showing the username
            //    ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            string GetAllTransferRequestAPIURL = ApiDomain + "/v1/products/transfer/requests";
            var AllTransferRequest = WebHelper.GetWebAPIResponseWithErrorDetails(GetAllTransferRequestAPIURL, WebHelper.ContentType.application_json, WebRequestMethods.Http.Get, "", "", "", "", BearerToken);
            var AllTransferresponse = JsonConvert.DeserializeObject<List<AllTransferRequestResponse>>(AllTransferRequest.ResponseString);
            if (AllTransferresponse != null)
            {
                model.lst = new List<TransferRequests>();
                if (Filterby != "All")
                {
                    if (Filterby == "Day")
                    {
                        AllTransferresponse = AllTransferresponse.Where(p => p.createdAt >= DateTime.Now.Date.AddHours(-24)).ToList();
                    }
                    if (Filterby == "Week")
                    {
                        AllTransferresponse = AllTransferresponse.Where(p => p.createdAt >= DateTime.Now.Date.AddDays(-7)).ToList();
                    }
                    if (Filterby == "Month")
                    {
                        AllTransferresponse = AllTransferresponse.Where(p => p.createdAt >= DateTime.Now.Date.AddMonths(-1)).ToList();
                    }
                }

                var _Status = AllTransferresponse.Select(p => p.status).Distinct().ToList();
                ViewBag.StatusList = PopulateDropdownListValues(_Status, Status);
                var _Type = AllTransferresponse.Select(p => p.type).Distinct().ToList();
                ViewBag.TypeList = PopulateDropdownListValues(_Type, Type);
                if (Status.ToUpper() != "ALL")
                {
                    AllTransferresponse = AllTransferresponse.Where(p => p.status == Status).ToList();
                }
                if (Type.ToUpper() != "ALL")
                {
                    AllTransferresponse = AllTransferresponse.Where(p => p.type == Type).ToList();
                }
                foreach (var transfer in AllTransferresponse)
                {
                    var productweight = transfer.product.productVC.credentialSubject.product.weight;
                    TransferRequests obj = new TransferRequests();
                    obj.RequestId = transfer.id;
                    obj.Updated = transfer.updatedAt;
                    obj.Type = transfer.type;
                    obj.CreatedBy = "Steel Co.";
                    obj.ProductType = "Stainless Steel Products";
                    obj.Quantity = productweight == null ? "" : (productweight.value + " " + productweight.unitCode);
                    obj.Status = transfer.status;
                    obj.ProductId = transfer.product.id;
                    model.lst.Add(obj);
                }
                ViewBag.Filterby = Filterby;
                ViewBag.Status = Status;
                ViewBag.Type = Type;
            }

            return View(model);
        }
        public List<SelectListItem> PopulateDropdownListValues(List<string> lst, string SelectedValue)
        {
            string SelectText = "ALL";
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
    }
}