using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ImageUtility.Shell.Helpers
{
    public static class ImageHelpers
    {
        public static Size GetImageDimension(string imageFilePath)
        {
            using (var imageStream = File.OpenRead(imageFilePath))
            {
                var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                return new Size(decoder.Frames[0].PixelWidth, decoder.Frames[0].PixelHeight);
            }
        }

        public static Bitmap ResizeImageKeepingAspectRatio(Bitmap bmp, int maxWidth, int maxHeight)
        {
            var ratioX = (double) maxWidth / bmp.Width;
            var ratioY = (double) maxHeight / bmp.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int) (bmp.Width * ratio);
            var newHeight = (int) (bmp.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        public static void CompressImage(string sourcePath, string destPath, int quality)
        {
            using (var bmp = new Bitmap(sourcePath))
            {
                CompressImage(bmp, destPath, quality);
            }
        }

        public static void CompressImage(Bitmap bmp, string destPath, int quality)
        {
            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(Encoder.Quality, quality);

            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp.Save(destPath, jpgEncoder, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}