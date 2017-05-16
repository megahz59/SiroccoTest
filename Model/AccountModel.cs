using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Crm.Sdk.Samples.Model
{
    class AccountModel
    {
        public string AccountName { get; set; }
        public string AdressRow1 { get; set; }
        public string AdressRow2 { get; set; }
        public int Revenue { get; set; }
        public bool CreditOnHold { get; set; }
    }
}
