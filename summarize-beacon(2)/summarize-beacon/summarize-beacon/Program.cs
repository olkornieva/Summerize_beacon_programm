using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace summarize_beacon
{
    class Program
    {
        const string CURRENT_RECORD = "https://beacon.nist.gov/rest/record/";
        const string NEXT_RECORD = "https://beacon.nist.gov/rest/record/next/";
        const string LAST_RECORD = "https://beacon.nist.gov/rest/record/last";
        const long START_TIME_STAMP = 1378395540;

        static DateTime GetDateTime(string text)
        {
            var monthPattern = @"([0-9]*) month";
            var dayPattern = @"([0-9]*) day";
            var hourPattern = @"([0-9]*) hour";

            var monthRegex = new Regex(monthPattern);
            var dayRegex = new Regex(dayPattern);
            var hourRegex = new Regex(hourPattern);

            var month = Convert.ToInt32(monthRegex.Match(text).Groups[1].Value);
            var day = Convert.ToInt32(dayRegex.Match(text).Groups[1].Value);
            var hour = Convert.ToInt32(hourRegex.Match(text).Groups[1].Value);

            return DateTime.Now.AddMonths(-month).AddDays(-day).AddHours(-hour);
        }

        //Parse parameters in command line
        static int ArgPos(string str, string[] args)
        {
            str = str.ToLower();
            for (int a = 0; a < args.Length; a++)
            {
                if (str == args[a].ToLower())
                {
                    if (a == args.Length - 1)
                    {
                        Console.WriteLine("Argument missing for {0}", str);
                        return -1;
                    }
                    return a;
                }
            }
            return -1;
        }

        static void Main(string[] args)
        {
            string strFrom = null, strTo = null;
            int i;
            if ((i = ArgPos("--from", args)) >= 0) strFrom = args[i + 1];
            if ((i = ArgPos("--to", args)) >= 0) strTo = args[i + 1];


            var resDictionary = new Dictionary<char, int>();

            if (!string.IsNullOrEmpty(strFrom) && !string.IsNullOrEmpty(strTo))
            {
                var from = GetDateTime(strFrom).ToUniversalTime();
                var to = GetDateTime(strTo).ToUniversalTime();

                if (from < FromUnixTime(START_TIME_STAMP) || from < FromUnixTime(START_TIME_STAMP))
                    throw new ArgumentException("Specified time should be bigger than Unix epoch time 1378395540");

                string endPoint = CURRENT_RECORD + ToUnixTime(from);
                var client = new RestClient(endPoint);
                var record = client.MakeRequest();

                PopulateDictionary(resDictionary, record.OutputValue);

                while (FromUnixTime(record.TimeStamp) <= to)
                {
                    record = GetNextRecord(record);
                    PopulateDictionary(resDictionary, record.OutputValue);
                }
            }
            else 
            {
                var client = new RestClient(LAST_RECORD);
                var record = client.MakeRequest();

                PopulateDictionary(resDictionary, record.OutputValue);
            }

            foreach (var pair in resDictionary)
            {
                Console.Out.WriteLine(string.Format("{0},{1}", pair.Key, pair.Value));
            }
        }

        public static void PopulateDictionary(Dictionary<char, int> resDictionary, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (resDictionary.ContainsKey(ch))
                {
                    resDictionary[ch]++;
                }
                else
                {
                    resDictionary.Add(ch, 1);
                }
            }
        }


        public static Record GetNextRecord(Record previousRecord)
        {
            string endPoint = NEXT_RECORD + previousRecord.TimeStamp;
            var client = new RestClient(endPoint);
            var record = client.MakeRequest();

            if (record.PreviousOutputValue != previousRecord.OutputValue)
                throw new ArgumentException(string.Format("PreviousOutputValue and current record outputValue mismatch. PreviousOutputValue = {0}, OutputValue = {1}", record.PreviousOutputValue, previousRecord.OutputValue));

            return record;
        }
        
        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
