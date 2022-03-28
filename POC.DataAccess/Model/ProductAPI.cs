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
        public Address address { get; set; }
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
        public Inspection inspection { get; set; }
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

        public Manufacturer manufacturer { get; set; }
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

    //public class PhoenixTrainerArgMax0
    //{
    //    public int _0 { get; set; }
    //}

    //public class PhoenixTrainerSoftmax0
    //{
    //    public double _0 { get; set; }
    //    public double _1 { get; set; }
    //}

    //public class PhoenixTrainerLogSoftmax0
    //{
    //    public double _0 { get; set; }
    //    public double _1 { get; set; }
    //}

    //public class PhoenixEnsemblerTruediv0
    //{
    //    public double _0 { get; set; }
    //    public double _1 { get; set; }
    //}

    public class CarbonAnomaly
    {
        [JsonProperty("Phoenix/Trainer/ArgMax:0")]
        public object PhoenixTrainerArgMax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/Softmax:0")]
        public object PhoenixTrainerSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/LogSoftmax:0")]
        public object PhoenixTrainerLogSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Ensembler/truediv:0")]
        public object PhoenixEnsemblerTruediv0 { get; set; }
    }

    //public class PhoenixSearchGenerator0LastDense24Logits0
    //{
    //    public double _0 { get; set; }
    //    public double _1 { get; set; }
    //}

    public class OriginAnomaly
    {
        [JsonProperty("Phoenix/Trainer/ArgMax:0")]
        public object PhoenixTrainerArgMax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/Softmax:0")]
        public object PhoenixTrainerSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/LogSoftmax:0")]
        public object PhoenixTrainerLogSoftmax0 { get; set; }

        [JsonProperty("Phoenix/search_generator_0/last_dense_24/logits:0")]
        public object PhoenixSearchGenerator0LastDense24Logits0 { get; set; }
    }

    //public class PhoenixSearchGenerator0LastDense238081Logits0
    //{
    //    public double _0 { get; set; }
    //    public double _1 { get; set; }
    //}

    public class PriceAnomaly
    {
        [JsonProperty("Phoenix/Trainer/ArgMax:0")]
        public object PhoenixTrainerArgMax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/Softmax:0")]
        public object PhoenixTrainerSoftmax0 { get; set; }

        [JsonProperty("Phoenix/Trainer/LogSoftmax:0")]
        public object PhoenixTrainerLogSoftmax0 { get; set; }

        [JsonProperty("Phoenix/search_generator_0/last_dense_238081/logits:0")]
        public object PhoenixSearchGenerator0LastDense238081Logits0 { get; set; }
    }
    public class PriceAnomalyV2
    {
        public string productId { get; set; }
        public string prediction { get; set; }
        public object correction { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class Anomaly
    {
        //public CarbonAnomaly carbonAnomaly { get; set; }
        //public OriginAnomaly originAnomaly { get; set; }
        //public PriceAnomaly priceAnomaly { get; set; }
        public PriceAnomalyV2 priceAnomaly { get; set; }
    }
    public class Co2eProduced
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class CarbonFootprintEvents
    {
        public List<string> events { get; set; }
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
        public EmissionCarbonFootprintDetails carbonFootprintDetails { get; set; }
    }
    public class CarbonEstimate
    {
        public int id { get; set; }
        public string carbonEventType { get; set; }
        public double estimate { get; set; }
        public DateTime createdAt { get; set; }
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

        public List<CarbonEstimate> carbonEstimates { get; set; }
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
    public class EventDisplayList
    {
        public object Latitude { get; set; }
        public object Longitude { get; set; }
        public string EventType { get; set; }
        public string InitiatorName { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string addressCountry { get; set; }
        public DateTime issuanceDate { get; set; }
        public object Latitude2 { get; set; }
        public object Longitude2 { get; set; }
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
        public DateTime FilterDate { get; set; }

        public string Status { get; set; }
    }

    public class AllProductBarGraph
    {
        public string CreatedDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public int Count { get; set; }
        public DateTime FilterDate { get; set; }

        public string Status { get; set; }
    }

    #region Event Emission
    public class Amount
    {
        public List<string> type { get; set; }
        public string unitCode { get; set; }
        public string value { get; set; }
    }

    public class ProcessMaterialsDetail
    {
        public string processMaterial { get; set; }
        public Amount amount { get; set; }
    }

    public class ProcessEmission
    {
        public int co2EmissionsInTonnes { get; set; }
        public int co2eEmissionsInTonnes { get; set; }
        public List<ProcessMaterialsDetail> processMaterialsDetails { get; set; }
    }

    public class FuelUsage
    {
        public List<string> type { get; set; }
        public string unitCode { get; set; }
        public string value { get; set; }
    }

    public class FuelTypesDetail
    {
        public string fuelType { get; set; }
        public FuelUsage fuelUsage { get; set; }
    }

    public class StationaryCombustion
    {
        public int co2EmissionsInTonnes { get; set; }
        public int co2eEmissionsInTonnes { get; set; }
        public int ch4EmissionsInTonnes { get; set; }
        public int no2EmissionsInTonnes { get; set; }
        public List<FuelTypesDetail> fuelTypesDetails { get; set; }
    }

    public class MobileCombustionUsage
    {
        public int co2EmissionsInTonnes { get; set; }
        public int co2eEmissionsInTonnes { get; set; }
        public int ch4EmissionsInTonnes { get; set; }
        public int no2EmissionsInTonnes { get; set; }
        public string fuelType { get; set; }
        public FuelUsage fuelUsage { get; set; }
    }

    public class DistanceDetail
    {
        public string vehicleType { get; set; }
        public string fuelType { get; set; }
        public string unitCode { get; set; }
        public string value { get; set; }
    }

    public class MobileCombustionDistance
    {
        public int co2EmissionsInTonnes { get; set; }
        public int co2eEmissionsInTonnes { get; set; }
        public int ch4EmissionsInTonnes { get; set; }
        public int no2EmissionsInTonnes { get; set; }
        public List<DistanceDetail> distanceDetails { get; set; }
    }

    public class ElectricityConsumption
    {
        public List<string> type { get; set; }
        public string unitCode { get; set; }
        public string value { get; set; }
    }

    public class PurchasedElectricity
    {
        public int co2EmissionsInTonnes { get; set; }
        public int co2eEmissionsInTonnes { get; set; }
        public string subregion { get; set; }
        public ElectricityConsumption electricityConsumption { get; set; }
    }

    public class EmissionCarbonFootprintEvents
    {
        public int co2eEmissionsInTonnes { get; set; }
        public List<EventNew> events { get; set; }
    }


    public class SaveEmissionRequest
    {
        public string productid { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string co2emissionintonnes { get; set; }
        public string Event { get; set; }

    }
    public class EmissionCarbonFootprintDetails
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string role { get; set; }
        public ProcessEmission processEmission { get; set; }
        public StationaryCombustion stationaryCombustion { get; set; }
        public MobileCombustionUsage mobileCombustionUsage { get; set; }
        public MobileCombustionDistance mobileCombustionDistance { get; set; }
        public PurchasedElectricity purchasedElectricity { get; set; }
        public EmissionCarbonFootprintEvents carbonFootprintEvents { get; set; }
    }

    public class EmissionEventRequest
    {
        public string productId { get; set; }
        public EmissionCarbonFootprintDetails carbonFootprintDetails { get; set; }
    }
    public class MillTestValues
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
    #endregion
    #region MillTest
    public class MillTestAddress
    {
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public string addressCountry { get; set; }
    }

    public class MillTestManufacturer
    {
        public string name { get; set; }
        public MillTestAddress address { get; set; }
    }

    public class MillTestPlace
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public string addressCountry { get; set; }
    }

    public class MillTestObservation
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
    }

    public class MillTestAPIRequest
    {
        public string productId { get; set; }
        public string certifier { get; set; }
        public MillTestManufacturer manufacturer { get; set; }
        public string specification { get; set; }
        public MillTestPlace place { get; set; }
        public string originalCountryOfMeltAndPour { get; set; }
        public List<MillTestObservation> observation { get; set; }
    }
    #endregion

    #region Product Save Emission Request
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Co2eProducedNew
    {
        public List<string> type { get; set; }
        public string unitCode { get; set; }
        public string value { get; set; }
    }

    public class EventNew
    {
        public string @event { get; set; }
        public Co2eProducedNew co2eProduced { get; set; }
    }

    public class CarbonFootprintEventsNew
    {
        public int co2eEmissionsInTonnes { get; set; }
        public List<EventNew> events { get; set; }
    }

    public class CarbonFootprintDetailsNew
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string role { get; set; }
        public CarbonFootprintEventsNew carbonFootprintEvents { get; set; }
    }

    public class ProductCarbonFootPrint
    {
        public string productId { get; set; }
        public CarbonFootprintDetailsNew carbonFootprintDetails { get; set; }
    }


    #endregion

    #region New All PRoduct API response class
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TotalProductsCountV2
    {
        public int active { get; set; }
        public int consumed { get; set; }
        public int sharedWithMe { get; set; }
        public int sharedWithMeDeclared { get; set; }
        public int sharedWithMeNonDeclared { get; set; }
        public int history { get; set; }
    }

    public class PropertyV2
    {
        public string name { get; set; }
        public List<string> type { get; set; }
        public string description { get; set; }
    }

    public class MeasurementV2
    {
        public List<string> type { get; set; }
        public string value { get; set; }
        public string unitCode { get; set; }
    }

    public class ProductSpecV2
    {
        public List<string> type { get; set; }
        public PropertyV2 property { get; set; }
        public MeasurementV2 measurement { get; set; }
    }

    public class GeoV2
    {
        public List<string> type { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class AddressV2
    {
        public List<string> type { get; set; }
        public string postalCode { get; set; }
        public string addressRegion { get; set; }
        public string streetAddress { get; set; }
        public string addressCountry { get; set; }
        public string addressLocality { get; set; }
    }

    public class OriginV2
    {
        public GeoV2 geo { get; set; }
        public List<string> type { get; set; }
        public AddressV2 address { get; set; }
        public string globalLocationNumber { get; set; }
    }

    public class OwnerV2
    {
        public int id { get; set; }
        public string did { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int mill { get; set; }
        public string role { get; set; }
        public string clientType { get; set; }
        public string backendLink { get; set; }
    }

    public class PricePredictionV2
    {
        public string productId { get; set; }
        public string prediction { get; set; }
        public object correction { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class PlaceV2
    {
        public GeoV2 geo { get; set; }
        public List<string> type { get; set; }
        public AddressV2 address { get; set; }
        public string globalLocationNumber { get; set; }
    }

    public class EventV2
    {
        public string id { get; set; }
        public string eventType { get; set; }
        public DateTime createdAt { get; set; }
        public PlaceV2 place { get; set; }
    }

    public class AnomalyV2
    {
        public string priceAnomaly { get; set; }
    }

    public class ActiveV2
    {
        public string id { get; set; }
        public List<ProductSpecV2> productSpecs { get; set; }
        public string productVCHash { get; set; }
        public int ownerId { get; set; }
        public int creatorId { get; set; }
        public object custodianId { get; set; }
        public object shipmentId { get; set; }
        public string status { get; set; }
        public OriginV2 origin { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string txHash { get; set; }
        public DateTime txTimestamp { get; set; }
        public string releaseStatus { get; set; }
        public OwnerV2 owner { get; set; }
        public PricePredictionV2 pricePrediction { get; set; }
        public List<EventV2> events { get; set; }
        public string weight { get; set; }
        public string unit { get; set; }
        public List<string> productType { get; set; }
        public string productName { get; set; }
        public string hsCode { get; set; }
        public bool shared { get; set; }
        public AnomalyV2 anomaly { get; set; }
    }

    public class ProductsV2
    {
        public List<ActiveV2> active { get; set; }
        public List<object> consumed { get; set; }
        public List<object> sharedWithMe { get; set; }
        public List<object> sharedWithMeDeclared { get; set; }
        public List<object> sharedWithMeNonDeclared { get; set; }
        public List<object> history { get; set; }
    }

    public class AllProductResponseV2
    {
        public TotalProductsCountV2 totalProductsCount { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        public string category { get; set; }
        public ProductsV2 products { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }
    public class AllProductResponseV3
    {
        public TotalProductsCountV2 totalProductsCount { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        public string category { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }
    #endregion
}
