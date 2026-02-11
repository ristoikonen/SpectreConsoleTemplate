using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using Microsoft.Maui.Graphics;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TSR.Worker.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SpectreConsoleTEMPL;

public static class Program
{
    static async Task Main(string[] args)
    {
        // ConfigurationManager configurationManager = new ConfigurationManager();
        //var configvalue1 = configurationManager.Sources; 

        string title = "title";

        SpectreConsoleOutput.DisplayTitleH3(title);

        // user choice scenarios
        var scenarios = SpectreConsoleOutput.SelectScenarios();
        var scenario = scenarios[0];

        // present
        switch (scenario)
        {

            case "Get Image as List<Color>":

                var imagename = @"StenoH.bmp"; //@"NYC_Skyline.bmp";
                var imagepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), imagename);

                if (File.Exists(imagepath))
                {
                    //TODO: get w, h to call SaveColorsAsBmp
                    using (Stream stream = File.OpenRead(imagepath))
                    {
                        var colors = await ImageColors.GetImageColors(stream);

                        if(colors is not null && colors.Count > 0)
                        {
                            Console.WriteLine($"Count: {colors.Count} \n First \t R: {colors[0].Red} \t B: {colors[0].Blue}\t R: {colors[0].Red}");
                            ImageColors.SaveImage(colors);
                        }
                    }
                }
                break;
            case "PDF AI Summariser":
                
                break;
            case "Get Response":
                
