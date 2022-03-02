using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class ProductMst
    {
        public int ProductId { get; set; }

        public string Owner { get; set; }

        public string id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ProductType { get; set; }

        public string Origin { get; set; }

        public string Status { get; set; }

        public string LastEvent { get; set; }

        public int? Coil { get; set; }

        public string SerialNumber { get; set; }

        public string LiftNumber { get; set; }

        public string CurrentLocation { get; set; }
        public DateTime IssuanceDate { get; set; }

        public DateTime? ProductionDate { get; set; }

        public string TechnologyType { get; set; }

        public string HsCode { get; set; }
    }
    public class ProductSummary
    {
        public int Id { get; set; }

        public int? Active { get; set; }

        public int? Consumed { get; set; }

        public int? History { get; set; }

        public int? SharedWithMe { get; set; }
    }
}
