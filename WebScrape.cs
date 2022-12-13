using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Interact
{
    public class WebScrape
    {
        string url = "http://ddinter.scbdd.com/inter-checker/";

        public List<string> Interaction(string drugA, string drugB)
        {
            List<string> result = new List<string>();

            try
            {
                EdgeOptions options = new EdgeOptions();
                var service = EdgeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                service.HideCommandPromptWindow = true;
                service.SuppressInitialDiagnosticInformation = true;
                options.AddArgument("headless");
                options.AddArgument("--silent");
                options.AddArgument("log-level=3");

                var driver = new EdgeDriver(service, options);
                driver.Manage().Window.Minimize();
                driver.Navigate().GoToUrl(url);

                string searchBar_id = "awesome-input";
                string listItem_class = "collection-item";
                string resultInter_class = "result-interaction-item";
                string resultManagement_class = "result-management-item";

                foreach (char item in drugA)
                {
                    driver.FindElement(By.Id(searchBar_id)).SendKeys(Char.ToString(item));
                }
                driver.FindElement(By.Id(searchBar_id)).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.ClassName(listItem_class)).Click();

                foreach (char item in drugB)
                {
                    driver.FindElement(By.Id(searchBar_id)).SendKeys(Char.ToString(item));
                }
                driver.FindElement(By.Id(searchBar_id)).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.ClassName(listItem_class)).Click();

                Thread.Sleep(500);
                driver.FindElement(By.Id("get_interactions")).Click();

                Thread.Sleep(1000);
                string interaction = driver.FindElement(By.ClassName(resultInter_class)).Text;
                string management = driver.FindElement(By.ClassName(resultManagement_class)).Text;
                driver.Close();
                result.Add(interaction);
                result.Add(management);
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error occured, check your connection with internet!");
            }

            return result;
        }
    }
}
