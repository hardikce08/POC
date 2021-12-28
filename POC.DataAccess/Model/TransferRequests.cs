using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class TransferRequests
    {
        public int RequestId { get; set; }
        public DateTime Updated { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ProductType { get; set; }
        public string Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProductId { get; set; }
    }
    public class TransferRequestViewModel
    {
        public List<TransferRequests> lst { get; set; }
    }
    public class TransferRequestGeo
    {
        public List<string> type { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class TransferRequestAddress
    {
        public List<string> type { get; set; }
        public string postalCode { get; set; }
        public string addressRegion { get; set; }
        public string streetAddress { get; set; }
        public string addressCountry { get; set; }
        public string addressLocality { get; set; }
        public string organizationName { get; set; }
    }

    public class TransferRequestPortOfEntry
    {
        public TransferRequestGeo geo { get; set; }
        public List<string> type { get; set; }
        public TransferRequestAddress address { get; set; }
    }

    public class TransferRequestPortOfDestination
    {
        public TransferRequestGeo geo { get; set; }
        public List<string> type { get; set; }
        public TransferRequestAddress address { get; set; }
    }

    public class TransferRequestReceiptLocation
    {
        public TransferRequestGeo geo { get; set; }
        public List<string> type { get; set; }
        public TransferRequestAddress address { get; set; }
    }

    public class TransferRequestContract
    {
        public int id { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string destination { get; set; }
        public bool isAccepted { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
        public string s3DocumentsFolderPath { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class TransferRequestProof
    {
        public string type { get; set; }
        public DateTime created { get; set; }
        public string proofValue { get; set; }
        public string proofPurpose { get; set; }
        public string verificationMethod { get; set; }
    }

    public class TransferRequestCredentialStatus
    {
        public string id { get; set; }
        public string type { get; set; }
        public string revocationListIndex { get; set; }
        public string revocationListCredential { get; set; }
    }

    public class TransferRequestWidth
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class TransferRequestLength
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class TransferRequestWeight
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class TransferRequestManufacturer
    {
        public string name { get; set; }
        public List<string> type { get; set; }
    }

    public class TransferRequestProduct
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> type { get; set; }
        public TransferRequestWidth width { get; set; }
        public TransferRequestLength length { get; set; }
        public TransferRequestWeight weight { get; set; }
        public string description { get; set; }
        public TransferRequestManufacturer manufacturer { get; set; }
        public TransferRequestProductVC productVC { get; set; }
        public List<TransferRequestProductSpec> productSpecs { get; set; }
        public string productVCHash { get; set; }
        public string status { get; set; }
        public TransferRequestOrigin origin { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int ownerId { get; set; }
        public int? custodianId { get; set; }
        public int creatorId { get; set; }
        public string txHash { get; set; }
        public DateTime txTimestamp { get; set; }
        public string releaseStatus { get; set; }
    }

    public class TransferRequestFacility
    {
        public TransferRequestGeo geo { get; set; }
        public List<string> type { get; set; }
        public TransferRequestAddress address { get; set; }
    }

    public class TransferRequestProperty
    {
        public string name { get; set; }
        public List<string> type { get; set; }
        public string description { get; set; }
    }

    public class TransferRequestMeasurement
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class TransferRequestObservation
    {
        public List<string> type { get; set; }
        public TransferRequestProperty property { get; set; }
        public TransferRequestMeasurement measurement { get; set; }
    }

    public class TransferRequestInspection
    {
        public List<string> type { get; set; }
        public List<TransferRequestObservation> observation { get; set; }
    }

    public class TransferRequestCredentialSubject
    {
        public string sku { get; set; }
        public List<string> type { get; set; }
        public string grade { get; set; }
        public string HSCode { get; set; }
        public TransferRequestProduct product { get; set; }
        public TransferRequestFacility facility { get; set; }
        public string heatNumber { get; set; }
        public TransferRequestInspection inspection { get; set; }
        public string productionDate { get; set; }
        public string technologyType { get; set; }
    }

    public class TransferRequestProductVC
    {
        public string id { get; set; }
        public List<string> type { get; set; }
        public TransferRequestProof proof { get; set; }
        public string issuer { get; set; }

        [JsonProperty("@context")]
        public List<string> Context { get; set; }
        public DateTime issuanceDate { get; set; }
        public TransferRequestCredentialStatus credentialStatus { get; set; }
        public TransferRequestCredentialSubject credentialSubject { get; set; }
    }

    public class TransferRequestProductSpec
    {
        public List<string> type { get; set; }
        public TransferRequestProperty property { get; set; }
        public TransferRequestMeasurement measurement { get; set; }
    }

    public class TransferRequestOrigin
    {
        public TransferRequestGeo geo { get; set; }
        public List<string> type { get; set; }
        public TransferRequestAddress address { get; set; }
    }

    public class TransferRequestSender
    {
        public int id { get; set; }
        public string did { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int mill { get; set; }
        public string role { get; set; }
    }

    public class TransferRequestReceiver
    {
        public int id { get; set; }
        public string did { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int mill { get; set; }
        public string role { get; set; }
    }

    public class AllTransferRequestResponse
    {
        public int id { get; set; }
        public string price { get; set; }
        public string weight { get; set; }
        public string unit { get; set; }
        public TransferRequestPortOfEntry portOfEntry { get; set; }
        public object portOfArrival { get; set; }
        public TransferRequestPortOfDestination portOfDestination { get; set; }
        public TransferRequestReceiptLocation receiptLocation { get; set; }
        public string countryOfDestination { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string comment { get; set; }
        public bool hasDocuments { get; set; }
        public object senderS3DocumentsFolderPath { get; set; }
        public object receiverS3DocumentsFolderPath { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public TransferRequestContract contract { get; set; }
        public TransferRequestProduct product { get; set; }
        public TransferRequestSender sender { get; set; }
        public TransferRequestReceiver receiver { get; set; }
    }
}
