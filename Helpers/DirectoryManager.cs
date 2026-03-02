using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SpectreConsoleTEMPL.Helpers
{
    public static class DirectoryManager
    {

        public static string GetAppDataDirectory()
        {
            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //entryAssembly != null
            //    ? Path.GetDirectoryName(entryAssembly.Location)
            //    : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //var directory = fullPath ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var settingsDirectory = Path.Combine(directory, "SpectreConsoleTEMPL");
            Directory.CreateDirectory(settingsDirectory);
            return settingsDirectory;
        }


        public static string GetSettingsDirectory()
        {   
            var directory = GetAppDataDirectory();
            var settingsDirectory = Path.Combine(directory, "Settings");
            Directory.CreateDirectory(settingsDirectory);
            return settingsDirectory;
        }       

        //var json = File.ReadAllText(_filePath);
        //var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    }
}
