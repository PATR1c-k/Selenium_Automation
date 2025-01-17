using MongoDB.Bson;
using MongoDB.Driver;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace NessusSeleniumTesting
{
    internal class UserDefineDefaultScanning
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--allow-insecure-localhost");
            chromeOptions.AddArgument("--disable-web-security");

            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void DefaultScan()
        {
            // Retrieve arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
            {
                Console.WriteLine("Error: No ID provided.");
                return;
            }

            string recordId = args[1];

            // Connect to MongoDB
            var client = new MongoClient("mongodb+srv://<MOngoURI>:<password>@cluster0.am5xmqs.mongodb.net");
            var database = client.GetDatabase("selenium_automation");
            var collection = database.GetCollection<BsonDocument>("formdatas");

            // Fetch the record from MongoDB
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(recordId));
            var record = collection.Find(filter).FirstOrDefault();
            if (record == null)
            {
                Console.WriteLine($"No record found with ID: {recordId}");
                return;
            }

            string name = record["name"].AsString;
            string description = record["description"].AsString;
            string target = record["target"].AsString;

            // Selenium actions
            driver.Url = "https://localhost:8834/#/";
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Login (static values for now)
            var usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Username']")));
            usernameField.SendKeys("Audix");

            var passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Password']")));
            passwordField.SendKeys("777");

            var signInButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-domselect='sign-in']")));
            signInButton.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("i.glyphicons.add")));

            // Fill form with dynamic data
            var nameField = driver.FindElement(By.CssSelector("input[data-input-id='name']"));
            nameField.Clear();
            nameField.SendKeys(name);

            var descriptionField = driver.FindElement(By.XPath("//textarea[@data-input-id='description']"));
            descriptionField.SendKeys(description);

            var targetField = driver.FindElement(By.CssSelector("textarea[data-input-id='text_targets']"));
            targetField.Clear();
            targetField.SendKeys(target);

            Thread.Sleep(2000);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
