using JSNet.BaseSys;
using JSNet.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    [TestFixture]
    class ConfigureTest
    {
        [Test]
        public void Test_GetOptions()
        {
            string[] strs = XmlConfigHelper.GetOptions("CurrentLanguage");
            //Console.WriteLine(string.Join(",", strs));
            string s = System.AppDomain.CurrentDomain.ToString();


            Log4NetUtil.OutputMessage += Log4NetUtil_OutputMessage;
        }

        void Log4NetUtil_OutputMessage(string obj)
        {
            throw new NotImplementedException();
        }

        public void Test<T>(Action<T> action, T p)
        {
            action(p);
        }
    }
}
