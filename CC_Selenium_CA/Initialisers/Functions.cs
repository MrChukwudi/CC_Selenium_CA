
using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Selenium_CC_CA.Initialisers
{
    public class Base
    {
        public delegate IWebElement CustomDelegate();

        //This method verifies that an element is enabled and applies an action to it.
        public static bool FindElement(Action<IWebElement> action, CustomDelegate Element)
        {
            try
            {
                if (Element.Invoke().Enabled && Element.Invoke().Displayed)
                {
                    action?.Invoke(Element.Invoke());

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            Functions.Screenshot(Element.Target.ToString());
            return false;
        }

        //This method receives a Css Selector as a string and returns an element 
        public static CustomDelegate CssSelector(String CssSelector)
        {
            CustomDelegate NewDelegate = delegate ()
            { return Constants.driver.FindElement(By.CssSelector(CssSelector)); };

            return NewDelegate;
        }

        public static CustomDelegate XPath(String XPath)
        {
            CustomDelegate NewDelegate = delegate ()
            { return Constants.driver.FindElement(By.XPath(XPath)); };

            return NewDelegate;
        }

        public static CustomDelegate Id(String Id)
        {
            CustomDelegate NewDelegate = delegate ()
            { return Constants.driver.FindElement(By.Id(Id)); };

            return NewDelegate;
        }

        public static CustomDelegate ClassName(String ClassName)
        {
            CustomDelegate NewDelegate = delegate ()
            { return Constants.driver.FindElement(By.ClassName(ClassName)); };

            return NewDelegate;
        }

        public static Action<IWebElement> ElementAction()
        {
            return null;
        }

    }

    //This class is checking if a Column text of a table matches a string  
    public class Table : Base
    {
        // TODO wait for the records to be displayed.
        //Overrides the ElementAction method of the Base class
        public static Action<IWebElement> ElementAction(string Text)
        {
            Action<IWebElement> NewAction = (element) =>
            {
                if (!element.Text.Contains(Text))
                {
                    throw new Exception("Column text and the name of the item don't match");
                }
            };
            return NewAction;
        }

        public static bool CssSelector(String TD, String Text)
        {
            if (FindElement(ElementAction(Text), Base.CssSelector(TD)))
                return true;
            else
                return false;
        }
        public static bool XPath(String TD, String Text)
        {
            if (FindElement(ElementAction(Text), Base.XPath(TD)))
                return true;
            else
                return false;
        }
    }

    public class Functions
    {
        public static string email;
        public static string password;
        public static List<string> LogFile = new List<string>();
        public static IWebDriver driver;


        public static void CheckUrl(String url)
        {
            do
            {
                try
                {
                    if (Constants.driver.Url != url)
                    {
                        Constants.driver.Navigate().GoToUrl(url);
                        // if we get logged out half way through the testing, log back in.
                        if (url != Constants.BaseUrl + LogIn_Page.PagePath && driver.Url == Constants.BaseUrl + LogIn_Page.PagePath)
                        {
                            LogIn_Page.TryLogin(Constants.email, Constants.password);
                            // TO DO log this as it might explain previous test failure.
                            // wait a bit to give a chance to complete login before re-attempting.
                            ShortWait();
                        }
                    }
                    else
                    {
                        Constants.driver.Navigate().Refresh();
                    }
                }
                catch (Exception e)
                {
                    Screenshot(url);
                    Console.WriteLine(e.Message.ToString());
                }
            }
            while (driver.Url != url);
        }

        /// <summary>
        /// Secondary Check URL - Use for multi login test cases
        /// </summary>
        /// <param name="url"></param>
        public static void CPCheckUrl(String url)
        {
            int i = 0;
            do
            {
                try
                {
                    if (Constants.driver.Url != url)
                    {
                        Constants.driver.Navigate().GoToUrl(url);
                        ShortWait();
                    }

                }
                catch (Exception e)
                {
                    Screenshot(url);
                    System.Diagnostics.Debug.WriteLine(e.Message.ToString());
                }
                i++;
            }
            while (Constants.driver.Url != url && i <= Constants.Loop);
        }

        public static void Refresh()
        {
            Constants.driver.Navigate().Refresh();
        }

        /// <summary>
        /// Write text in the element.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static bool WriteInElement(By selector, string text)
        {
            try
            {
                IWebElement element = Constants.driver.FindElement(selector);
                if (element != null)
                {
                    element.Clear();
                    element.SendKeys(text);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Apply a filter to a table, then looks if the search field is found in the table.
        /// </summary>
        /// <param name="searchBoxSelector">By selector for search box to filter the table</param>
        /// <param name="searchString">string used to filter the table</param>
        /// <param name="tableCellSelector">By selector for table cell containing the search string after filtering applied.</param>
        /// <returns>true if search string is found in the designated table cell</returns>
        internal static bool TableSearch(By searchBoxSelector, string searchString, By tableCellSelector)
        {
            try
            {
                bool success = false;
                // find the searchbox
                IWebElement searchBox = driver.FindElement(searchBoxSelector);
                if (searchBox != null)
                {
                    // write search terms in the search box
                    try
                    {
                        searchBox.Clear();
                    }
                    catch (Exception e) { Console.WriteLine(e.Message); searchBox.Click(); searchBox.Clear(); } // to save from an error in a specific test case.
                    searchBox.SendKeys(searchString);
                    // set explicit wait.
                    WebDriverWait wait = new WebDriverWait(driver, Constants.WaitTime);
                    // keep looking for element until the end of the explicit wait or until found.
                    IWebElement foundElement = wait.Until<IWebElement>(condition =>
                    {
                        try
                        {
                            IWebElement element = driver.FindElement(tableCellSelector);
                            if (element != null)
                            {
                                if (element.Text.ToLower().Contains(searchString.ToLower()))
                                {
                                    return element;
                                }
                            }
                            return null;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    });

                    // if foundElement is not null, the the value was found in the table, so set success to true.
                    if (foundElement != null)
                    {
                        success = true;
                    }

                }
                else
                {
                    // if the search box was not found, log it.
                    Log.Entry(Log.Error, string.Format("{0} not found on the page. Search could not proceed. Search term: {1}", searchBoxSelector.ToString(), searchString), string.Empty);
                }

                return success;
            }
            catch (Exception ex) // this happens when the search times out.
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// returns whether an element is displayed.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        internal static bool IsElementDisplayed(By selector, bool isExpected)
        {
            try
            {
                IWebElement element = driver.FindElement(selector);
                if (element != null)
                {
                    return element.Displayed;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// wait for an element to show after a search (if implicit wait is set)
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IWebElement WaitAfterTableSearch(By selector)
        {
            try
            {
                // explicit wait
                WebDriverWait wait = new WebDriverWait(driver, Constants.WaitTime);
                // keep looking for element until the end of the explicit wait or until found.
                IWebElement element = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(selector);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                });
                // return element.
                return element;
            }
            catch (Exception)
            {
                return null;
            }
            //try
            //{
            //    element = driver.FindElement(selector);
            //}
            //catch (Exception)
            //{
            //    element = null;
            //}
        }

        /// <summary>
        /// Get the text from the element. If the element is not available, returns null.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static string GetText(By selector)
        {
            try
            {
                return Constants.driver.FindElement(selector).Text;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// finds if an element is enabled and displayed (as close to clickable as available)
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsElementClickable(By by)
        {
            try
            {
                IWebElement element = Constants.driver.FindElement(by);
                if (element.Displayed && element.Enabled)
                {
                    return true;
                }
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if an alert is present and logs it result using the test name.
        /// </summary>
        /// <param name="testName"></param>
        internal static void LogAlert(string testName)
        {
            string[] result = FindAlert();
            if (string.IsNullOrWhiteSpace(result[1]) || result[0].Equals(Constants.ErrorMsg[0], StringComparison.OrdinalIgnoreCase) ||
                result[0].Equals(Constants.NotFound[0], StringComparison.OrdinalIgnoreCase) || result[0].Equals(Constants.Alert[0], StringComparison.OrdinalIgnoreCase))
            {
                Log.Entry(Log.Fail, testName, result[1]);
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: " + result[1]);
            }
            else
            {
                Log.Entry(Log.Pass, testName, result[1]);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: " + result[1]);
            }
        }

        /// <summary>
        /// This method returns the text of a pop-up window (with class name 'toast-container' alert for confirmation)
        /// </summary>
        /// <returns></returns>
        public static string[] FindAlert()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Constants.driver, Constants.WaitTime);
                IWebElement myDynamicElement = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(By.Id("toast-container"));
                    }
                    catch
                    {
                        return null;
                    }
                });

                return new string[2]
                {
                    myDynamicElement.Text,
                    Constants.driver.FindElement(By.XPath("//div[contains(@class, 'toast-message')]")).Text
                };
                //myDynamicElement.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                Debug.WriteLine("Checking for Alert: " + ex.Message.ToString());
                return Constants.NotFound;
            }

        }

        /// <summary>
        /// If the notification is still using the Sweet Alert System
        /// Check if an alert is present and logs it result using the test name.
        /// </summary>
        /// <param name="testName"></param>
        internal static void LogAlert_SA(string testName)
        {
            string[] result = FindAlert_SA();
            if (string.IsNullOrWhiteSpace(result[1]) || result[0].Equals(Constants.ErrorMsg[0], StringComparison.OrdinalIgnoreCase) ||
                result[0].Equals(Constants.NotFound[0], StringComparison.OrdinalIgnoreCase) || result[0].Equals(Constants.Alert[0], StringComparison.OrdinalIgnoreCase))
            {
                Log.Entry(Log.Fail, testName, result[1]);
            }
            else
            {
                Log.Entry(Log.Pass, testName, result[1]);
            }
        }

        public static string[] FindAlert_SA()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Constants.driver, Constants.ShortWait);
                IWebElement myDynamicElement = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]/h2"));
                    }
                    catch
                    {
                        return null;
                    }
                });

                return new string[2]
                {
                    myDynamicElement.Text,
                    Constants.driver.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]/p")).Text
                };
                //myDynamicElement.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                Debug.WriteLine("Checking for Alert: " + ex.Message.ToString());
                return Constants.NotFound;
            }

        }




        /// <summary>
        /// If a window pops up (with class name 'sweet-alert showSweetAlert visible') this method makes it disappear
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ClosePopUp(string text)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, Constants.WaitTime);
                IWebElement myDynamicElement = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]//h2"));
                    }
                    catch
                    {
                        return null;
                    }
                });
                if (myDynamicElement.Text.Equals(text))
                {
                    myDynamicElement = driver.FindElement(By.XPath("//div[@class='sa-confirm-button-container']//button"));
                    myDynamicElement.Click();
                    return true;
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Closing popup: " + ex.Message.ToString());
            }

            return false;
        }

        //This method updates the log file
        public static void Feedback(DateTime StartingTime, string[] TestCase, String NameOfTheTestCase)
        {
            string errorBlock, normalBlock;
            SetupBlockOutlines(out errorBlock, out normalBlock);

            bool error = false;
            if (TestCase[0] == Constants.ErrorMsg[0] || TestCase[0] == Constants.NotFound[0] || TestCase[0] == Constants.Alert[0])
            {
                error = true;
            }

            if (error)
            {
                Constants.LogFile.Add("");
                Constants.LogFile.Add(errorBlock);
            }

            Constants.LogFile.Add("");
            Constants.LogFile.Add(NameOfTheTestCase);
            Constants.LogFile.Add(normalBlock);
            Constants.LogFile.Add("Started At " + StartingTime.ToString("H-mm-ss"));
            Constants.LogFile.Add("");

            foreach (var item in TestCase)
            {
                Constants.LogFile.Add("Result = " + item);
            }

            Constants.LogFile.Add("");
            Constants.LogFile.Add("Finished At " + DateTime.Now.ToString("H-mm-ss"));
            Constants.LogFile.Add("Duration : " + (DateTime.Now - StartingTime).ToString());
            Constants.LogFile.Add(normalBlock);

            if (error)
            {
                Constants.LogFile.Add("");
                Constants.LogFile.Add(errorBlock);
            }

            using (TextWriter tw = new StreamWriter(Program.LogFile_Path))
            {
                foreach (var String in LogFile)
                {
                    tw.WriteLine(String);
                }
                tw.Close();
            }

        }

        /// <summary>
        /// setup the block outlines for errors and successes!
        /// </summary>
        /// <param name="errorBlock"></param>
        /// <param name="normalBlock"></param>
        private static void SetupBlockOutlines(out string errorBlock, out string normalBlock)
        {
            errorBlock = string.Empty;
            normalBlock = string.Empty;
            for (int i = 0; i < 92; i++)
            {
                errorBlock += "*";
                normalBlock += "-";
            }
        }


        /// <summary>
        /// Write test outcome in the log file.
        /// </summary>
        /// <param name="startingTime"></param>
        /// <param name="testCase"></param>
        /// <param name="nameOfTheTestCase"></param>
        public static void Feedback(DateTime startingTime, List<LogItem> testCase, String nameOfTheTestCase)
        {
            string errorBlock, normalBlock;
            SetupBlockOutlines(out errorBlock, out normalBlock);

            bool error = false;
            foreach (LogItem item in testCase)
            {
                if (item.GetStatus() == Log.Fail)
                {
                    error = true;
                    break;
                }
            }

            if (error)
            {
                Constants.LogFile.Add("");
                Constants.LogFile.Add(errorBlock);
            }

            Constants.LogFile.Add("");
            Constants.LogFile.Add(nameOfTheTestCase);
            Constants.LogFile.Add(normalBlock);
            Constants.LogFile.Add("Started At " + startingTime.ToString("H-mm-ss"));
            Constants.LogFile.Add("");

            foreach (var item in testCase)
            {
                Constants.LogFile.Add(item.Print());
            }

            Constants.LogFile.Add("");
            Constants.LogFile.Add("Finished At " + DateTime.Now.ToString("H-mm-ss"));
            Constants.LogFile.Add("Duration : " + (DateTime.Now - startingTime).ToString());
            Constants.LogFile.Add(normalBlock);

            if (error)
            {
                Constants.LogFile.Add("");
                Constants.LogFile.Add(errorBlock);
            }

            using (TextWriter tw = new StreamWriter(Program.LogFile_Path))
            {
                foreach (var String in LogFile)
                {
                    tw.WriteLine(String);
                }
                tw.Close();
            }

        }

        //This method takes a Screenshot and saves it in the New_Directory_Path
        public static void Screenshot(string name)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                name = name.Replace(c.ToString(), "");
            }

            // TODO check why the null error.
            try
            {
                Screenshot ss = ((ITakesScreenshot)Constants.driver).GetScreenshot();
                ss.SaveAsFile(Program.New_Directory_Path + name + ".png", ScreenshotImageFormat.Png);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Click on an element until elementToCheckAfterClick is present or not present anymore. This will be tried multiple times maximum is Constant.Loop.
        /// </summary>
        /// <param name="elementToCheckAfterClick"></param>
        /// <param name="isElementToAppear"></param>
        /// <param name="elementToClick"></param>
        /// <returns></returns>
        public static bool MultipleTryClick(By elementToCheckAfterClick, bool isElementToAppear, By elementToClick)
        {
            int x = Constants.Loop;

            do
            {
                ClickElement(elementToClick);
                x--;
            } while ((isElementToAppear ^ WaitForElement(elementToCheckAfterClick, false)) && x >= 0);

            if (x >= 0)
                return true;
            else
                return false;
        }

        //This method prevents characters appearing on the screen
        public static void GetConsoleSecurePassword()
        {
            password = "";
            List<string> list = new List<string>();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (list.Count != 0)
                    {
                        list.RemoveAt(list.Count - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    list.Add(i.KeyChar.ToString());
                    Console.Write("*");
                }
            }
            foreach (var item in list)
            {
                password += item;
            }
        }


        /// <summary>
        /// Wait until an element is found, displayed and enabled. Wait time is Constant.LongWait.
        /// </summary>
        /// <param name="selector"></param>
        public static bool WaitForElement(By selector, bool isExpected)
        {
            return ClickOrWaitForElement(selector, false, isExpected);
        }

        /// <summary>
        /// Wait for element ot be available and click if required.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="click"></param>
        /// <returns></returns>
        private static bool ClickOrWaitForElement(By selector, bool click, bool isExpected)
        {
            bool returnValue = false;
            var wait = new WebDriverWait(Constants.driver, Constants.LongWait);
            try
            {
                returnValue = wait.Until(Condition =>
                {
                    try
                    {
                        IWebElement element = Constants.driver.FindElement(selector);
                        if (element != null && element.Displayed && element.Enabled)
                        {
                            if (click)
                            {
                                element.Click();
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return false;
                    }

                });
                return returnValue;
            }
            catch (WebDriverTimeoutException)
            {
                if (click)
                {
                    Console.WriteLine("Element {0} could not be clicked after {1}.", selector.ToString(), Constants.LongWait.ToString());
                }
                else
                {
                    Console.WriteLine("Element {0} could not be found or did not get displayed and enabled before {1} time out.", selector.ToString(), Constants.LongWait.ToString());
                }

                return false;
            }
        }

        /// <summary>
        /// Scroll to the required element.
        /// </summary>
        /// <param name="selector"></param>
        public static bool MoveToElement(By selector)
        {
            try
            {
                MoveToElement(driver.FindElement(selector));
                return true;
            }
            catch (Exception) { return false; } // prevent crash if element does not exist. // TODO collect missing element names here.
        }

        /// <summary>
        /// scroll to the required element
        /// </summary>
        /// <param name="elementToShow"></param>
        public static void MoveToElement(IWebElement elementToShow)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView();", elementToShow);
        }

        /// <summary>
        /// scroll to the top of the page
        /// </summary>
        internal static void ScrollToTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, 0);");
        }

        /// <summary>
        /// scroll to the bottom of the page
        /// </summary>
        internal static void ScrollToBottom()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        /// <summary>
        /// Wait for the button to be clickable, then click.
        /// </summary>
        /// <param name="selector"></param>
        public static bool ClickElement(By selector)
        {
            return ClickOrWaitForElement(selector, true, true);
        }

        /// <summary>
        /// check if element is present
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsElementPresent(By by, bool isExpected)
        {
            try
            {
                Constants.driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// look for a non existing element to wait for the implicit wait time.
        /// </summary>
        public static void ShortWait()
        {
            try
            {
                driver.FindElement(By.Id("waiting_for_javascripts_to_load"));
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        /// <summary>
        /// set IsDebug to true when running in debug mode.
        /// </summary>
        [Conditional("DEBUG")]
        public static void SetDebug()
        {
            Constants.IsDebug = true;
        }
        [Conditional("DEBUG")]
        public static void UseSso()
        {
            Constants.UseSso = false;
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Get the parent of the current element.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IWebElement GetParent(this IWebElement node)
        {
            return node.FindElement(By.XPath(".."));
        }

        /// <summary>
        /// An expectation for checking whether an element is visible.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located, visible and clickable.</returns>
        public static Func<IWebDriver, IWebElement> ElementIsClickable(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }

        /// <summary>
        /// If conditionEnabled is true, tries to click until condition is found or timeout.
        /// If conditionEnabled is false, tries to click until condition is not found anymore or timeout.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="condition"></param>
        /// <param name="conditionEnabled"></param>
        /// <returns></returns>
        public static bool ClickUntilCondition(this IWebElement element, By condition, bool conditionEnabled)
        {
            var wait = new WebDriverWait(Constants.driver, Constants.LongWait);
            int x = Constants.Loop;


            //if (conditionEnabled)
            //{
            do
            {
                ClickOnElement(element, wait);
                x--;
            } while (x >= 0 && (conditionEnabled ^ Functions.WaitForElement(condition, true)));
            //}
            //else
            //{
            //    do
            //    {
            //        ClickOnElement(element, wait);
            //        x--;
            //    } while (x >= 0 && Functions.WaitForElement(condition));
            //}

            if (x >= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// this is to simplify clickUntilCondition method and avoid duplicating code.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="wait"></param>
        private static void ClickOnElement(IWebElement element, WebDriverWait wait)
        {
            try
            {
                wait.Until(Condition =>
                {
                    if (element.Displayed && element.Enabled)
                    {
                        try
                        {
                            element.Click();
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// If the notification is still using the Sweet Alert System
        /// Check if an alert is present and logs it result using the test name.
        /// </summary>
        /// <param name="testName"></param>
        internal static void LogAlert_SA(string testName)
        {
            string[] result = FindAlert_SA();
            if (string.IsNullOrWhiteSpace(result[1]) || result[0].Equals(Constants.ErrorMsg[0], StringComparison.OrdinalIgnoreCase) ||
                result[0].Equals(Constants.NotFound[0], StringComparison.OrdinalIgnoreCase) || result[0].Equals(Constants.Alert[0], StringComparison.OrdinalIgnoreCase))
            {
                //Log.AddFail(testName, result[1]);
                Log.Entry(Log.Fail, testName, result[1]);
            }
            else
            {
                Log.Entry(Log.Pass, testName, result[1]);
            }
        }


        public static string[] FindAlert_SA()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Constants.driver, Constants.WaitTime);
                IWebElement myDynamicElement = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]/h2"));
                    }
                    catch
                    {
                        return null;
                    }
                });

                return new string[2]
                {
                    myDynamicElement.Text,
                    Constants.driver.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]/p")).Text
                };
                //myDynamicElement.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                Debug.WriteLine("Checking for Alert: " + ex.Message.ToString());
                return Constants.NotFound;
            }

        }

        private static void CheckForLicenseWarning()
        {
            // check for confirmation message or error message.
            while (FindAlert_SA_Quick()[0] == "License Warning")
            {
                //ShortWait();
                ClosePopUp("License Warning");
                //ShortWait();
            }
        }

        public static bool ClosePopUp(string text)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Constants.driver, Constants.WaitTime);
                IWebElement myDynamicElement = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]//h2"));
                    }
                    catch
                    {
                        return null;
                    }
                });
                if (myDynamicElement.Text.Equals(text))
                {
                    myDynamicElement = Constants.driver.FindElement(By.XPath("//div[@class='sa-confirm-button-container']//button"));
                    myDynamicElement.Click();
                    return true;
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                System.Diagnostics.Debug.WriteLine("Closing popup: " + ex.Message.ToString());
            }

            return false;
        }

        public static string[] FindAlert_SA_Quick()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Constants.driver, Constants.ShortWait);
                IWebElement myDynamicElement = wait.Until<IWebElement>(d =>
                {
                    try
                    {
                        return d.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]/h2"));
                    }
                    catch
                    {
                        return null;
                    }
                });

                return new string[2]
                {
                    myDynamicElement.Text,
                    Constants.driver.FindElement(By.XPath("//div[contains(@class, 'sweet-alert')]/p")).Text
                };
                //myDynamicElement.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                Debug.WriteLine("Checking for Alert: " + ex.Message.ToString());
                return Constants.NotFound;
            }

        }



    }
}