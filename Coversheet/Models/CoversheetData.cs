using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSG.ProcessAutomation.Coversheet.WebApi.Models
{
    public class CoversheetData
    {
        private readonly string barcodeYstring = "Y0115010620100000000000000000000001100000000";
        private string barcodeY;
        public CoversheetData()
        {

        }

        public string JobId { get; set; }
        public int StartSeq { get; set; }
        public string Match { get; set; }
        public int EndSeq { get; set; }
        public string Description { get; set; }
        public string FirstSeqName { get; set; }
        public string LastSeqName { get; set; }
        public string Duplex { get; set; }
        public string PageSize { get; set; }
        public string Inventory { get; set; }
        public string BarcodeXString
        {
            get => "X" + JobId + StartSeq.ToString("D6") + EndSeq.ToString("D6") + Match.PadLeft(10, '0');

        }

        public string BarcodeYString
        {
            get
            {
                return string.IsNullOrEmpty(barcodeY) ? barcodeYstring : barcodeY;
            }
            set { barcodeY = value; }

        }
    }
}