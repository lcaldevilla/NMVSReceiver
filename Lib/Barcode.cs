using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMVSReceiver.Lib
{
    public class Barcode
    {
        public string GTIN { get; set; }
        public string Serial { get; set; }
        public string Batch { get; set; }
        public string Expire { get; set; }
    }
}