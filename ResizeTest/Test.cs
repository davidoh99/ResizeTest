using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;

namespace SnipSketchTest
{
    public class Test
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string PaintId = @"C:\Windows\System32\mspaint.exe";

        private static WindowsDriver<WindowsElement> session;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AppiumOptions ao = new();
            ao.AddAdditionalCapability("app", PaintId);
            ao.AddAdditionalCapability("deviceName", "WindowsPC");
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), ao);
        }

        [Test]
        public void Resize()
        {
            var canvas = session.FindElementByClassName("MSPaintView");

            try
            {
                session.FindElementByName("Maximise");
            }
            catch (WebDriverException)
            {
                session.FindElementByName("Restore").Click();
            }

            List<ActionSequence> actionSequencesList = new();
            OpenQA.Selenium.Appium.Interactions.PointerInputDevice pen = new(PointerKind.Pen);
            ActionSequence mouseSequence = new(pen, 0);
            mouseSequence.AddAction(pen.CreatePointerDown(PointerButton.PenContact));
            mouseSequence.AddAction(pen.CreatePointerMove(CoordinateOrigin.Pointer, 100, 100, TimeSpan.FromSeconds(1)));
            mouseSequence.AddAction(pen.CreatePointerMove(CoordinateOrigin.Pointer, -100, -100, TimeSpan.FromSeconds(1)));
            mouseSequence.AddAction(pen.CreatePointerUp(PointerButton.PenContact));
            actionSequencesList.Add(mouseSequence);
            session.PerformActions(actionSequencesList);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            session.Close();
        }
    }
}