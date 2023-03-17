using ShoelessJoeAPI.App.Controllers;
using ShoelessJoeAPI.DataAccess.DataModels;

namespace ShoelessJoeAPI.App.FileIO
{
    public class FileWriter
    {
        private static string _path = "";

        public static void WriteError(Exception exception, string location)
        {
            if (!Directory.Exists(SecretConfig.ErrorPath))
            {
                Directory.CreateDirectory(SecretConfig.ErrorPath);
            }

            _path = SecretConfig.ErrorPath + GenerateFileName();

            if (!File.Exists(_path))
            {
                using StreamWriter sw = File.CreateText(_path);
                WriteError(sw, exception, location);
            }
            else
            {
                using StreamWriter sw = File.AppendText(_path);
                WriteError(sw, exception, location);
            }
        }

        private static string GenerateFileName()
        {
            return "Error_file_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        }

        private static void WriteError(StreamWriter writer, Exception exception, string location)
        {
            if (ValidateLocation(location))
            {
                writer.WriteLine(DateTime.Now.ToString("F"));
                writer.WriteLine($"Location: ${location} Controller");
                writer.WriteLine();
                writer.WriteLine($"Message: {exception.Message}");
                writer.WriteLine($"Source: {exception.Source}");
                writer.WriteLine($"Stack Trace: {exception.StackTrace}");
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("====================================================");
                writer.WriteLine();
                writer.WriteLine();
            }
        }

        private static bool ValidateLocation(string location)
        {
            return location switch
            {
                "Users" => true,
                "Shoes" => true,
                "Models" => true,
                "Manufacters" => true,
                _ => throw new ArgumentException("Not a valid exception"),
            };
        }
    }
}
