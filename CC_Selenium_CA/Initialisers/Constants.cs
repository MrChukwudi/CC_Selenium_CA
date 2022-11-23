using System;

namespace Selenium_CC_CA.Initialisers
{
    public class Constants : Functions
    {
        internal static string CompanyID;
        internal static string Email;
        internal static string Password;
        internal static bool UseSso;
        public static bool IsDebug;

        internal static string BaseUrl = "https://dev-portal.crises-control.com";
        internal const string ProtoUrl = "http://prototypes.transputec.net/ccportal";
        internal const string DevUrl = "https://dev-portal.crises-control.com";
        internal const string LiveUrl = "https://portal.crises-control.com";
        internal const string GUrl = "https://g-portal.crises-control.com";
        internal const string PpUrl = "https://pp-portal.crises-control.com";
        internal const string MeaUrl = "https://mea-portal.crises-control.com";
        internal const string KsaUrl = "https://ksa-portal.crises-control.com";
        internal const string UaeUrl = "https://uae-portal.crises-control.com";
        internal const string AzUrl = "https://az-portal.crises-control.com";
        //internal const string ProtoAdminUrl = "http://prototypes.transputec.net/admin.crises-control.com/";

        public static readonly TimeSpan ImplicitWait = TimeSpan.FromSeconds(5);
        public static readonly TimeSpan ShortWait = TimeSpan.FromSeconds(0);
        public static readonly TimeSpan WaitTime = TimeSpan.FromSeconds(8);
        public static readonly TimeSpan LongWait = TimeSpan.FromSeconds(30);

        public const int Loop = 5;
        //public const int WhileLoop = 200;

        public static string UserNumber1 = "";
        public static string UserNumber2 = "Vitto Padre"; //MEA
        public const string UserNumber3 = "Carlvin Garcia"; //MEA
        //public const string UserNumber3 = "Ali Legris";

        public readonly static string[] SuccessMsg = { "Success" };
        public readonly static string[] ErrorMsg = { "Error" };
        public readonly static string[] NotFound = { "Message Not Found" };
        public readonly static string[] Alert = { "Alert" };
        public readonly static string[] AlreadyExistsMsg = { "Item already exists" };

        public const string SimpleText = "Lorem ipsum";

        internal static string NewPing = "Transputec's Automated Ping with Audio Attachment";
        internal static string NewScheduledPingAfterEdit = "Edited Automated Scheduled Ping";
        internal static string ScheduledPingJobName = "Automated Scheduled Ping";
        internal static string NewTriggeredPingAfterEdit = "Edited Automated Triggered Ping";
        internal static string TriggeredPingJobName = "Automated Triggered Ping";
        internal static string NewIncident = "Transputec's Automated incident";
        internal static string NewLocation = "Transputec's Automated Location";
        internal static string NewGroup = "Transputec's Automated Group";
        internal static string NewDepartment = "Automated Department";
        internal static string NewDepartmentAfterEdit = "Transputec's Automated Department";
        internal static string NewDept = "Automated Department";
        internal static string NewDeptAfterEdit = "Transputec's Automated Department";
        internal const string NewAsset = "Transputec's Automated Asset";
        internal static string NewChecklistItem1 = "item 1";
        internal static string NewChecklistItem2 = "item 2";
        internal static string TimeZone = "London";
        //internal const string NewISOP_Wizard = "(TEST) Aircraft Damage";

        public const string NewUserFirstName = "Automated ";
        public const string NewUserFirstNameAfterEdit = "Transputec Automated ";
        public const string NewUserLastName = "User";
        public const string NewUserEmail = "transputec@test.com";
        public const string NewUserMobile = "1234567890";
        public const string ImportUsers = "Importing Users Test";

        public const string NewTask = "Transputec's Automated Task";
        public const string NewTaskAfterEdit = "Transputec's Automated Task (Edit)";
        public const string NewSecondTask = "Transputec's Automated Second Task Test";
        public const string KeyContacts = "Key";

        public const string SOSname = "DEV - SOS Request";
        public const string SOSAlertUpdate = "This Is A Test";

        // Selenium User Details
        internal const string seleniumFN = "Selenium ";
        internal const string seleniumLN = "User";
        internal const string seleniumUserEmail = "seleniumuser@crises-control.com";
        internal const string seleniumPwd = "@ut0_Ping-User";

        // Selenium User Details 
        internal static string PingGroup = "Selenium Gpr.";
        internal static string PingLocation = "Selenium Loc.";
        internal static string PingDepartment = "Selenium Dept.";

        // Ping Config Message
        internal static string PingMessage = "Transputec's Automated Ping.";

        // Login URL
        public const string LoginPage = "/users/login";

    }
}
