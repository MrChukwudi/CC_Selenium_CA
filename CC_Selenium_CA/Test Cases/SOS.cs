using OpenQA.Selenium;

namespace Selenium_CC_CA.Initialisers
{
    class SOS : Constants
    {
        public static string[] Edit()
        {
            CheckUrl(BaseUrl + "/incident/sossetup");

            WriteInElement(By.Id("Description"), "");
            ClickElement(By.XPath("//button[contains(@id, 'btn_submit')]"));

            return FindAlert();
        }

        public static string[] Tracking_Data()
        {
            CheckUrl(BaseUrl + "/activeincident/sosalerts");

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), UserNumber1, By.XPath("//*[@id='tbl_sos_alert']/tbody/tr/td[2]")))
            {
                do
                {
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-success btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath("//*[contains(text(),'Tracking Data')]"),true));

                ClickElement(By.XPath("//*[contains(text(),'Tracking Data')]"));
            }
            return FindAlert();
        }
        public static string[] Incident_Details()
        {
            CheckUrl(BaseUrl + "/activeincident/sosalerts");

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), UserNumber1, By.XPath("//*[@id='tbl_sos_alert']/tbody/tr/td[2]")))
            {
                do
                {
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-success btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath("//*[contains(text(),'Details')]"),true));

                ClickElement(By.XPath("//*[contains(text(),'Details')]"));

                if (TableSearch(By.XPath("//*[@id='tbl_sos_details_filter']/label/input"), SOSname, By.XPath("//*[@id='tbl_sos_details']/tbody/tr[1]/td[2]")))
                {
                    do
                    {
                        ClickElement(By.XPath("//*[@id='tbl_sos_details']/tbody/tr[1]/td[6]/div/button"));
                    }
                    while (!IsElementDisplayed(By.XPath("//*[contains(text(),'Send Update')]"),true));

                    ClickElement(By.XPath("//*[contains(text(),'Send Update')]"));
                    WriteInElement(By.Id("InciMsg"), SOSAlertUpdate);
                    ClickElement(By.Id("btn_send_user_update"));

                    ClickElement(By.Id("sosdetailsback')]"));
                    return SuccessMsg;
                }
            }
            return ErrorMsg;
        }
        public static string[] Case_Notes()
        {
            CheckUrl(BaseUrl + "/activeincident/sosalerts");

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), UserNumber1, By.XPath("//*[@id='tbl_sos_alert']/tbody/tr/td[2]")))
            {
                do
                {
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-success btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath("//*[contains(text(),'Case Notes')]"),true));

                ClickElement(By.XPath("//*[contains(text(),'Case Notes')]"));
            }
            return FindAlert();
        }
        public static string[] Print()
        {
            CheckUrl(BaseUrl + "/activeincident/sosalerts");

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), UserNumber1, By.XPath("//*[@id='tbl_sos_alert']/tbody/tr/td[2]")))
            {
                do
                {
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-success btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath("//*[contains(text(),'Print')]"),true));

                ClickElement(By.XPath("//*[contains(text(),'Print')]"));
            }
            return FindAlert();
        }
        public static string[] False_Alert()
        {
            CheckUrl(BaseUrl + "/activeincident/sosalerts");

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), UserNumber1, By.XPath("//*[@id='tbl_sos_alert']/tbody/tr/td[2]")))
            {
                do
                {
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-success btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath("//*[contains(text(),'False Alert')]"),true));

                ClickElement(By.XPath("//*[contains(text(),'False Alert')]"));
            }
            return FindAlert();
        }
        public static string[] Close_SOS()
        {
            CheckUrl(BaseUrl + "/activeincident/sosalerts");

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), UserNumber1, By.XPath("//*[@id='tbl_sos_alert']/tbody/tr/td[2]")))
            {
                do
                {
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-success btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath("//*[contains(text(),'Close SOS')]"),true));

                ClickElement(By.XPath("//*[contains(text(),'Close SOS')]"));
            }
            return FindAlert();
        }
    }
}
