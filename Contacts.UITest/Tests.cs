using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Contacts.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }
        [Test]
        public void SuperPrueba()
        {
            app.EnterText(x => x.Marked("entUserName"), "Victor Roque");
            app.EnterText(x => x.Marked("email"), "vhcobden@gmail.com");
            app.EnterText(x => x.Marked("entPassword"), "password");
            app.Tap(x => x.Marked("btnLogin"));
            app.Tap(x => x.Marked("btnLogin"));
            app.WaitForNoElement(x => x.Marked("indIsBusy"));
            app.WaitForElement(x => x.Marked("Perfil"));

            app.EnterText(x => x.Marked("entBirthday"), "01/07/1993");
            app.EnterText(x => x.Marked("entPhoneNumber"), "934545454");

            app.DismissKeyboard();
            app.Tap(x => x.Marked("btnSaveProfile"));

            app.WaitForNoElement(x => x.Marked("indIsBusy"));
            app.WaitForElement(x => x.Marked("lblPhoneNumber"));


            app.TouchAndHold(x => x.Text("Vlady"));
            app.Tap(x => x.Text("Eliminar"));
            app.TouchAndHold(x => x.Text("Goku"));
            app.Tap(x => x.Text("Eliminar"));

            app.WaitForNoElement(x => x.Marked("lblPhoneNumber"));

            app.Tap(x => x.Button());
            app.EnterText(x => x.Marked("entContactName"), "Satan");
            app.EnterText(x => x.Marked("entPhoneNumber"), "934545454");
            app.Tap(x => x.Marked("btnAddContact"));
            app.Tap(x => x.Text("Aceptar"));

            app.WaitForElement(x => x.Marked("lblPhoneNumber"));

            var phoneElement = app.Query(x => x.Marked("lblPhoneNumber"));
            Assert.IsTrue(phoneElement.Any());
        }
    }
}

