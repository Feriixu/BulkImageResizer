using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input Path: ");
            var path = Console.ReadLine();
            Console.Write("Target width: ");
            var width = Convert.ToInt32(Console.ReadLine());
            Console.Write("Target height: ");
            var height = Convert.ToInt32(Console.ReadLine());
            var folderName = width + "x" + height;
            Directory.CreateDirectory(Environment.CurrentDirectory + "/" + folderName);
            foreach (var image in System.IO.Directory.GetFiles(path, "*.png"))
            {
                Console.WriteLine(Path.GetFileName(image));
                var resized = ResizeImage(Image.FromFile(image), width, height);
                resized.Save(folderName + "/" + Path.GetFileNameWithoutExtension(image) + "_resized.png", ImageFormat.Png);
            }
        }


        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