                string jsonContent = File.ReadAllText("postcodes.json");
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    AustralianPostcode[] postcodes = JsonSerializer.Deserialize<AustralianPostcode[]>(jsonContent) ?? Array.Empty<AustralianPostcode>();
                }
                
                //var _httpClient = new HttpClient();
                //HttpGenericClient<AustralianPostcode[]> client = new HttpGenericClient<AustralianPostcode[]>(_httpClient);

                //var coinarray = await client.GetAsync(@"www.matthewproctor.com", @"Content/postcodes/australian_postcodes.json");
                //if (coinarray is not null && coinarray is AustralianPostcode[] && coinarray.Length > 0)
                //{
                //    var coin = coinarray[0];
                //}

                break;
            case "Generate image":
                string postcode = "2914";
                //var imageUrl = @"https://maps.googleapis.com/maps/api/staticmap?sensor=false&size=600x600&maptype=map&visible=-35.171389,149.128889&zoom=15&key=AIzaSyBdIWfU6TpZTfvxllzk8sTMf_tlP_niLs0";
                var openImageUrl = @"https://www.openstreetmap.org/?edit_help=1#map=17/-35.193336/149.146029";
                var yandexImageUrl = @"https://static-maps.yandex.ru/1.x/?ll=149.146029,-35.193336&z=16&size=600,600&l=map&pt=149.146029,-35.193336,pm2rdm";

                // 1. Create a rectangle based on a center point and "buffer" (radius)
                // Roughly 1 degree of Lat/Lng is ~111km, so 0.01 is about 1.1km
                double centerLat = -35.193336;
                double centerLng = 149.146029;
                double radKM = 1;

                double latBuffer = radKM / 111.0;
                double lngBuffer = radKM / (111.0 * Math.Cos(centerLat * Math.PI / 180.0));


                RectLatLng exportArea = new RectLatLng(
                    centerLat + latBuffer, // Top (North)
                    centerLng - lngBuffer, // Left (West)
                    lngBuffer * 2,         // Width
                    latBuffer * 2          // Height
                );
                GPoint exportAreaTopLeft = GMapProviders.YandexMap.Projection.FromLatLngToPixel(exportArea.LocationTopLeft, 16);


                // 2. Fetch the image
                // Zoom 16 is good for street level; 13 is good for a whole city
                int zoomLevel = 16;

                // NOTE:
                // The original code attempted to call `GetStaticImage` on OpenStreetMap provider:
                //   GMapProviders.OpenStreetMap.GetStaticImage(exportArea, zoomLevel, 1)
                // but `OpenStreetMapProvider` does not expose `GetStaticImage`. Use `GetTileImage`
                // after converting lat/lng to pixel coordinates using the provider's projection.
                GPoint exportAreaTopLeftOSM = GMapProviders.OpenStreetMap.Projection.FromLatLngToPixel(exportArea.LocationTopLeft, zoomLevel);

                // NOTE: Doesn't work!
                // TODO: Find a way....
                var osmTile = GMapProviders.OpenStreetMap.GetTileImage(exportAreaTopLeftOSM, zoomLevel);
                //var osmTile = GMapProviders.YandexMap.GetTileImage(exportAreaTopLeft, zoomLevel);

                if (osmTile?.Data != null)
                {
                    using (var mapImage = osmTile.Data)
                    {
                        try
                        {
                            string fileName = $"map_{centerLat}_{centerLng}_osm.jpg";
                            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(mapImage))
                            {
                                image.Save(fileName);
                            }
                            Console.WriteLine($"Saved: {fileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to save OSM tile image: {ex.Message}");
                        }
                    }
                }

                using (var mapImage = GMapProviders.HereMap.GetTileImage(exportAreaTopLeft, zoomLevel).Data)
                {
                    if (mapImage != null)
                    {
                        string fileName = $"map_{centerLat}_{centerLng}.jpg";
                        //mapImage.Save(fileName);
                        
                        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(mapImage))
                        {
                            image.Save(fileName); // "map_via_imagesharp.jpg");
                        }
                        Console.WriteLine($"Saved: {fileName}");
                    }
                }

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(openImageUrl);
                    response.EnsureSuccessStatusCode();

                    var encoder = new JpegEncoder
                    {
                        Quality = 80
                    };

                    var barr = await response.Content.ReadAsByteArrayAsync();

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(barr))
                    {
                        // Create a MemoryStream to save the output JPG data to a new byte array
                        using (MemoryStream outputStream = new MemoryStream())
                        {
                            // Configure the JpegEncoder with the desired quality (0-100)


                            // Save the image to the output stream using the Jpeg format and encoder
                            image.Save(outputStream, encoder);

                            // Return the new JPG image from a byte array
                            //return outputStream.ToArray();
                            File.WriteAllBytes("output_image.jpg", outputStream.ToArray());
                        }
                    }


                    //var image6L = SixLabors.ImageSharp.Image.Load(barr, encoder);

                }


                using (HttpClient client = new HttpClient())
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(openImageUrl);
                    await File.WriteAllBytesAsync(postcode + ".png", imageBytes);

                    //using (Bitmap bitmap = new Bitmap(imageBytes))
                    //{
                    //    // Create a Graphics object from the bitmap
                    //    using (Graphics graphics = Graphics.FromImage(bitmap))
                    //    {
                    //        // Define the brush for text color
                    //        using (Brush textBrush = new SolidBrush(textColor))
                    //        {
                    //            // Draw the text onto the image
                    //            graphics.DrawString(text, font, textBrush, location);
                    //        }
                    //    }

                    //    // Save the modified image
                    //    bitmap.Save(outputPath, ImageFormat.Png); // Or ImageFormat.Jpeg, etc.
                    //}

                    byte[] imageBytes2 = File.ReadAllBytes(args[0]);
                    var imageBytesEnumerable = new List<IEnumerable<byte>> { imageBytes2 };

                    //client.DownloadFileAsync(new Uri(url), @"c:\temp\image35.png");
                }

                break;

            case "AddOllamaChatCompletion":

                // Simple Semantic Kernel-style registration and chat completion using Ollama
                var baseUrl = AnsiConsole.Ask<string>("Ollama base URL (press enter for http://localhost:11434):");
                if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "http://localhost:11434";

                var modelName = AnsiConsole.Ask<string>("Model name to use (eg. llama3.2):");
                if (string.IsNullOrWhiteSpace(modelName)) modelName = "llama3.2";

                var promptText = AnsiConsole.Ask<string>("Prompt to send to the model:");

                var kernel = new KernelShim();
                var ollamaClient = new OllamaClient(baseUrl);
                kernel.AddOllamaChatCompletion("ollama", ollamaClient);
                // register a simple plugin
                kernel.AddPlugin(new SimpleTimestampPlugin());

                try
                {
                    var completion = await kernel.GetChatCompletionAsync("ollama", modelName, promptText);
                    AnsiConsole.MarkupLine("[bold blue]Completion:[/]");
                    AnsiConsole.WriteLine(completion);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }

                break;

            case "IChatClient":

                // Run a simple Ollama-based chat loop
                await OllamaChat.RunAsync();

                break;
        }
    }
}
