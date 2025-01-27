﻿using OpenQA.Selenium.Chrome;
using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Globalization;
using System.Threading;



namespace TFR_cons
{
	public class Program // Main class
	{
		public static OpenQA.Selenium.Chrome.ChromeDriver ChromeDriver = new ChromeDriver();
		//public static OpenQA.Selenium.Chrome.ChromeDriver ChromeDriver = new ChromeDriver(@"D:\Программинг\с\new_shit\TFR\TFR_cons\bin\Debug");
		public static IJavaScriptExecutor js = (IJavaScriptExecutor)ChromeDriver; // Make JS instance for JS execution
		public static Thread MessageTrackingThread; 

		static void Main(string[] args) // Main method
		{

			Console.WriteLine("Current culture: " + CultureInfo.CurrentCulture.Name);

			Properties.Settings.Default.dbCreated = true;
			TFR_cons.Properties.Settings.Default.dbCreated = true;

			//var options = new ChromeOptions(); // Create Chrom browser instance and call it as a driver
			//options.AddArgument("--disable-gpu");
			//ChromeDriver = new ChromeDriver(options);

			try { ChromeDriver.Navigate().GoToUrl("https://profit.ly/profiding"); } // Go to URL file:///D:/1/profitly.html https://profit.ly/profiding
			catch { Console.WriteLine("Chrome driver: can't go to: file:///D:/1/profitly.html"); } // https://profit.ly/profiding

			Console.WriteLine("Version: 01-01/11/2018");
			Console.WriteLine("Please set the action: ");
			Console.WriteLine("bought. Injects bought message ");
			Console.WriteLine("sold. Injects bought message ");
			Console.WriteLine("else. Injects bought message ");
			Console.WriteLine("brand new db. Creates new DB from scratch");
			Console.WriteLine("start. Starts the program");
			Console.WriteLine("");


			while (true)
			{
				string s = Console.ReadLine();

				if (s == "bought")
				{
					InjectTestMessage.InjectMsg("bought");
				}

				else if (s == "sold")
				{
					InjectTestMessage.InjectMsg("sold");
				}

				else if (s == "else")
				{
					InjectTestMessage.InjectMsg("else");
				}

				else if (s == "new db")
				{
					DataBase.DropTable();
					DataBase.DBStructCreate();
					DataBase.DBInsertStartingBalance(0);
				}

				else if (s == "brand new db")
				{
					DataBase.DBCreate();
				}

				else if (s == "start") // Message tracking with manual login or offline page opening. Desired page must be saved and offline url specefied
				{
					//GetAndTrackMMessages.startTracking = true;
					MessageTrackingThread = new Thread(new ThreadStart(GetAndTrackMMessages.MessageSearch)); // A thread for message tracking. Message tracking exist in a parralell thread
					MessageTrackingThread.IsBackground = true;
					MessageTrackingThread.Name = "MessageTrackingThread";
					MessageTrackingThread.Start();
				}

				else if (s == "login")
				{

					try { ChromeDriver.FindElementByXPath("//*[@id=\"confirm-modal\"]/div[3]/span/a").Click(); } // Click on OK disclamer button
					catch { Console.WriteLine("Can't click on desclamer button. There is no pop-up window or page changed."); }

					List<string> jsString = new List<string>(); // Collection for JS commands
					//jsString.Add("document.getElementsByName('g-recaptcha-response')[0].style.display='block';"); // // Display text area for pasting resolved captcha. Make textarea where captcha hash generated by google must be posted visible. Captcha will be solved and posted to this textarea
					jsString.Add("document.getElementsByName('j_username')[0].value='hunterhpm@gmail.com';"); // Type login 
					jsString.Add("document.getElementsByName('j_password')[0].value='Hpm.41771';"); // Type password
					//jsString.Add("document.getElementsByName('g-recaptcha-response')[0].value='" + Anticaptcha_example.Program.ExampleNoCaptchaProxyless() + "';");
					//jsString.Add("document.getElementsByName('Submit')[0].click();"); // Submit button click 

					// JS execution
					foreach (string z in jsString)
					{
						try { Console.WriteLine("Executing JS command: " + z); js.ExecuteScript(z); }
						catch { Console.WriteLine("Getting element error: " + z + ". This command did not work. No such element or page changed. Error."); }
					}

				}

			}




		} //

	}
}
