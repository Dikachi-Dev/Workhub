using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Workhub.Infrastructure.Services;
public static class FileHelper
{
    private static readonly IConfigurationRoot Configuration;

    static FileHelper()
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
    }

    public static byte[] GetResizedImage(byte[] bytes, int newWidth)
    {
        using (MemoryStream ms = new System.IO.MemoryStream(bytes))
        {
            using (Image image = Image.FromStream(ms))
            {
                if (image.Width <= 200 || image.Height <= 200)
                    return bytes;

                using (MemoryStream stream = new MemoryStream())
                {
                    int newHeight = image.Height * newWidth / image.Width;
                    if (image.Width > newWidth && GraphicsSupportsPixelFormat(image.PixelFormat))
                    {
                        using (Image thumbnail = new Bitmap(newWidth, newHeight, image.PixelFormat))
                        {
                            Graphics thumbGraph = Graphics.FromImage(thumbnail);
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;


                            Rectangle rect = new Rectangle(0, 0, newWidth, newHeight);
                            thumbGraph.DrawImage(image, rect);


                            thumbnail.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                    else
                    {
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }


                    byte[] content = stream.ToArray();
                    return content;
                }
            }
        }
    }

    static bool GraphicsSupportsPixelFormat(System.Drawing.Imaging.PixelFormat format)
    {
        //  these pixel formats are not supported by the Graphics.FromImage() method 
        //  http://msdn.microsoft.com/en-us/library/system.drawing.graphics.fromimage.aspx 
        if (format == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
            format == System.Drawing.Imaging.PixelFormat.Format4bppIndexed ||
            format == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ||
            format == System.Drawing.Imaging.PixelFormat.Undefined ||
           format == System.Drawing.Imaging.PixelFormat.DontCare ||
            format == System.Drawing.Imaging.PixelFormat.Format16bppArgb1555 ||
            format == System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)
        {
            return false;
        }


        return true;
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

