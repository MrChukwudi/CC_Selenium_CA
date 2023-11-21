//using CC_Selenium_CA.Test_Cases;
using Selenium_CC_CA.Initialisers;
using System;
using System.IO;
using System.Threading;

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
            Functions.LoopEnabled();
            Functions.HeadlessRun();
            Functions.testingRun();

            Directory.CreateDirectory(New_Directory_Path);

            Feedback(DateTime.Now, BrowserSelection.SelectBrowser(out Constants.driver), "SelectBrowser");
            Log.Constructor();

            //internet check goes here
            IsConnectedToInternet();

            if (Constants.driver != null)
            {                
                Console.WriteLine("------------------");
                Console.WriteLine("Setup Run");
                Console.WriteLine("------------------");

                Constants.BaseUrl = Constants.LiveUrl;
                Functions.Region("EEC");
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "EEC - Login");
                Feedback(DateTime.Now, Ping.Create(), "EEC - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "EEC - Acknowledge Ping");
                Feedback(DateTime.Now, LogIn_Page.LogOut(), "EEC - Log Out");
                
                Constants.BaseUrl = Constants.MeaUrl;
                Functions.Region("MEA");
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "OMN - Login");
                Feedback(DateTime.Now, Ping.Create(), "OMN - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "OMN - Acknowledge Ping");
                Feedback(DateTime.Now, LogIn_Page.LogOut(), "OMN - Log Out");

                Constants.BaseUrl = Constants.UaeUrl;
                Functions.Region("UAE");
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "UAE - Login");
                Feedback(DateTime.Now, Ping.Create(), "UAE - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "UAE - Acknowledge Ping");
                Feedback(DateTime.Now, LogIn_Page.LogOut(), "UAE - Log Out");

                Constants.BaseUrl = Constants.KsaUrl;
                Functions.Region("KSA");
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "KSA- Login");
                Feedback(DateTime.Now, Ping.Create(), "KSA - Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "KSA - Acknowledge Ping");
                Feedback(DateTime.Now, LogIn_Page.LogOut(), "KSA - Log Out");
                
                driver.Quit();
                BrowserSelection.CleanUp();

                if (LoopEnabled == true)
                {
                    for (int i = 1; i <= loopcount; i++) 
                    {
                        int one = 1;
                        Console.WriteLine("------------------");
                        Console.WriteLine("Loop Number: " + (i+one));
                        Console.WriteLine("------------------");

                        // Timer goes here
                        Timer();

                        // Start Browser (Again)
                        Feedback(DateTime.Now, BrowserSelection.SelectBrowser(out Constants.driver), "SelectBrowser");

                        //internet check goes here
                        IsConnectedToInternet();

                        //Constants.BaseUrl = Constants.PpUrl;
                        //Feedback(DateTime.Now, LogIn_Page.LogIn(), "pp - Login");
                        //Feedback(DateTime.Now, Ping.Create(), "pp - Create Ping");
                        //Feedback(DateTime.Now, Ping.Acknowledge(), "pp - Acknowledge Ping");
                        //Feedback(DateTime.Now, LogIn_Page.LogOut(), "pp - Log Out");

                        Constants.BaseUrl = Constants.LiveUrl;
                        Functions.Region("EEC");
                        Feedback(DateTime.Now, LogIn_Page.LogIn(), "EEC - Login");
                        Feedback(DateTime.Now, Ping.Create(), "EEC - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "EEC - Acknowledge Ping");
                        Feedback(DateTime.Now, LogIn_Page.LogOut(), "EEC - Log Out");

                        Constants.BaseUrl = Constants.MeaUrl;
                        Functions.Region("MEA");
                        Feedback(DateTime.Now, LogIn_Page.LogIn(), "OMN - Login");
                        Feedback(DateTime.Now, Ping.Create(), "OMN - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "OMN - Acknowledge Ping");
                        Feedback(DateTime.Now, LogIn_Page.LogOut(), "OMN - Log Out");

                        Constants.BaseUrl = Constants.UaeUrl;
                        Functions.Region("UAE");
                        Feedback(DateTime.Now, LogIn_Page.LogIn(), "UAE - Login");
                        Feedback(DateTime.Now, Ping.Create(), "UAE - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "UAE - Acknowledge Ping");
                        Feedback(DateTime.Now, LogIn_Page.LogOut(), "UAE - Log Out");

                        Constants.BaseUrl = Constants.KsaUrl;
                        Functions.Region("KSA");
                        Feedback(DateTime.Now, LogIn_Page.LogIn(), "KSA- Login");
                        Feedback(DateTime.Now, Ping.Create(), "KSA - Create Ping");
                        Feedback(DateTime.Now, Ping.Acknowledge(), "KSA - Acknowledge Ping");
                        Feedback(DateTime.Now, LogIn_Page.LogOut(), "KSA - Log Out");

                        driver.Quit();
                        BrowserSelection.CleanUp();
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

                //driver.Quit(); 
                BrowserSelection.CleanUp();
                Console.ReadLine(); // Added to read the console logs 
            }
        }

    }
}
