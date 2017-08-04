using JSNet.BaseSys;
using JSNet.Utilities;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.Log4NetTest
{
    [TestFixture]
    public class Log4NetTest
    {
        [Test]
        public void Test_Default()
        {
            //这个Demo，没有使用Log4NetUtil，直接调用log4net原生dll，结合Log4Net.xml的配置完成。

            //具体配置参考 Log4Net.xml
            //配置文件的路径在 AssemblyInfo.cs
            log4net.ILog log = log4net.LogManager.GetLogger("Loging");//获取一个日志记录器
            log.Fatal("这里是致命错误Fatal");
            log.Error("这里是普通错误Error");
            log.Warn("这里是一般警告错误Warn");
            log.Debug("这里是一般调试信息Debug");

            //写入数据库，这里的数据库连接字符串必须写在 Log4Net.xml 
            //log.Info(new MyLog("1", "13620834810", "已发短信", "成功发送"), new Exception("发生异常！！！"));
        }

        [Test]
        public void Test_Log4NetUtil_Init()
        {
            /*
             * 【说明】
             * 以下这句作用是添加配置文件，需放在  log4net.LogManager.GetLogger() 方法所在项目的 AssemblyInfo.cs 里
             * ps:若放在其他项目的 AssmblyInfo.cs 里，配置文件将不起作用，例如本例，是放在Utlities项目的 AssemblyInfo.cs 里，因为 log4net.LogManager.GetLogger() 方法在 Log4NetUtil.cs
             * [assembly: log4net.Config.XmlConfigurator(ConfigFile = @"..\..\..\Utilities\Xml\Log4Net.xml", Watch = true)]
             * ConfigFile 路径是以启动项目为相对路径的
             */


            //这个Demo，使用Log4NetUtil，结合Log4Net.xml的配置完成。

            Log4NetUtil.Init("SmsSendLog", "SystemLog", "ErrorLog");
            Log4NetUtil.SetConnString("AdoNetAppender_SQLServer", BaseSystemInfo.TestDbHelperConnection, "SmsSendLog");
            Log4NetUtil.SetConnString("AdoNetAppender_SQLServer2", BaseSystemInfo.TestDbHelperConnection, "SystemLog");
            Log4NetUtil.SetDefatultLog("SystemLog");

        }

        [Test]
        public void Test_Log4NetUtil_Init2()
        {
            /*
             * 【说明】
             * 以下这句作用是添加配置文件，需放在  log4net.LogManager.GetLogger() 方法所在项目的 AssemblyInfo.cs 里
             * ps:若放在其他项目的 AssmblyInfo.cs 里，配置文件将不起作用，例如本例，是放在Utlities项目的 AssemblyInfo.cs 里，因为 log4net.LogManager.GetLogger() 方法在 Log4NetUtil.cs
             * [assembly: log4net.Config.XmlConfigurator(ConfigFile = @"..\..\..\Utilities\Xml\Log4Net.xml", Watch = true)]
             * ConfigFile 路径是以启动项目为相对路径的
             */


            //这个Demo，使用Log4NetUtil，指定config路径

            string configPath = System.AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Utilities\Xml\Log4Net.xml";
            Log4NetUtil.Init(configPath, "SmsSendLog", "SystemLog", "ErrorLog");
            Log4NetUtil.SetConnString("AdoNetAppender_SQLServer", BaseSystemInfo.TestDbHelperConnection, "SmsSendLog");
            Log4NetUtil.SetConnString("AdoNetAppender_SQLServer2", BaseSystemInfo.TestDbHelperConnection, "SystemLog");
            Log4NetUtil.SetDefatultLog("SystemLog");

        }

        [Test]
        public void Test_Log4NetUtil_Log()
        {
            //写入数据库
            JSNet.Utilities.Log4NetUtil.Info(new SystemLog("XXX登陆系统失败"), new Exception("密码错误！"));//使用default logger
            JSNet.Utilities.Log4NetUtil.Info(new SmsLog("1", "13620834810", "已发短信", "成功发送"), "SmsSendLog");
            //写入txt
            JSNet.Utilities.Log4NetUtil.Error("出错了！！！", new Exception("真的出错了！！"), "ErrorLog");
        }


    }
}
