using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class ReportView
    {
        public int Active { get; set; }
        public int Consumed { get; set; }
        public int History { get; set; }
        public int SharedWithme { get; set; }
        public List<ProductListView> lstProducts { get; set; }
    }

    public class ReportViewNew
    {
        public string Status { get; set; }
        public List<ProductListViewNew> lstProducts { get; set; }
    }
    public class ProductListViewNew
    {
        public string Productid { get; set; }
        public string Country { get; set; }
        public string TechnologyType { get; set; }
        public string Coil { get; set; }
        public string SerialNumber { get; set; }
        public string LiftNumber { get; set; }
        public DateTime IssuanceDate { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string Origin { get; set; }
        public string HsCode { get; set; }
        public string ProductType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
