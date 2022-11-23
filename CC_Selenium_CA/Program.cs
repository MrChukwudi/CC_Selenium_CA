using CC_Selenium_CA.Test_Cases;
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

            Directory.CreateDirectory(New_Directory_Path);

            Feedback(DateTime.Now, BrowserSelection.SelectBrowser(out Constants.driver), "selectBrowser");

            Log.Constructor();

            if (Constants.driver != null)
            {
                Feedback(DateTime.Now, Ping_Portal.EEC(), "EEC - Send & Acknowledge Ping");

                //Feedback(DateTime.Now, LogIn_Page.LogIn(), "Login");                            
                /*
                Constants.BaseUrl = Constants.LiveUrl;
                Feedback(DateTime.Now, Ping.Create(), "Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "Acknowledge Ping");

                Constants.BaseUrl = Constants.MeaUrl;
                Feedback(DateTime.Now, Ping.Create(), "Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "Acknowledge Ping");

                Constants.BaseUrl = Constants.KsaUrl;
                Feedback(DateTime.Now, Ping.Create(), "Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "Acknowledge Ping");

                Constants.BaseUrl = Constants.UaeUrl;
                Feedback(DateTime.Now, Ping.Create(), "Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "Acknowledge Ping");
                */
                //Feedback(DateTime.Now, SOS.Tracking_Data(), "SOS Tracking Data");
                //Feedback(DateTime.Now, SOS.Incident_Details(), "SOS Incident Details");
                //Feedback(DateTime.Now, SOS.Case_Notes(), "SOS Case Notes");
                //Feedback(DateTime.Now, SOS.Print(), "SOS Print");
                //Feedback(DateTime.Now, SOS.Close_SOS(), "Close SOS");

                // DO NOT UNCOMMENT THE NEXT FOUR LINES WITHOUT SUBSTENTIAL CODE CHANGE
                //Feedback(DateTime.Now, Report.Data_Export(), "Data Export"); // not possible at the moment due to file download/upload
                //Feedback(DateTime.Now, User.Import(), "Import User"); // not possible at the moment due to file download/upload
                //Feedback(DateTime.Now, User.DeleteImportedUser(), "Delete Imported User"); // not possible at the moment due to file download/upload
                //Feedback(DateTime.Now, LogIn_Page.Password_Reset(), "Password Reset"); // this needs user input so not good right now

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
