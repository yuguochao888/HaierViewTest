using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using DataModel;

namespace HaierViewTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public static ViewTestEntities ViewTestEntities=new ViewTestEntities();

        public App() : base()
        {
            Assembly assembly=Assembly.GetAssembly(this.GetType());
            string resurceName = assembly.GetName().Name + ".g";
            ResourceManager rm=new ResourceManager(resurceName,assembly);
            var res = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);
          //  var ss=res.GetObject(,true)
         
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ViewTestEntities>());
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    //图片的名字
        //    SplashScreen s = new SplashScreen("StartUp.png");
        //    //显示图片
        //    s.Show(false);
        //    //图片显示多久，分别是时、分、秒；
        //    //设置的时间到了图片自动消失，没到一直显示。
        //    s.Close(new TimeSpan(0, 0, 3));
        //    base.OnStartup(e);
        //}

    }
}
