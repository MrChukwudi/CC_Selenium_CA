using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Selenium_CA.Test_Cases
{
    class Ping_Portal : Constants
    {
        // EEC Ping Test Case
        public static List<LogItem> EEC()
        {
            Log.Clear();

            //Log.AddTestStart("EEC Test Ping");
            Constants.BaseUrl = Constants.LiveUrl;
            Ping_Functions.Login(Constants.seleniumUserEmail, Constants.seleniumPwd); // 1st user login no need for this
            Ping_Functions.CloseCookieBR(); // handled in the 1st login pro no need for this

            Ping_Auto.Create();
            Ping_Auto.Acknowledge();

            Ping_Functions.LogOut();

            //LogAlert(testName);
            return Log.GetLog();
        }

        // MEA Ping Test Case
        public static List<LogItem> MEA()
        {
            Log.Clear();

            //Log.AddTestStart("OMN Test Ping");
            Constants.BaseUrl = Constants.MeaUrl;
            Ping_Functions.Login(Constants.seleniumUserEmail, Constants.seleniumPwd);
            Ping_Functions.CloseCookieBR();

            Ping.Create();            
            Ping.Acknowledge();

            Ping_Functions.LogOut();

            return Log.GetLog();
        }

        // KSA Ping Test Case
        public static List<LogItem> KSA()
        {
            Log.Clear();

            //Log.AddTestStart("KSA Test Ping");
            Constants.BaseUrl = Constants.KsaUrl;
            Ping_Functions.Login(Constants.seleniumUserEmail, Constants.seleniumPwd);
            Ping_Functions.CloseCookieBR();

            Ping_Auto.Create();
            Ping_Auto.Acknowledge();

            Ping_Functions.LogOut();

            return Log.GetLog();
        }

        // UAE Ping Test Case
        public static List<LogItem> UAE()
        {
            Log.Clear();

            //Log.AddTestStart("UAE Test Ping");
            Constants.BaseUrl = Constants.UaeUrl;
            Ping_Functions.Login(Constants.seleniumUserEmail, Constants.seleniumPwd);
            Ping_Functions.CloseCookieBR();

            Ping_Auto.Create();
            Ping_Auto.Acknowledge();

            Ping_Functions.LogOut();

            return Log.GetLog();
        }
    }
}
