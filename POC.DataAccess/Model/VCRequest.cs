using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class VCRequest
    {
        public string productName { get; set; }
        public string hsCode { get; set; }
        public VCFacility facility { get; set; }
        public VCManufacturer manufacturer { get; set; }
        public UnitofMeasure weight { get; set; }
        public UnitofMeasure length { get; set; }
        public UnitofMeasure width { get; set; }
        public List<VCObservation> observation { get; set; }
        public string technologyType { get; set; }
    }
    [Serializable]
    public class VCResponse
    {
        public string id { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class VCFacility
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public string addressCountry { get; set; }
    }

    public class VCManufacturer
    {
        public string name { get; set; }
    }

    public class UnitofMeasure
    {
        public string unit { get; set; }
        public string value { get; set; }
    }

    

    public class VCObservation
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
    }
}
