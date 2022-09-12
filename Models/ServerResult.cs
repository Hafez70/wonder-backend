using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTLicVerify.Models
{
    public class ServerResult
    {

        public ServerResult(bool res,string _description,string _stringResult)
        {
            boolResult = res;
            description = _description;
            stringResult = _stringResult;
        }

        public ServerResult(bool res, string _description)
        {
            boolResult = res;
            description = _description;
        }

        public ServerResult(string res, string _description)
        {
            stringResult = res;
            description = _description;
        }

        public string description { get; set; }
        public bool boolResult { get; set; }
        public string stringResult { get; set; }
     
    }
}
