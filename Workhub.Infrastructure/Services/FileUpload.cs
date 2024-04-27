using Microsoft.Extensions.Configuration;

namespace Workhub.Infrastructure.Services;
public static class FileHelper
{
    private static readonly IConfigurationRoot Configuration;

    static FileHelper()
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
    }

    public static string CreateDocFile(byte[] doc, string filename)
    {
        try
        {
            if (doc != null)
            {
                string basePath = Configuration["ImagePath"];
                string filePath = Path.Combine(basePath, filename);

                DirectoryInfo dir = new DirectoryInfo(basePath);
                if (!dir.Exists)
                    dir.Create();

                using (FileStream fStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    fStream.Write(doc, 0, doc.Length);
                }
                return filename;
            }
            return "";
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static byte[] GetDoc(string filepath)
    {
        try
        {
            string basePath = Configuration["ImagePath"];
            string filePath = Path.Combine(basePath, filepath);

            FileInfo oFile = new FileInfo(filePath);
            if (oFile.Exists)
            {
                using (FileStream oFileStream = oFile.OpenRead())
                {
                    long lBytes = oFileStream.Length;
                    byte[] fileData = new byte[lBytes];
                    oFileStream.Read(fileData, 0, Convert.ToInt32(lBytes));
                    return fileData;
                }
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

