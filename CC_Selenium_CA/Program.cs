//using CC_Selenium_CA.Test_Cases;
using Selenium_CC_CA.Initialisers;
using System;
using System.IO;

namespace Selenium_CC_CA
{
    class Program : Constants
    {

        public static string New_Directory_Path = string.Format(Environment.CurrentDirectory + "\\Log_{0}_{1}_{2}\\", BaseUrl.Replace("https://", string.Empty), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("H-mm-ss"));
        public static string LogFile_Path = New_Directory_Path + DateTime.Now.ToString("H-mm-ss") + ".txt";
        public static DateTime TestStartTime = DateTime.Now;


        public static void Main(string[] args)
        {
            Functions.SetDebug(); // if in debug mode, set debug flag.
            Functions.LoopEnabeld();

            Directory.CreateDirectory(New_Directory_Path);

            Feedback(DateTime.Now, BrowserSelection.SelectBrowser(out Constants.driver), "selectBrowser");

            Log.Constructor();

            if (Constants.driver != null)
            {
                
                Constants.BaseUrl = Constants.LiveUrl;
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "EEC - Login");
                Feedback(DateTime.Now, Ping.Create(), "EEC - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "EEC - Acknowledge Ping");

                Constants.BaseUrl = Constants.MeaUrl;
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "OMN - Login");
                Feedback(DateTime.Now, Ping.Create(), "EEC - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "EEC - Acknowledge Ping");

                Constants.BaseUrl = Constants.UaeUrl;
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "UAE - Login");
                Feedback(DateTime.Now, Ping.Create(), "UAE - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "UAE - Acknowledge Ping");

                Constants.BaseUrl = Constants.KsaUrl;
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "KSA- Login");
                Feedback(DateTime.Now, Ping.Create(), "KSA - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "KSA - Acknowledge Ping");
                               
                if (LoopEnabeld == true)
                {
                    for (int i = 0; i <= loopcount; i++)
                    {
                        Console.WriteLine("Loop Number: " + i);
                        Constants.BaseUrl = Constants.LiveUrl;
                        Feedback(DateTime.Now, Ping.Create(), "EEC - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "EEC - Acknowledge Ping");

                        Constants.BaseUrl = Constants.MeaUrl;
                        Feedback(DateTime.Now, Ping.Create(), "MEA - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "MEA - Acknowledge Ping");

                        Constants.BaseUrl = Constants.UaeUrl;
                        Feedback(DateTime.Now, Ping.Create(), "UAE - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "UAE - Acknowledge Ping");

                        Constants.BaseUrl = Constants.KsaUrl;
                        Feedback(DateTime.Now, Ping.Create(), "KSA - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "KSA - Acknowledge Ping");
                    }
                }
                else
                {
                    // do nothing 
                }

                LogFile.Add("");
                LogFile.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                LogFile.Add("Automated Test Finished On " + DateTime.Now.ToLocalTime().ToString());
                LogFile.Add("Duration : " + (DateTime.Now - Program.TestStartTime));
                LogFile.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

                using (TextWriter tw = new StreamWriter(Program.LogFile_Path))
                {
                    foreach (var String in LogFile)
                    {
                        tw.WriteLine(String);
                    }
                    tw.Close();
                }
                
                driver.Quit();
                Console.ReadLine();
            }
        }

    }
}
