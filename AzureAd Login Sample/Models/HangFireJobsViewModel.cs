using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POC.Models
{
    public class HangFireJobsViewModel
    {
        public Dictionary<int, string> Actions
        {
            get
            {
                Dictionary<int, string> data = new Dictionary<int, string>();

                var names = Enum.GetNames(typeof(JobEnum));

                var list = new List<string>(names);
                list.Sort();

                foreach (var item in list)
                {
                    data.Add(
                        (int)Enum.Parse(typeof(JobEnum), item),
                        item.Replace("_", " "));
                }

                return data;
            }
        }

        public enum JobEnum
        {
            LoadProductData = 1,
            LoadVCandCreateProduct = 2
        }
    }

}