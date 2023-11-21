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

        public static readonly TimeSpan ImplicitWait = TimeSpan.FromSeconds(5);
        public static readonly TimeSpan ShortWait = TimeSpan.FromSeconds(0);
        public static readonly TimeSpan WaitTime = TimeSpan.FromSeconds(8);
        public static readonly TimeSpan LongWait = TimeSpan.FromSeconds(30);

        internal static bool LoopEnabled;
        public const int Loop = 5;
        public const int loopcount = 1;
        internal static bool HeadlessRun;
        internal static bool testingRun;
        //public const int WhileLoop = 200;



        public static string UserNumber1 = "";
        public static string LoggedCompanyID = "";

        public readonly static string[] SuccessMsg = { "Success" };
        public readonly static string[] ErrorMsg = { "Error" };
        public readonly static string[] NotFound = { "Message Not Found" };
        public readonly static string[] Alert = { "Alert" };
        public readonly static string[] AlreadyExistsMsg = { "Item already exists" };

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

        //Time Delay Gap 
        public const int hour = 0;
        public const int minutes = 1;

    }
}
