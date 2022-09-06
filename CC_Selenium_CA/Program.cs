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
                Feedback(DateTime.Now, LogIn_Page.LogIn(), "Login");
                
                Feedback(DateTime.Now, Global_Config.Setup(), "Global Config");
                
                Feedback(DateTime.Now, Location.Create(), "Create Location");
                Feedback(DateTime.Now, Location.Edit(), "Edit Location");
                
                Feedback(DateTime.Now, Group.Create(), "Create Menu Access");
                Feedback(DateTime.Now, Group.Edit(), "Edit Menu Access");
                
                Feedback(DateTime.Now, Department.Create(), "Create Group");
                Feedback(DateTime.Now, Department.Edit(), "Edit Group");
                
                Feedback(DateTime.Now, Dept.Create(), "Create Department");
                Feedback(DateTime.Now, Dept.Edit(), "Edit Department");
                
                Feedback(DateTime.Now, User.Create(), "Create User");
                Feedback(DateTime.Now, User.Edit(), "Edit User");
                
                Feedback(DateTime.Now, Incident.Create(), "Create Incident");
                Feedback(DateTime.Now, Incident.Task_Create(), "Create Task");
                Feedback(DateTime.Now, Incident.Launch(), "Launch Incident");
                
                Feedback(DateTime.Now, Task.Accept(), "Accept Task"); 
                Feedback(DateTime.Now, Task.Checklist(), "Complete Checklist Items");
                Feedback(DateTime.Now, Task.Send_Update(), "Task Send Update");
                Feedback(DateTime.Now, Task.Delegation(), "Task Delegation");
                Feedback(DateTime.Now, Task.Reallocation(), "Task Reallocation");
                Feedback(DateTime.Now, Task.Complete(), "Task Completion");

                Feedback(DateTime.Now, Incident.IncidentDetails_Acknowledgements(), "Incident Details Acknowledgements");
                Feedback(DateTime.Now, Incident.IncidentDetails_Conference_Call(), "Conference Call");
                Feedback(DateTime.Now, Incident.IncidentDetails_Report(), "Incident Details Report");
                Feedback(DateTime.Now, Incident.IncidentDetails_Timeline(), "Incident Details Timeline");
                Feedback(DateTime.Now, Incident.IncidentDetails_Send_Update(), "Incident Send Update");
                Feedback(DateTime.Now, Incident.Incident_Audit(), "Incident Audit");
                
                Feedback(DateTime.Now, Incident.Deactivate(), "Deactivate Incident"); 
                Feedback(DateTime.Now, Incident.Edit(), "Edit Incident");

                Feedback(DateTime.Now, ISOP_Wizard.Create(), "Create ISOP Wizard");

                //SOP cannot be deleted if attached to an incident so delete the incident first
                Feedback(DateTime.Now, Incident.Delete(), "Delete Incident");
                Feedback(DateTime.Now, ISOP_Wizard.Delete(), "Delete ISOP Wizard");
  
                Feedback(DateTime.Now, Asset.Create(), "Create Asset");
                Feedback(DateTime.Now, Asset.Edit(), "Edit Asset");
                Feedback(DateTime.Now, Asset.Delete(), "Delete Asset");

                Feedback(DateTime.Now, Ping.Create(), "Create Ping");
                Feedback(DateTime.Now, Ping.Acknowledge(), "Acknowledge Ping");

                Feedback(DateTime.Now, Scheduler.Create(), "Schedule Ping");
                Feedback(DateTime.Now, Scheduler.Edit(), "Edit Scheduled Ping");
                Feedback(DateTime.Now, Scheduler.Delete(), "Delete Scheduled Ping");

                Feedback(DateTime.Now, Trigger.Create(), "Trigger Ping");
                Feedback(DateTime.Now, Trigger.Edit(), "Edit Triggered Ping");
                Feedback(DateTime.Now, Trigger.Delete(), "Delete Triggered Ping");
                
                Feedback(DateTime.Now, Company_Setup.Modifiy(), "Company Setup: Edit Company");
                Feedback(DateTime.Now, Company_Setup.Modifiy2(), "Company Setup: Revert Edit");
                
                Feedback(DateTime.Now, User.Delete(), "Delete User");
                
                Feedback(DateTime.Now, User.bulkAdd(), "Bulk Add");
                Feedback(DateTime.Now, User.Delete_bulkAdd(), "Delete Bulk Add");
                
                Feedback(DateTime.Now, Location.Delete(), "Delete Location");
                
                Feedback(DateTime.Now, Group.Delete(), "Delete Menu Access");
                
                Feedback(DateTime.Now, Department.Delete(), "Delete Group");
                
                Feedback(DateTime.Now, Dept.Delete(), "Delete Department");
               
                Feedback(DateTime.Now, Report.Ping_Report(), "Ping Report");
                Feedback(DateTime.Now, Report.Device_Report(), "Device Report");
                Feedback(DateTime.Now, Report.Incident_Report(), "Incident Report");
                Feedback(DateTime.Now, Report.Performance_Report(), "Performance Report");  // Need to change the time span from this month to custom 
                Feedback(DateTime.Now, Report.User_Report(), "User Report");
                
                Feedback(DateTime.Now, Response_Options.Edit(), "Edit Response Options");
                Feedback(DateTime.Now, Response_Options.Edit2(), "Reverse Response Options Edit");
                
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
