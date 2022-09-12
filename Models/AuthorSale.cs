using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTLicVerify.Models
{
    public class AuthorSale
    {
        public long Id { get; set; }
        public string amount { get; set; }
        public string sold_at { get; set; }
        public string license { get; set; }
        public string support_amount { get; set; }
        public string supported_until { get; set; }
        public SaleItem Item { get; set; }
        public List<EnvatoAccess> EnvatoAccess { get; set; }
        public string code { get; set; }
        public string email { get; set; }
        public int purchase_count { get; set; }

    }
}
