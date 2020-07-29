using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LowLevelDesign.Hexify;
using NExifTool;
namespace MP4DurationEditor
{
    class Program
    {
        //The duration is located after the lmvhd part of the file, so, the program
        //will search for that section.
        private static byte[] LmvhdHex = { 0x6C, 0x6D, 0x76, 0x68, 0x64 };
        private static void Main(string[] args)
        {
            var videoPath = Prompt("Enter video path: ");
 	    videoPath = videoPath.Trim();
            if(videoPath != null && !videoPath.EndsWith(".mp4")) throw new Exception("Only .mp4 videos are supported.");
            try
            {
                Console.WriteLine("Getting video metadata..");
                //Using exiftool to get the metadata of the file.
                var exifTool =  new ExifTool(new ExifToolOptions(){IncludeBinaryTags = true});
                var metadata = exifTool.GetTagsAsync(videoPath).Result;
                var duration = metadata.First(x => x.Name == "Duration") ??
                               throw new Exception("You entered a non-video file.");
                var hexDuration = "";
                if(duration.IsInteger)
                    hexDuration = duration.GetInt64().ToString("x8");
                else if (duration.IsDouble)
                    hexDuration = ((long)(duration.GetDouble() * 1000)).ToString("x8");
                Console.WriteLine("Converting video to hex..");
                var hexString = Hex.ToHexString(File.ReadAllBytes(videoPath));
                if (!hexString.Contains(Hex.ToHexString(LmvhdHex))) throw new Exception("The LMVHD part is not found. File is either unsupported, or corrupted.");
                //Get all text after the lmvhd part
                var lmvhdPart = hexString.Substring(hexString.IndexOf(Hex.ToHexString(LmvhdHex), StringComparison.Ordinal));
                var newDuration = Prompt($"Enter new duration (hex, {hexDuration.Length} characters): ");
                if(newDuration.Length != hexDuration.Length) throw new Exception("New duration's length is not equal to the original duration length.");
                if(!long.TryParse(newDuration, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out _)) throw new Exception("You entered a non-hex string.");
                //Replace only the first occurence of the correct pattern (the duration)
                Console.WriteLine("Replacing the correct bytes..");
                var regex = new Regex(Regex.Escape(hexDuration));
                var newText = regex.Replace(lmvhdPart, newDuration, 1);
                Console.WriteLine("Writing result to the file..");
                var newFilePath = Path.Combine(Path.GetDirectoryName(videoPath), Path.GetFileNameWithoutExtension(videoPath) + "_edited.mp4");
                File.Create(newFilePath).Close();
                //Replace the lmvhd part with the new, edited part
                hexString = hexString.Replace(lmvhdPart, newText);
                File.WriteAllBytes(newFilePath, Hex.FromHexString(hexString));
		Console.WriteLine($"Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        //Same as input("prompt") in python
        private static string Prompt(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? throw new ArgumentNullException();
        }
    }
}
