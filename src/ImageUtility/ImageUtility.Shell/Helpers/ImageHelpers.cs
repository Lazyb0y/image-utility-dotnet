using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageUtility.Shell.Helpers
{
    public static class ImageHelpers
    {
        public static void CompressImage(string sourcePath, string destPath, int quality)
        {
            using (var bmp = new Bitmap(sourcePath))
            {
                var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                var myEncoderParameters = new EncoderParameters(1);
                var myEncoderParameter = new EncoderParameter(Encoder.Quality, quality);

                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp.Save(destPath, jpgEncoder, myEncoderParameters);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}