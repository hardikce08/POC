using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class TotalProductsCount
    {
        public int active { get; set; }
        public int consumed { get; set; }
        public int sharedWithMe { get; set; }
        public int sharedWithMeDeclared { get; set; }
        public int sharedWithMeNonDeclared { get; set; }
        public int history { get; set; }
    }

    public class Proof
    {
        public string type { get; set; }
        public DateTime created { get; set; }
        public string proofValue { get; set; }
        public string proofPurpose { get; set; }
        public string verificationMethod { get; set; }
    }

    public class Weight
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class Manufacturer
    {
        public string name { get; set; }
        public List<string> type { get; set; }
    }

    public class Width
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class Length
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> type { get; set; }
        public Weight weight { get; set; }
        public string description { get; set; }
        public Manufacturer manufacturer { get; set; }
        public Width width { get; set; }
        public Length length { get; set; }
    }

    public class Geo
    {
        public List<string> type { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
    }

    public class Address
    {
        public List<string> type { get; set; }
        public string postalCode { get; set; }
        public string addressRegion { get; set; }
        public string streetAddress { get; set; }
        public string addressCountry { get; set; }
        public string addressLocality { get; set; }
    }

    public class Facility
    {
        public Geo geo { get; set; }
        public List<string> type { get; set; }
        public Address address { get; set; }
    }

    public class Property
    {
        public string name { get; set; }
        public List<string> type { get; set; }
        public string description { get; set; }
    }

    public class Measurement
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class Observation
    {
        public List<string> type { get; set; }
        public Property property { get; set; }
        public Measurement measurement { get; set; }
    }

    public class Inspection
    {
        public List<string> type { get; set; }
        public List<Observation> observation { get; set; }
    }

    public class CredentialSubject
    {
        public string sku { get; set; }
        public List<string> type { get; set; }
        public string grade { get; set; }
        public string HSCode { get; set; }
        public Product product { get; set; }
        public Facility facility { get; set; }
        public string heatNumber { get; set; }
        public Inspection inspection { get; set; }
        public string productionDate { get; set; }
        public string technologyType { get; set; }
        public Place place { get; set; }
        public string eventId { get; set; }
        public object eventType { get; set; }
        public Initiator initiator { get; set; }
        public string productId { get; set; }
        public string description { get; set; }
        public Receiver receiver { get; set; }
        public DateTime? eventTime { get; set; }
        public PortOfEntry portOfEntry { get; set; }
        public ReceiptLocation receiptLocation { get; set; }
        public PortOfDestination portOfDestination { get; set; }
        public string countryOfDestination { get; set; }
        public List<NewProduct> newProduct { get; set; }
        public List<ConsumedProduct> consumedProducts { get; set; }
        public string deliveryMethod { get; set; }
        public string trackingNumber { get; set; }
    }

    public class ProductVC
    {
        public string id { get; set; }
        public List<string> type { get; set; }
        public Proof proof { get; set; }
        public string issuer { get; set; }

        [JsonProperty("@context")]
        public List<string> Context { get; set; }
        public DateTime issuanceDate { get; set; }
        public CredentialSubject credentialSubject { get; set; }
    }

    public class ProductSpec
    {
        public List<string> type { get; set; }
        public Property property { get; set; }
        public Measurement measurement { get; set; }
    }

    public class Origin
    {
        public Geo geo { get; set; }
        public List<string> type { get; set; }
        public Address address { get; set; }
    }

    public class Place
    {
        public Geo geo { get; set; }
        public List<string> type { get; set; }
        public Address address { get; set; }
    }

    public class Initiator
    {
        public string name { get; set; }
        public List<string> type { get; set; }
    }

    public class Receiver
    {
        public string name { get; set; }
        public List<string> type { get; set; }
    }

    public class PortOfEntry
    {
        public Geo geo { get; set; }
        public List<string> type { get; set; }
        public Address address { get; set; }
    }

    public class ReceiptLocation
    {
        public Geo geo { get; set; }
        public List<string> type { get; set; }
        public Address address { get; set; }
    }

    public class PortOfDestination
    {
        public Geo geo { get; set; }
        public List<string> type { get; set; }
        public Address address { get; set; }
    }

    public class NewProduct
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> type { get; set; }
        public Width width { get; set; }
        public Length length { get; set; }
        public Weight weight { get; set; }
        public string description { get; set; }
        public Manufacturer manufacturer { get; set; }
    }

    public class ConsumedProduct
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> type { get; set; }
        public Width width { get; set; }
        public Length length { get; set; }
        public Weight weight { get; set; }
        public string description { get; set; }
        public Manufacturer manufacturer { get; set; }
    }

    public class EventVC
    {
        public string id { get; set; }
        public List<string> type { get; set; }
        public Proof proof { get; set; }
        public string issuer { get; set; }

        [JsonProperty("@context")]
        public List<string> Context { get; set; }
        public DateTime issuanceDate { get; set; }
        public CredentialSubject credentialSubject { get; set; }
    }
    public class FootPrintEvents
    {
        public Co2eProduced Co2eProduced { get; set; }
    }
    public class Event
    {
        public string id { get; set; }
        public int createdById { get; set; }
        public string eventType { get; set; }
        public string txHash { get; set; }
        public DateTime txTimestamp { get; set; }
        public EventVC eventVC { get; set; }
        public string eventVCHash { get; set; }
        public DateTime createdAt { get; set; }
        public string s3DocumentsFolderPath { get; set; }
    }

    public class Owner
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

    public class PhoenixTrainerArgMax0
    {
        public int _0 { get; set; }
    }

    public class PhoenixTrainerSoftmax0
    {
        public double _0 { get; set; }
        public double _1 { get; set; }
    }

    public class PhoenixTrainerLogSoftmax0
    {
        public double _0 { get; set; }
        public double _1 { get; set; }
    }

    public class PhoenixEnsemblerTruediv0
    {
        public double _0 { get; set; }
        public double _1 { get; set; }
    }

    public class CarbonAnomaly
    {
        [JsonProperty("Phoenix/Trainer/ArgMax:0")]
        public PhoenixTrainerArgMax0 PhoenixTrainerArgMax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/Softmax:0")]
        public PhoenixTrainerSoftmax0 PhoenixTrainerSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/LogSoftmax:0")]
        public PhoenixTrainerLogSoftmax0 PhoenixTrainerLogSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Ensembler/truediv:0")]
        public PhoenixEnsemblerTruediv0 PhoenixEnsemblerTruediv0 { get; set; }
    }

    public class PhoenixSearchGenerator0LastDense24Logits0
    {
        public double _0 { get; set; }
        public double _1 { get; set; }
    }

    public class OriginAnomaly
    {
        [JsonProperty("Phoenix/Trainer/ArgMax:0")]
        public PhoenixTrainerArgMax0 PhoenixTrainerArgMax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/Softmax:0")]
        public PhoenixTrainerSoftmax0 PhoenixTrainerSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/LogSoftmax:0")]
        public PhoenixTrainerLogSoftmax0 PhoenixTrainerLogSoftmax0 { get; set; }

        [JsonProperty("Phoenix/search_generator_0/last_dense_24/logits:0")]
        public PhoenixSearchGenerator0LastDense24Logits0 PhoenixSearchGenerator0LastDense24Logits0 { get; set; }
    }

    public class PhoenixSearchGenerator0LastDense238081Logits0
    {
        public double _0 { get; set; }
        public double _1 { get; set; }
    }

    public class PriceAnomaly
    {
        [JsonProperty("Phoenix/Trainer/ArgMax:0")]
        public PhoenixTrainerArgMax0 PhoenixTrainerArgMax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/Softmax:0")]
        public PhoenixTrainerSoftmax0 PhoenixTrainerSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/LogSoftmax:0")]
        public PhoenixTrainerLogSoftmax0 PhoenixTrainerLogSoftmax0 { get; set; }

        [JsonProperty("Phoenix/search_generator_0/last_dense_238081/logits:0")]
        public PhoenixSearchGenerator0LastDense238081Logits0 PhoenixSearchGenerator0LastDense238081Logits0 { get; set; }
    }

    public class Anomaly
    {
        public CarbonAnomaly carbonAnomaly { get; set; }
        public OriginAnomaly originAnomaly { get; set; }
        public PriceAnomaly priceAnomaly { get; set; }
    }
    public class Co2eProduced
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class CarbonFootprintEvents
    {
        public List<FootPrintEvents> events { get; set; }
        public int co2eEmissionsInTonnes { get; set; }
    }

    public class CarbonFootprintDetails
    {
        public string role { get; set; }
        public DateTime endDate { get; set; }
        public DateTime startDate { get; set; }
        public CarbonFootprintEvents carbonFootprintEvents { get; set; }
    }

    public class CarbonFootprint
    {
        public int id { get; set; }
        public CarbonFootprintDetails carbonFootprintDetails { get; set; }
    }

    public class Active
    {
        public string id { get; set; }
        public ProductVC productVC { get; set; }
        public List<ProductSpec> productSpecs { get; set; }
        public string productVCHash { get; set; }
        public string status { get; set; }
        public Origin origin { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int ownerId { get; set; }
        public object custodianId { get; set; }
        public int creatorId { get; set; }
        public string txHash { get; set; }
        public DateTime txTimestamp { get; set; }
        public string releaseStatus { get; set; }
        public List<Event> events { get; set; }
        public Owner owner { get; set; }
        public object custodian { get; set; }
        public List<CarbonFootprint> carbonFootprints { get; set; }
        public Anomaly anomaly { get; set; }
        public List<object> sharedWith { get; set; }
        public object price { get; set; }
        public List<object> predescessors { get; set; }

        //public string id { get; set; }
        //public ProductVC productVC { get; set; }
        //public List<ProductSpec> productSpecs { get; set; }
        //public string productVCHash { get; set; }
        //public string status { get; set; }
        //public Origin origin { get; set; }
        //public DateTime createdAt { get; set; }
        //public DateTime updatedAt { get; set; }
        //public int ownerId { get; set; }
        //public int? custodianId { get; set; }
        //public int creatorId { get; set; }
        //public string txHash { get; set; }
        //public DateTime txTimestamp { get; set; }
        //public List<Event> events { get; set; }
        //public Owner owner { get; set; }
        //public bool shared { get; set; }
        //public Anomaly anomaly { get; set; }
          
    }

    public class Products
    {
        public List<Active> active { get; set; }
        public List<object> consumed { get; set; }
        public List<object> sharedWithMe { get; set; }
        public List<object> sharedWithMeDeclared { get; set; }
        public List<object> sharedWithMeNonDeclared { get; set; }
        public List<object> history { get; set; }
    }

    public class AllProductResponse
    {
        public TotalProductsCount totalProductsCount { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        public string category { get; set; }
        public Products products { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }

    public class AllPRoductEventsLineGraph
    {
        public string CreatedDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public int Count { get; set; }
    }

}
