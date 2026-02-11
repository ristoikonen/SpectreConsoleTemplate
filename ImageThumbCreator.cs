using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;
using GMap.NET;
using GMap.NET.MapProviders;

namespace SpectreConsoleTEMPL;

public class ExifVector
{
    public ExifVector() { }
    public ExifVector(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile exifProfile)
    {
        this.ExifProfile = exifProfile;
    }

    public SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile? ExifProfile { get; set; }
}

public static class ImageInfoReader
{

    public static ExifVector? GetExifVector(string fileNamepath)
    {
        if (File.Exists(fileNamepath))
        {
            using var image = Image.Load(fileNamepath);
            var exif_profile = image.Metadata.ExifProfile;
            if (exif_profile is not null)
            {
                // ExifVector? exif_vector
                return new ExifVector(exif_profile);
            }
        }
        return null;
    }

    public static SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile? GetExifProfile(string fileNamepath)
    {
        if (File.Exists(fileNamepath))
        {
            using var image = Image.Load(fileNamepath);
            return image.Metadata.ExifProfile;
        }
        return null;
    }
}

public record ImageFormat(string Name, string Extension, string MimeType, byte[] HeaderBytes)
{
    // 2. Define static instances to act like enum members
    public static readonly ImageFormat Png = new(
        "Portable Network Graphics", ".png", "image/png", new byte[] { 0x89, 0x50, 0x4E, 0x47 });

    public static readonly ImageFormat Jpeg = new(
        "Joint Photographic Experts Group", ".jpg", "image/jpeg", new byte[] { 0xFF, 0xD8 });

    public static readonly ImageFormat Bmp = new(
        "Bitmap", ".bmp", "image/bmp", new byte[] { 0x42, 0x4D });

    // 3. Optional: Add a method to list all "enum" values
    public static IEnumerable<ImageFormat> All => new[] { Png, Jpeg, Bmp };

    // 4. Optional: Add logic directly inside the 'enum'
    public ImageFormat? ValidateHeader(byte[] fileHeader) =>
        //HeaderBytes is not null && HeaderBytes.Length > 1 &&
        All.FirstOrDefault(f => f.HeaderBytes.SequenceEqual(HeaderBytes));
    //fileHeader.Take(HeaderBytes.Length).SequenceEqual(HeaderBytes);
}


public static class ImageThumbCreator
{
    public static Image? Create(string fileNamepath, int maxWidth, int maxHeight)
    {
        if (string.IsNullOrWhiteSpace(fileNamepath))
            return null;

        if (!File.Exists(fileNamepath))
            return null;

        // Load image with ImageSharp
        using var image = Image.Load(fileNamepath);

        // Define maximum thumbnail dimension (preserves aspect ratio)
        //const int maxDimension = 200;

        var resizeOptions = new ResizeOptions
        {
            //used to be .Max
            Mode = ResizeMode.Crop,
            Size = new SixLabors.ImageSharp.Size(maxWidth, maxHeight)
        };

        image.Mutate(x => x.Resize(resizeOptions));

        var barr = File.ReadAllBytes(fileNamepath).Take(2).ToArray();

        var imgf = new ImageFormat("", "", "", barr).ValidateHeader(File.ReadAllBytes(fileNamepath).Take(2).ToArray());

        // Save to memory stream as JPEG and create System.Drawing.Bitmap
        using var ms = new MemoryStream();
        //image.SaveAsPngAsync(ms);
        //image.SaveAsBmp(ms, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder {  SupportTransparency = true });
        image.SaveAsJpeg(ms, new JpegEncoder { Quality = 90 });
        ms.Position = 0;

        //HACK: Return ImageSharp image directly to avoid System.Drawing dependency
        //using var tempBmp = new Bitmap(ms);
        // Clone to decouple bitmap from the underlying stream which will be disposed
        //var result = new Bitmap(tempBmp);

        return image;
    }

    //public static SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile? GetExifProfile(string fileNamepath)
    //{
    //    if (File.Exists(fileNamepath))
    //    {
    //        using var image = Image.Load(fileNamepath);
    //        return image.Metadata.ExifProfile;
    //    }
    //    return null;
    //}
}



//if (exif_profile != null)
//{
//    // Access a specific tag directly
//    //var software = exif_profile.Values(ExifTag.Software);
//    //Console.WriteLine($"Software: {software?.Value}");
//    foreach (IExifValue value in exif_profile.Values)
//    {
//        var tag = value.Tag;
//        if (tag != null)
//        { 
//            tag.
//        }
//        // Tag describes what it is (e.g., "ISOSpeedRatings")
//        Console.WriteLine($"{value.Tag}: {value.GetValue()}");
//    }
//}

//public string Id { get; set; } = Guid.NewGuid().ToString();
//public string FileName { get; set; } = string.Empty;
//public float[] ImageEmbedding { get; set; } = Array.Empty<float>();
//public float[] GetVector()
//{
//    return ImageEmbedding;
//}
