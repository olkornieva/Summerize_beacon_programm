using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using summarize_beacon;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        const string CURRENT_RECORD = "https://beacon.nist.gov/rest/record/";
        const string NEXT_RECORD = "https://beacon.nist.gov/rest/record/next/";
        const string LAST_RECORD = "https://beacon.nist.gov/rest/record/last";
        const long START_TIME_STAMP = 1378395540;
        
        [TestMethod]
        public void GetLastRecord()
        {
            var client = new RestClient(LAST_RECORD);
            var record = client.MakeRequest();

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public void GetByTimeStamp()
        {
            var timestamp = 1425050705;
            string endPoint = CURRENT_RECORD + timestamp;
            
            var client = new RestClient(LAST_RECORD);
            var record = client.MakeRequest();

            Assert.IsNotNull(record);
        }
    }
}
