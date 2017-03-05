﻿using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Contacts.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .InstalledApp("com.hjr.contactsapp")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .InstalledApp("com.hjr.contactsapp")
                .StartApp();
        }
    }
}

