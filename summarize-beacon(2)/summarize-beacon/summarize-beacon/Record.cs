using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace summarize_beacon
{
    [XmlType("record")]
    public class Record
    {
        [System.Xml.Serialization.XmlElementAttribute("version")]
        public string Version { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("frequency")]
        public int Frequency { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("timeStamp")]
        public long TimeStamp { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("seedValue")]
        public string SeedValue { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("previousOutputValue")]
        public string PreviousOutputValue { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("signatureValue")]
        public string SignatureValue { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("outputValue")]
        public string OutputValue { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("statusCode")]
        public string StatusCode { get; set; }
    }
}
