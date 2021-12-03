using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class Contract
    {
        public string Sender { get; set; }
        public int Id { get; set; }
        public ContractAPIRequest ContractRequest { get; set; }
    }
    public class ContractAPIRequest
    {
        public string sender { get; set; }
        public string receiver { get; set; }
        public string destination { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
    public class ContractUpdateRequest
    {
        public int contractId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
    public class ContractDeleteAPIRequest
    {
        public List<string> contractIds { get; set; }
    }
    public class ContractView
    {
        public int ContractId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CounterParty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
    public class ContracListView
    {
        public List<ContractView> lst { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Sender
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

    public class ContractReceiver
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

    public class AllContractReponse
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
        public Sender sender { get; set; }
        public ContractReceiver receiver { get; set; }
    }


}
