using System;
using System.Configuration;using System.IO;

namespace Main.Shared
{
    public static class AppConfiguration
    {
        private static string SavedServicesFileName_Default => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "services.xml");

        public static string SavedServicesFileName
        {
            get
            {
                return ReadSetting("SavedServicesFileName", SavedServicesFileName_Default);
            }
            set { AddUpdateAppSettings("SavedServicesFileName", value); }
        }

        public static string Style_Acccent
        {
            get { return ReadSetting("Style_Accent"); }
            set { AddUpdateAppSettings("Style_Accent", value); }
        }

        public static string Style_AppTheme
        {
            get { return ReadSetting("Style_AppTheme"); }
            set { AddUpdateAppSettings("Style_AppTheme", value); }
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                System.Windows.MessageBox.Show("Error writing app settings");
            }
        }

        private static string ReadSetting(string key, string defaultValue = "")
        {
            string returnString = defaultValue;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings[key] == null)
                {
                    AddUpdateAppSettings(key, defaultValue);
                    return defaultValue;
                }
                else
                {
                    returnString = appSettings[key];
                }
            }
            catch (ConfigurationErrorsException)
            {
                System.Windows.MessageBox.Show("Error reading app settings");
            }

            return returnString;
        }
    }
}