using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTLicVerify.Models
{
    public class EnvatoAccess
    {

        public string activationCode { get; set; }
        public AuthorSale AuthorSale { get; set; }
        public long authorSaleId { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string machineId { get; set; }
        public string extenstion_version{ get; set; }
        public string extenstion_name{ get; set; }
        public string application_version { get; set; }
        public string application_name { get; set; }
        public string machine_name { get; set; }
        public int expires_in { get; set; }

        public DateTime? activated_at { get; set; }
        public DateTime? connected_at { get; set; }
        public long Id { get; set; }
        public bool activate { get; set; }
    }
}
