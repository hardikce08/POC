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
}
