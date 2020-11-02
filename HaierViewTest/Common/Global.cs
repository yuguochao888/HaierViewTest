using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaierViewTest.Common
{
   public static class Global
    {

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
    }
}
