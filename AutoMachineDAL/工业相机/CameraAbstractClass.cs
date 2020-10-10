using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoMachineDAL
{//   
    public abstract class CameraAbstractClass
    {//   

        public bool CameraWorkMode = true;//CameraWorkMode=true:软件触发;CameraWorkMode=flase:硬件触发;


        /**********************************************软触发模式开始***************************************************************/


        public abstract void OpenCameraSoftTrigger(string DCF_Name);


        public abstract void ContinuousAcquisitonSoftTrigger();


        public abstract void SnapAcquisitionSoftTrigger(ref byte[] ImageBufferPtr);


        public abstract void StopCameraSoftTrigger();


        public abstract void CloseCameraSoftTrigger();



        /**********************************************软触发模式结束***************************************************************/



        /**********************************************硬触发模式开始***************************************************************/

        public abstract void OpenCameraHardTrigger(string DCF_Name);


        public abstract void ContinuousAcquisitonHardTrigger();


        public abstract void StopCameraHardTrigger();


        public abstract void CloseCameraHardTrigger();


        public abstract void EnableHardTrigger();


        public abstract void DisableHardTrigger();

        /**********************************************硬触发模式结束***************************************************************/


    }
}
