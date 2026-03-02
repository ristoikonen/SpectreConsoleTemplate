using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

using System;
using System.IO;
using System.Text.Json;
//using ManagedCommon;
//using Microsoft.CommandPalette.Extensions.Toolkit;

namespace SpectreConsoleTEMPL.Helpers;

// namespace Microsoft.CmdPal.Ext.WindowsTerminal.Helpers;
public class AppSettings
{
    public string? SettingName1 { get; set; }
    public int SettingName2 { get; set; }
}

public sealed class AppSettingsManager
{
    private const string FileName = "appsettings.json";

    private static string SettingsPath()
    {
        //Microsoft.CommandPalette.Extensions.Toolkit.Utilities.GetAppDataPath("Microsoft.CmdPal");

        //var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, FileName);
    }

    private readonly string _filePath;

    public AppSettings Current { get; private set; } = new();

    public AppSettingsManager()
    {
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Directory.CreateDirectory(directory);
        _filePath = directory; // SettingsPath();
        Load();
    }

    public void Load()
    {
        //try
        //{
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                //var directory = Environment.GetFolderPath(_filePath); // Environment.SpecialFolder.ApplicationData);
                //Directory.CreateDirectory(directory);
                var loaded = JsonSerializer.Deserialize<AppSettings>(json);//, AppSettingsJsonContext.Default.AppSettings!);
                if (loaded is not null)
                {
                    Current = loaded;
                }
            }
        //}
        //catch (Exception ex)
        //{
            //ExtensionHost.LogMessage(new LogMessage { Message = ex.ToString() });
            //Logger.LogError("Failed to load app settings", ex);
        //}
    }

    public void Save()//string SettingName1)
    {
        //try
        //{
            var settings = new AppSettings
            {
                SettingName1 = Current.SettingName1,
                SettingName2 = Current.SettingName2,
                // ... other properties
            }; 
            var settings_directory = DirectoryManager.GetSettingsDirectory();
            string settings_filepath = Path.Combine(settings_directory, "settings.json");
            //var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //Directory.CreateDirectory(directory);
            var json = JsonSerializer.Serialize<AppSettings>(settings);
            File.WriteAllText(settings_filepath, json);
        //}
        //catch (Exception ex)
        //{
            //ExtensionHost.LogMessage(new LogMessage { Message = ex.ToString() });
            //Logger.LogError("Failed to save app settings", ex);
        //}
    }
}
