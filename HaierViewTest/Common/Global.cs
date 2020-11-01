using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using DevExpress.Utils.CommonDialogs.Internal;
using Microsoft.Win32;

namespace HaierViewTest.Common
{
   public static class Global
    {
        public static byte[] BitmapImageToByteArray(BitmapImage bmp)
        {
            byte[] bytearray = null;
            try
            {
                Stream smarket = bmp.StreamSource;
                if (smarket != null && smarket.Length > 0)
                {
                    //设置当前位置
                    smarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(smarket))
                    {
                        bytearray = br.ReadBytes((int)smarket.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return bytearray;
        }

        public static byte[] BitmapImageToBytes(BitmapImage bmp)
        {
            byte[] ByteArray = null;

            try
            {
              MemoryStream stream=new MemoryStream();
              
             
                if (stream != null)
                {
                    stream.Position = 0;
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        ByteArray = br.ReadBytes((int)stream.Length);
                    }
                }




            }
            catch(Exception exception)
            {
                return null;
            }

            return ByteArray;
        }
        public static System.Drawing.Image GetImage(byte[] streamByte)
        {
            MemoryStream ms = new MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        public static byte[] ImageToBytes(System.Drawing.Image imgPhoto)
        {
            //将Image转换成流数据，并保存为byte[] 
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }

        public static BitmapImage OpenImageFile()
        {


            OpenFileDialog openfiledialog = new OpenFileDialog
            {
                Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*"
            };

            if ((bool)openfiledialog.ShowDialog())
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = new MemoryStream(File.ReadAllBytes(openfiledialog.FileName));
                image.EndInit();
                image.Freeze();
                return image;
            }

            return null;
            

        }
    }
}
