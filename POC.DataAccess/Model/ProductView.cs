using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class ProductView
    {
        public string id { get; set; }
        public ProductResult result { get; set; }
    }

    public class ProductResult
    {
        public string id { get; set; }
        public string ProductName { get; set; }
        public string Technology { get; set; }
        public string Description { get; set; }
        public string HSCode { get; set; }
        public string FacilityAddressCountry { get; set; }
        public string Manufacturer { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Grade { get; set; }
        public DateTime issuanceDate { get; set; }
    }

    public class ProductDetail
    {
        public string ObservationDescription { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
    }
    public class ProductDetailView
    {
        public Active result { get; set; }
    }
    public class ProductListView
    {
        public string id { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProductType { get; set; }
        public string Origin { get; set; }
        public string LastEvent { get; set; }
        public string Status { get; set; }
        public string SharedProduct { get; set; }
        public string Coil { get; set; }
        public string SerialNumber { get; set; }
        public string LiftNumber { get; set; }
        public string CurrentLocation { get; set; }
        public string CreatedOnString { get; set; }

    }
    public class ProductListViewV2
    {
       
        public string Owner { get; set; }
        public string id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProductType { get; set; }
        public string Origin { get; set; }
        public string Status { get; set; }
        public string LastEvent { get; set; }

        public string HsCode { get; set; }
        public DateTime IssuanceDate { get; set; }

        public DateTime? ProductionDate { get; set; }

        public string TechnologyType { get; set; }
    }
    public class LineProductCount
    {
        public string Date { get; set; }
        public int Count { get; set; }
    }
}
