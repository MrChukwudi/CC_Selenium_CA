using System;

namespace Selenium_CC_CA.Initialisers
{
    public class LogItem
    {
        string Status { get; set; }
        string Test { get; set; }
        string Outcome { get; set; }

        public LogItem(string status, string test, string outcome)
        {
            Status = status;
            Test = test;
            Outcome = outcome;
        }

        public static LogItem New(string status, string test, string outcome)
        {
            return new LogItem(status, test, outcome);
        }

        public string Print()
        {
            return string.Format("Result = {0}\nSub-test: {1}\nOutcome: {2}", Status, Test, Outcome); // ("Result = {0}\tSub-test: {1,30}\tOutcome: {2}", Status, Test, Outcome);
        }

        public string GetStatus()
        {
            return Status;
        }
    }
}
