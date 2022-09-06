using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_CC_CA.Initialisers
{
    /// <summary>
    /// Keep the logs of all the test case and sub tests and reurn them at the end of the test case.
    /// </summary>
    static class Log
    {
        public const string Pass = "PASS";
        public const string Fail = "FAIL";
        public const string Error = "ERROR";

        private static List<LogItem> CompleteLog { get; set; }

        public static void Constructor()
        {
            CompleteLog = new List<LogItem>();
        }

        public static void Entry(LogItem newEntry)
        {
            CompleteLog.Add(newEntry);
        }

        public static void Entry(string status, string test, string outcome)
        {
            CompleteLog.Add(LogItem.New(status, test, outcome));
        }

        public static void CALog(string status, string test, string outcome)
        {
            Console.WriteLine(status, test, outcome);
        }

        /// <summary>
        /// strings further than index 2 will be ignored.
        /// </summary>
        /// <param name="newEntry"></param>
        public static void Entry(string[] newEntry)
        {
            switch (newEntry.Length)
            {
                case 2:
                    CompleteLog.Add(LogItem.New(newEntry[0], newEntry[1], string.Empty));
                    break;
                case 1:
                    CompleteLog.Add(LogItem.New(newEntry[0], string.Empty, string.Empty));
                    break;
                case 0:
                    break;
                case 3:
                default:
                    CompleteLog.Add(LogItem.New(newEntry[0], newEntry[1], newEntry[2]));
                    break;
            }
        }

        public static List<LogItem> GetLog()
        {
            return CompleteLog;
        }

        public static void Clear()
        {
            CompleteLog.Clear();
        }

    }
}
