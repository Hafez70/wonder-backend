using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTLicVerify.Models
{
    public class SaleItem
    {
        public long Id { get; set; }
        public string name { get; set; }
        public int number_of_sales { get; set; }
        public string author_username { get; set; }
        public string author_url { get; set; }
        public string url { get; set; }
        public string updated_at { get; set; }
        public string site { get; set; }
     
    }
}
