using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class Events
    {
    }
    public class EventView
    {
        public int Coil { get; set; }
        public List<POC.DataAccess.Model.EventDisplayList> events { get; set; }
    }
}
