using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace SpectreConsoleTEMPL;

class UserSettings
{
    [Name("User ID")]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }

    [Ignore]
    public string? InternalSecret { get; set; } // This won't be exported

}

public static class UseCSV
{
    public static void TestWrite()
    { 
        var csvUtility = new CSVUtility<UserSettings>();
        var userList = new List<UserSettings>
        {
            new UserSettings { Id = 1, LastName = "Smith",
                Email = "asm@gmc.com" }
            };
        csvUtility.Write(userList, @"c:\tmp\output.csv");
    }
}

internal class CSVUtility<T>
{

    public void Write(List<T> Lista, string fileName)
    { 
        // 2. Write to CSV
        using (var writer = new StreamWriter(fileName))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        { 
            csv.WriteRecords(Lista);
        }

        Console.WriteLine("CSV created successfully!");
    }
}
