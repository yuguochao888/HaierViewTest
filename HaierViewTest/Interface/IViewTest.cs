using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace HaierViewTest.Interface
{
    /// <summary>
    /// 视觉检测接口，返回检测结果和检测图片
    /// </summary>
   public interface IViewTest
   {
       /// <summary>
       /// 获取图片结果
       /// </summary>
       /// <returns></returns>
       Image Getimage();

       /// <summary>
       /// 获取根据视觉检测的结果
       /// </summary>
       /// <returns></returns>
       bool GetTestResult();

      /// <summary>
      /// 开始测试
      /// </summary>
      /// <param name="message">返回信息</param>
      /// <param name="hObject">返回hoject</param>
       void StartAction( out string message,out HObject hObject);

   }
}
