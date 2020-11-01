using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HaierViewTest.Common
{
public class ImageHelper
    {
        /// <summary>
        /// 将Bitmap图片转换成byte字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Bmp);
            ms.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// 将byte字节数组转换成Bitmap图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Bitmap bmp = new Bitmap(ms);
            ms.Close();
            return bmp;
        }

        /// <summary>
        /// 将BitmapImage图片转换成byte字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] BitmapImageToBytes(BitmapImage bmp)
        {
            byte[] bytes = null;
            Stream s = bmp.StreamSource;
            s.Position = 0; //很重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0。 
            using (BinaryReader br = new BinaryReader(s))
            {
                bytes = br.ReadBytes((int)s.Length);
            }
            return bytes;
        }


        /// <summary>
        /// 将byte字节数组转换成BitmapImage图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BitmapImage BytesToBitmapImage(byte[] bytes)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        /// <summary>
        /// 将Bitmap图片转换成BitmapImage图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            return BytesToBitmapImage(BitmapToBytes(bitmap));
        }

        /// <summary>
        /// 将BitmapImage图片转换成Bitmap图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            return BytesToBitmap(BitmapImageToBytes(bitmapImage));
        }
    }
 
}
