using System;
using JSNet.Utilities;
using System.Diagnostics;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class UnitTest
    {
        [SetUp]
        [Category("NA")]
        public void BeforeTest()
        { Console.WriteLine("BeforeTest"); }

        [TestFixtureSetUp]
        [Category("NA")]
        public void BeforeAllTests()
        { Console.WriteLine("BeforeAllTests"); }

        [TearDown]
        [Category("NA")]
        public void AfterTest()
        { Console.WriteLine("AfterTest"); }

        [TestFixtureTearDown]
        [Category("NA")]
        public void AfterAllTests()
        { Console.WriteLine("AfterAllTests"); }

        [Test]
        public void Test_Utilities_BaseRandom_GetRandom()
        {
            Console.WriteLine("Test_Utilities_BaseRandom_GetRandom");
            int a = BaseRandom.GetRandom();
            int b = BaseRandom.GetRandom(0, 1000);
            string c = BaseRandom.GetRandomString();
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
        }
        [Test]
        public void Test_Utilities_DateUtil()
        {
            Console.WriteLine("Test_Utilities_DateUtil");
            string s = DateUtil.GetDayOfWeek("Monday");
            string s1 = DateUtil.GetDayOfWeek("Monday", true);
            Console.WriteLine(string.Format("Monday是星期{0}，Monday转为中文是{1}",s,s1));
            int a  = DateUtil.GetDaysOfMonth(Convert.ToDateTime("2008-2-29"));
            int a1  = DateUtil.GetDaysOfMonth(2008,2);
            Assert.AreEqual(29, a, "2月有" + a + "天");
            Assert.AreEqual(29, a1, "2月有" + a1 + "天");

            int b = DateUtil.GetDaysOfYear(2008);
            int b1 = DateUtil.GetDaysOfYear(Convert.ToDateTime("2008-2-29"));
            Assert.AreEqual(366, b, "2008年有" + b + "天");
            Assert.AreEqual(366, b1, "2008年有" + b1 + "天");

            string c = DateUtil.GetWeekNameOfDay(Convert.ToDateTime("2008-2-29"));
            Console.WriteLine(c);

            int d = DateUtil.GetWeekOfYear(Convert.ToDateTime("2008-2-29"));
            Console.WriteLine(string.Format("2008-02-29是2008年的第{0}周",d));

            bool e = DateUtil.IsDateTime("2008-02-01");
            Console.WriteLine(string.Format("2008-02-01是正确日期吗？{0}", e));
            bool e1 = DateUtil.IsDateTime("2008-02-30");
            Console.WriteLine(string.Format("2008-02-30是正确日期吗？{0}", e1));
            bool e2 = DateUtil.IsDateTime("2008-2-1");
            Console.WriteLine(string.Format("2008-2-1是正确日期吗？{0}", e2));
            bool e3 = DateUtil.IsDateTime("20080201");
            Console.WriteLine(string.Format("20080201是正确日期吗？{0}", e3));
            bool e4 = DateUtil.IsDateTime("2008-02-01 24:00:00");
            Console.WriteLine(string.Format("2008-02-01 24:00:00是正确日期吗？{0}", e4));
            bool e5 = DateUtil.IsDateTime("2008-02-01 23:00:00");
            Console.WriteLine(string.Format("2008-02-01 23:00:00是正确日期吗？{0}", e5));
            bool e6 = DateUtil.IsDateTime("2008-02-01 23:00:00.000");
            Console.WriteLine(string.Format("2008-02-01 23:00:00.000是正确日期吗？{0}", e6));

            string f = DateUtil.ToString(DateTime.Now, "yyyy-MM-dd HH:mm:ss");
            Console.WriteLine(string.Format("当前日期转换为‘yyyy-MM-dd HH:mm:ss’格式:{0}", f));
            string f1 = DateUtil.ToString(DateTime.Now, "yyyy年M月d日H时m分s秒");
            Console.WriteLine(string.Format("当前日期转换为‘yyyy年M月d日H时m分s秒’格式:{0}", f1));

            DateTime dt1 = new DateTime();
            DateTime dt2 =new DateTime();
            DateUtil.WeekRange(2008, 6, ref dt1, ref dt2);
            Console.WriteLine(string.Format("2008年第6周，起始时间为：{0}，结束时间为：{1}", dt1.ToString("yyyy-MM-dd"), dt2.ToString("yyyy-MM-dd")));
        }

        [Test]
        public void Test_Utilities_EnumDescription()
        {
            Console.WriteLine("Test_Utilities_EnumDescription");
            string a = TestEnum.test1.ToDescription();
            Console.WriteLine(string.Format("枚举test1的描述是：{0}", a));
        }
        [Test]
        public void Test_Utilities_LogUtil()
        {
            Console.WriteLine("Test_Utilities_LogUtil");

            try
            {
                throw new ArgumentNullException();
            }
            catch(Exception e)
            {
                LogUtil.WriteLog(e);
            }
        }

        [Test]
        public void Test_Utilities_ReflectionUtil()
        {
            Console.WriteLine("Test_Utilities_ReflectionUtil");
            System.Collections.Generic.List<Student> ls = new System.Collections.Generic.List<Student>();
            ls.Add(new Student()
            {
                Name="学生1",
                Age=13,
                StudentNo="20160001",
                Birth=Convert.ToDateTime("2000-10-01")
            });
            ls.Add(new Student()
            {
                Name = "学生2",
                Age = 15,
                StudentNo = "20170001A",
                Birth = Convert.ToDateTime("1999-10-1")
            });
            ls.Add(new Student()
            {
                Name="学生3"
            });
            System.Data.DataTable dt = ReflectionUtil.CreateTable(ls);

        }

        [Test]
        public void Test_Utilities_StringUtil()
        {
            Console.WriteLine("Test_Utilities_ReflectionUtil");
            string a = StringUtil.GetLike("field1", "三味书屋");
            Console.WriteLine(string.Format("GetLike三味书屋=>{0}", a));

            string a1 = StringUtil.GetSearchString("三味书屋", true);
            Console.WriteLine(string.Format("GetSearchString(\"三味书屋\",true)=>{0}", a1));

            string a2 = StringUtil.GetSearchString("三味书屋");
            Console.WriteLine(string.Format("GetSearchString(\"三味书屋\")=>{0}", a2));


            string[] arr = new string[]{"一","而","三层"};
            bool b = StringUtil.Exists(arr, "一");
            bool b1 = StringUtil.Exists(arr, "而");
            bool b2 = StringUtil.Exists(arr, "三层");
            Assert.AreEqual(true, b || b1 || b2);

            string[] arr1 = new string[] { "元老1", "元老2", "元老3" };
            string[] c = StringUtil.Concat(arr1, "我是新合并进来的");
            Console.WriteLine(string.Format("Concat()后的数组元素=>{0}", string.Join(",",c)));

            string[] arr2 = new string[] { "元老1", "元老2", "元老3" };
            string[] arr3 = new string[] { "新来的1", "新来的2", "新来的3" };
            string[] d = StringUtil.Concat(arr2, arr3);
            Console.WriteLine(string.Format("Concat()后的数组元素=>{0}", string.Join(",", d)));

            string[] arr4 = new string[] { "元老1", "元老2", "移除的元老3" };
            string[] e = StringUtil.Remove(arr4, "移除的元老3");
            Console.WriteLine(string.Format("Remove()后的数组元素=>{0}", string.Join(",", e)));

            string[] arr5 = new string[] { "元老1", "元老2", "元老3" };
            string f = StringUtil.ArrayToList(arr5);
            Console.WriteLine(string.Format("ArrayToList()后的数组元素=>{0}", f));

            string[] arr6 = new string[] { "元老1", "元老2", "元老3" };
            string g = StringUtil.ArrayToList(arr5,"'");
            Console.WriteLine(string.Format("ArrayToList()后的数组元素=>{0}", g));


            string h = StringUtil.RepeatString("重复", 4);
            Console.WriteLine(string.Format("RepeatString(\"重复\", 4)=>{0}", h));

            string i = StringUtil.GetFirstPinyin("罗小明");
            Console.WriteLine(string.Format("GetFirstPinyin(\"罗小明\")=>{0}", i));

            string j = StringUtil.GetPinyin("罗小明");
            Console.WriteLine(string.Format("GetPinyin(\"罗小明\")=>{0}", j));
        }
    }
    


    
}
