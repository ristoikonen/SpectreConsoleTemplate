//using Microsoft.Maui.Graphics;
//using Microsoft.Maui.Graphics.Platform;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MauiColor = Microsoft.Maui.Graphics.Color;

namespace SpectreConsoleTEMPL;

public static class ImageColors
{

    public static void SaveColorsAsBmp(List<MauiColor> colors, int width, int height, string filePath)
    {
        // 1. Create a new image using ImageSharp
        using (Image<SixLabors.ImageSharp.PixelFormats.Rgba32> image = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(width, height))
        {
            for (int i = 0; i < colors.Count && i < (width * height); i++)
            {
                int x = i % width;
                int y = i / width;

                // 2. Convert MAUI Color to Rgba32
                var c = colors[i];
                image[x, y] = new SixLabors.ImageSharp.PixelFormats.Rgba32(c.Red, c.Green, c.Blue, c.Alpha);
            }

            // 3. Save as BMP
            image.SaveAsBmp(filePath);
        }
    }


    //TODO: async?
    public static void SaveImage(List<MauiColor> colors)
    {
        byte[] pixelData = new byte[colors.Count * 4];
        var imagename = @"NYC_SkylineNEWW.bmp";
        var imagepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), imagename);

        for (int i = 0; i < colors.Count; i++)
        {
            pixelData[i * 4] = (byte)(colors[i].Red * 255);
            pixelData[i * 4 + 1] = (byte)(colors[i].Green * 255);
            pixelData[i * 4 + 2] = (byte)(colors[i].Blue * 255);
            pixelData[i * 4 + 3] = (byte)(colors[i].Alpha * 255);
        }

        File.WriteAllBytes(imagepath, pixelData);

    }

    public async static Task<List<MauiColor>> GetImageColors(Stream imageStream)
    {
        var colors = new List<MauiColor>();
        
        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> siximage 
            = await SixLabors.ImageSharp.Image.LoadAsync<SixLabors.ImageSharp.PixelFormats.Rgba32>(imageStream);

        List<SixLabors.ImageSharp.Color> mcolors = new List<SixLabors.ImageSharp.Color>(siximage.Width * siximage.Height);

        siximage.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                // Get a read-only span of the pixel row
                ReadOnlySpan<SixLabors.ImageSharp.PixelFormats.Rgba32> pixelRow = accessor.GetRowSpan(y);

                // 3. Iterate through the pixels in the row and add to the list
                foreach (ref readonly SixLabors.ImageSharp.PixelFormats.Rgba32 pixel in pixelRow)
                {
                    // Implicitly convert the Rgba32 pixel to a more generic SixLabors.ImageSharp.Color struct
                    mcolors.Add(pixel);

                    MauiColor mauiColor = new MauiColor(pixel.R, pixel.G, pixel.B, pixel.A);
                    colors.Add(mauiColor);

                }
            }
        });


        /*

        // 1. Load the image using the platform-specific implementation
        IImage image = PlatformImage.FromStream(imageStream);

        if (image != null)
        {
            // 2. To get pixel data, we can save it to a stream as a specific format
            using (var ms = new MemoryStream())
            {
                // Note: Saving as PNG/JPEG doesn't give raw pixels easily.
                // For raw access, most developers use platform-specific APIs (SkiaSharp or native).
                // However, a common trick is to draw the IImage to a small buffer.

                // To simplify, let's assume you want a sample of colors:
                int width = (int)image.Width;
                int height = (int)image.Height;

                // For true pixel-by-pixel access in MAUI Graphics, 
                // it is often easier to use SkiaSharp or native Bitmap objects.
                var bytes = image.AsBytes();
                await image.AsBytesAsync().ContinueWith(t =>
                {
                    var pixelBytes = t.Result;
                    
                    //var cols = t.Result.AsEnumerable<Microsoft.Maui.Graphics.Color>();
                    // Process pixelBytes to extract colors
                });
                await image.AsBytesAsync().ContinueWith(t =>
                {
                    var pixelBytes = t.Result;
                    for (int i = 0; i < pixelBytes.Length; i += 4)
                    {
                        if (pixelBytes.Length > (i + 4))
                        {
                            byte r = pixelBytes[i];
                            byte g = pixelBytes[i + 1];
                            byte b = pixelBytes[i + 2];
                            byte a = pixelBytes[i + 3];
                            Color c = new Color(r, g, b, a);
                            string hx  = c.ToHex();
                            colors.Add(c);
                            //colors.Add(Color.FromRgba(r, g, b, a);
                        }
                        else 
                        { 
                            Console.WriteLine($"Warning: pixelBytes length {pixelBytes.Length} is not a multiple of 4. Last index: {i}");
                        }

                        // 		i	3742708	int

                    }

                    // Process pixelBytes to extract colors
                });
                //var uniqueColors = new List<Color>();
 

                // 3. Loop through bytes (4 bytes per pixel: R, G, B, A)
                
                //for (int i = 0; i < bytes.Length; i += 4)
                //{
                //    byte r = bytes[i];
                //    byte g = bytes[i + 1];
                //    byte b = bytes[i + 2];
                //    byte a = bytes[i + 3];

                //    colors.Add(Color.FromRgba(r, g, b, a));
                //}
                
            }
        }
        */


        return colors;
    }


}