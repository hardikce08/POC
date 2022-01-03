using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class InspectInitiator
    {
        public string name { get; set; }
    }

    public class InspectPlace
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public string addressCountry { get; set; }
    }

    public class InspectWeight
    {
        public string unit { get; set; }
        public string value { get; set; }
    }

    public class InspectWidth
    {
        public string unit { get; set; }
        public string value { get; set; }
    }

    public class InspectLength
    {
        public string unit { get; set; }
        public string value { get; set; }
    }

    public class InspectManufacturer
    {
        public string name { get; set; }
    }

    public class InspectProduct
    {
        public string id { get; set; }
        public string name { get; set; }
        public InspectWeight weight { get; set; }
        public InspectWidth width { get; set; }
        public InspectLength length { get; set; }
        public InspectManufacturer manufacturer { get; set; }
        public string description { get; set; }
    }

    public class InspectObservation
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
    }

    public class InspectEventAPIRequest
    {
        public InspectInitiator initiator { get; set; }
        public InspectPlace place { get; set; }
        public InspectProduct product { get; set; }
        public List<InspectObservation> observation { get; set; }
    }

    public class StartStorageAPIRequest
    {
        public string eventType { get; set; }
        public InspectInitiator initiator { get; set; }
        public InspectPlace place { get; set; }
        public InspectProduct product { get; set; }
        public List<InspectObservation> observation { get; set; }
        public InspectWidth storedWeight { get; set; }
    }
}
