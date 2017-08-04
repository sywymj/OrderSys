using System;
using JSNet.Utilities;
using System.Diagnostics;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;
using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using System.Data.Common;
using System.Data;
using JSNet.DbUtilities;
using JSNet.BaseSys;

namespace UnitTest
{
    [TestFixture]
    public class DbTest
    {
        /// <summary>
        /// 测试 dbHelper 对象是否一样
        /// </summary>
        [Test]
        public void Test_SameDbHelper()
        {
            object[] objs = new object[2];
            for (int i = 0; i < objs.Length; i++)
            {
                IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
                dbHelper.Open();
                objs[i] = dbHelper;
                dbHelper.Dispose();
            }

            for (int i = 0; i < objs.Length; i++)
            {
                for(int j = 0;j<objs.Length;j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    Assert.AreNotSame(objs[i], objs[j]);
                }
            }
        }

        /// <summary>
        /// 测试 dbHelper 资源释放
        /// </summary>
        [Test]
        public void Test_DisposeDbHelper()
        {
            try
            {
                object[] objs = new object[10000];
                for (int i = 0; i < objs.Length; i++)
                {
                    IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.TestDbHelperConnectionString, BaseSystemInfo.CenterDbType);
                    dbHelper.Open();
                    objs[i] = dbHelper;
                    dbHelper.Dispose();
                }
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Test_CloseDbHelper()
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    //连接字符串的连接池只有5个，若不及时关闭数据库，会报错，此处测试数据库关闭函数
                    IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.TestDbHelperConnectionString, BaseSystemInfo.CenterDbType);
                    dbHelper.Open();
                    dbHelper.Close();//若不关闭，必需要报错！！
                    Console.WriteLine(i.ToString());
                }
            }
            catch
            {
                Assert.Fail();
            }
        }


        [Test]
        public void Test_InsertManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager = new UserManager(dbHelper);
            UserEntity userEntity = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = new string('a', 50),
                F3 = "333",
                DateTimeType = DateTime.Now,
                NumberType = new Random().Next(1000),
                DoubleType = new Random().Next(1000)*0.21,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            string id = userManager.Insert(userEntity);
            Console.WriteLine("Insert的id:" + id);

            IDbHelper dbHelper1 = DbHelperFactory.GetHelper(BaseSystemInfo.BusinessDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager1 = new UserManager(dbHelper1);
            UserEntity userEntity1 = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = new string('a', 50),
                F3 = "333",
                DateTimeType = DateTime.Now,
                NumberType = new Random().Next(1000),
                DoubleType = new Random().Next(1000) * 0.21,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            string id1 = userManager1.Insert(userEntity1);
            Console.WriteLine("Insert1的id1:" + id1);

            IDbHelper dbHelper2 = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager2 = new UserManager(dbHelper2);
            UserEntity userEntity2 = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = new string('a', 50),
                F3 = "333",
                DateTimeType = DateTime.Now,
                NumberType = new Random().Next(1000),
                DoubleType = new Random().Next(1000) * 0.21,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            string id2 = userManager2.Insert(userEntity2, false);
            Console.WriteLine("Insert2的id2:" + id2);


            UserEntity userEntity3 = new UserEntity();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldF1, DateTime.Now.ToString()));
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldF2, "2222"));
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldF3, "3333"));
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldDateTimeType, null));
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldNumberType, null));
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldNullType, null));

            IDbHelper dbHelper3 = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager3 = new UserManager(dbHelper3);
            string id3 = userManager3.Insert(kvps);
            Console.WriteLine("Insert3的id3:" + id3);

            IDbHelper dbHelper4 = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager4 = new UserManager(dbHelper4);
            string id4 = userManager3.Insert(kvps,false);
            Console.WriteLine("Insert4的id4:" + id4);
        }

        [Test]
        public void Test_UpdateManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager = new UserManager(dbHelper);
            UserEntity userEntity = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = "update0",
                F3 = "333U",
                DateTimeType = DateTime.Now,
                NumberType = new Random().Next(1000),
                DoubleType = new Random().Next(1000) * 0.21,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            userManager.Update(userEntity, 23078);

            UserEntity userEntity1 = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = "update1",
                F3 = "333U",
                DateTimeType = DateTime.Now,
                NumberType = new Random().Next(1000),
                DoubleType = new Random().Next(1000) * 0.21,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            userManager.Update(userEntity1, "2222",userEntity1.FieldF2);

            UserEntity userEntity2 = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = "update2",
                F3 = "333U2",
                DateTimeType = DateTime.Now,
                NumberType = 70,
                DoubleType = 70.11,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            WhereClause clause  = new WhereClause(userEntity2.FieldDoubleType, Comparison.LessThan, 70);
            clause.AddClause(LogicOperator.And,Comparison.GreaterThan,40);
            userManager.Update(clause, userEntity2);

            UserEntity userEntity3 = new UserEntity();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(userEntity3.FieldF1, "update3"));
            userManager.Update(kvps, 23075);

            UserEntity userEntity4 = new UserEntity();
            List<KeyValuePair<string, object>> kvps1 = new List<KeyValuePair<string, object>>();
            kvps1.Add(new KeyValuePair<string, object>(userEntity4.FieldF1, "update4"));
            userManager.Update(kvps, 864, userEntity4.FieldNumberType);
        }

        [Test]
        public void Test_DeleteManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager = new UserManager(dbHelper);
            UserEntity userEntity = new UserEntity();
            userManager.Delete(23081);

            WhereClause clause = new WhereClause(userEntity.FieldNumberType,Comparison.GreaterOrEquals,4);
            clause.AddClause(LogicOperator.And,Comparison.LessOrEquals,6);
            userManager.Delete(clause);

            WhereStatement statement = new WhereStatement(dbHelper);
            statement.Add(userEntity.FieldNumberType,Comparison.GreaterOrEquals,2);
            userManager.Delete(statement);

            //userManager.SetDeleted(23081);
        }

        [Test]
        public void Test_ThreadsDeleteManager()
        {
            for (int i = 1; i <= 3; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(_Bingfa_Delete_Manager_Fun));
                thread.Start(i);
            }
        }

        [Test]
        public void Test_WhereClause()
        {
            //WhereClause 是where从句的子句，
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserEntity entity = new UserEntity();

            WhereClause clause = new WhereClause(entity.FieldF1, Comparison.Equals, 1);
            clause.AddClause(LogicOperator.Or, Comparison.In, new int[] { 1, 2, 3 });// TODO
            WhereStatement statement = new WhereStatement(dbHelper);
            statement.Add(clause);
            string where = statement.BuildWhereStatement();
            Assert.AreEqual(@"(F1 = 1 OR F1 IN ('1', '2', '3'))", where.Trim());
            Console.WriteLine(where);

            WhereClause clause1 = new WhereClause(entity.FieldF1, Comparison.Equals, 1);
            clause1.AddClause(LogicOperator.Or, Comparison.In, 1);// TODO
            WhereStatement statement1 = new WhereStatement(dbHelper);
            statement1.Add(clause1);
            string where1 = statement1.BuildWhereStatement();
            Assert.AreEqual(@"(F1 = 1 OR F1 IN (1))", where1.Trim());
            Console.WriteLine(where1);

            WhereClause clause2 = new WhereClause(entity.FieldF1, Comparison.Equals, 1);
            clause2.AddClause(LogicOperator.Or, Comparison.In, new string[] { "1", "2", "3" });// TODO
            WhereStatement statement2 = new WhereStatement(dbHelper);
            statement2.Add(clause2);
            string where2 = statement2.BuildWhereStatement();
            Assert.AreEqual(@"(F1 = 1 OR F1 IN ('1', '2', '3'))", where2.Trim());
            Console.WriteLine(where2);

            WhereClause clause3 = new WhereClause(entity.FieldF1, Comparison.Equals, DBNull.Value);
            clause3.AddClause(LogicOperator.Or, Comparison.In, new string[] { "1", "2", "3" });// TODO
            WhereStatement statement3 = new WhereStatement(dbHelper);
            statement3.Add(clause3);
            string where3 = statement3.BuildWhereStatement();
            Assert.AreEqual(@"(F1 IS NULL OR F1 IN ('1', '2', '3'))", where3.Trim());
            Console.WriteLine(where3);
        }

        [Test]
        public void Test_WhereStatement()
        {
            //WhereStatement 是Where从句
            //{Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%')) OR {Level2}(Age BETWEEN 15 AND 20)，同一个LEVEL用AND连接，不同LEVEL用OR连接
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserEntity entity = new UserEntity();
            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Clear();
            WhereStatement statement = new WhereStatement(dbHelper);
            statement.Add(new WhereClause(entity.FieldF1, Comparison.Equals, 1));//and them together
            statement.Add(new WhereClause(entity.FieldF2, Comparison.Equals, 2));//and them together
            string whereSql = statement.BuildWhereStatement();
            string whereSqlWithParm = statement.BuildWhereStatement(true, out parameters);
            Console.WriteLine(whereSql + "，" + whereSqlWithParm);

            parameters.Clear();
            WhereStatement statement1 = new WhereStatement(dbHelper);
            statement1.Add(new WhereClause(entity.FieldF1, Comparison.In, new string[]{"1","2"}), 1);//or them together
            statement1.Add(new WhereClause(entity.FieldF1, Comparison.Equals, 2), 2);//or them together
            string whereSql1 = statement1.BuildWhereStatement();
            string whereSqlWithParm1 = statement1.BuildWhereStatement(true, out parameters);
            Console.WriteLine(whereSql1 + "，" + whereSqlWithParm1);

            parameters.Clear();
            WhereStatement statement2 = new WhereStatement(dbHelper);
            statement2.Add(entity.FieldF1, Comparison.Like, "1%'", 1);//same as new WhereClause
            statement2.Add(entity.FieldF2, Comparison.NotLike, "1%'--", 1);//same as new WhereClause
            statement2.Add(entity.FieldF2, Comparison.Equals, "1", 2);//same as new WhereClause
            string whereSql2 = statement2.BuildWhereStatement();
            string whereSqlWithParm2 = statement2.BuildWhereStatement(true, out parameters);
            Console.WriteLine(whereSql2 + "，" + whereSqlWithParm2);
        }

        [Test]
        public void Test_GetIdsManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            //where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "333"));//Test Return Data
            where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "3334"));//Test Return String[0]

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(entity.PrimaryKey,Sorting.Ascending);
            orderby.Add(entity.FieldDateTimeType,Sorting.Descending);
            
            string[] ids =  manager.GetIds(where,orderby);
            Console.WriteLine(ids);
        }

        [Test]
        public void Test_GetIdsByPageManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "333"));

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(entity.PrimaryKey,Sorting.Ascending);

            int count = 0;
            string[] ids = manager.GetIdsByPage(where, out count, 2, 2, orderby);
            Console.WriteLine(ids);
        }

        [Test]
        public void Test_GetSingleManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "333"));
            entity = manager.GetSingle(23086, whereStatement: where);
        }


        [Test]
        public void Test_GetCountManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "333"));
            int count = manager.GetCount(where);
            Console.WriteLine(count);
        }

        [Test]
        public void Test_GetListManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            where.Add(new WhereClause(entity.FieldF3, Comparison.In,new string[]{"333","3333"}));
            where.Add(new WhereClause(entity.FieldF2, Comparison.Like, "aa%"));

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(entity.PrimaryKey,Sorting.Ascending);

            int count = 0;
            List<UserEntity> list = manager.GetList(where, out count, orderby);
            List<UserEntity> listByPage = manager.GetListByPage(where, out count, 2, 2, orderby);
        }

        [Test]
        public void Test_GetDataTableManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "333"));

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(entity.PrimaryKey,Sorting.Ascending);

            int count =0;
            DataTable dt1 = manager.GetDataTable(where, out count, orderby);
            DataTable dt2 = manager.GetDataTableByPage(where, out count, 2, 2, orderby);
        }

        [Test]
        public void Test_ExistsManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager manager = new UserManager(dbHelper);
            UserEntity entity = new UserEntity();

            WhereStatement where = new WhereStatement(dbHelper);
            where.Add(new WhereClause(entity.FieldF3, Comparison.Equals, "333"));

            bool b = manager.Exists(where);
            bool b1 = manager.Exists(entity.FieldF2, "222");
        }

        [Test]
        public void Test_BuildPagingSQL()
        {

        }


        [Test]
        public void Test_BatchAddManager()
        {
            //可以测试 数据库连接 一直打开直至插入完成才close 跟 每次插入都close 的性能
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            dbHelper.Open();
            UserManager userManager = new UserManager(dbHelper);
            for(int i = 0;i<1000;i++)
            {
                //dbHelper.Open();
                UserEntity userEntity = new UserEntity()
                {
                    F1 = DateTime.Now.ToString(),
                    F2 = new string('a', 50),
                    F3 = "333",
                    DateTimeType = DateTime.Now,
                    NumberType = i,
                    DoubleType = new Random().Next(1000) * 0.21,
                    FloatType = 1.23456789f,
                    DecimalType = 300.055M,
                    NullType = null,
                };
                userManager.Insert(userEntity);
                //dbHelper.Close();
            }
            dbHelper.Close();
        }

        [Test]
        public void Test_TransManager()
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            dbHelper.Open();
            try
            {
                dbHelper.BeginTransaction();
                UserManager userManager = new UserManager(dbHelper);
                for (int i = 0; i < 10; i++)
                {
                    if (i == 5) { throw new Exception(); }
                    //dbHelper.Open();
                    UserEntity userEntity = new UserEntity()
                    {
                        F1 = DateTime.Now.ToString(),
                        F2 = new string('a', 50),
                        F3 = "333",
                        DateTimeType = DateTime.Now,
                        NumberType = i,
                        DoubleType = new Random().Next(1000) * 0.21,
                        FloatType = 1.23456789f,
                        DecimalType = 300.055M,
                        NullType = null,
                    };
                    userManager.Insert(userEntity);
                    //dbHelper.Close();
                }
                dbHelper.CommitTransaction();
            }
            catch
            {
                dbHelper.RollbackTransaction();
            }
            finally
            {
                dbHelper.Close();
            }
        }

        /// <summary>
        /// 测试 Manager对象 并发插入
        /// </summary>
        [Test]
        public void Test_BingfaAddManager()
        {
            //模拟并发插入，要等所有的函数执行完才知道有没有异常
            for (int i = 1; i <= 1000; i++)
            {
                //QueueUserWorkItem()方法：将工作任务排入线程池。  
                // Fun 表示要执行的方法(与WaitCallback委托的声明必须一致)。  
                // i   为传递给Fun方法的参数(obj将接受)。  
                ThreadPool.QueueUserWorkItem(new WaitCallback(_Bingfa_Add_Manage_Fun), i);
            }

            //for (int i = 1; i <= 12; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(_Bingfa_Add_Manage_Fun));
            //    thread.Start(i);
            //}
        }

        private void _Bingfa_Add_Manage_Fun(object obj)
        {
            int n = (int)obj;
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager = new UserManager(dbHelper);
            UserEntity userEntity = new UserEntity()
            {
                F1 = DateTime.Now.ToString(),
                F2 = new string('a', 50),
                F3 = "333",
                DateTimeType = DateTime.Now,
                NumberType = n,
                DoubleType = new Random().Next(1000) * 0.21,
                FloatType = 1.23456789f,
                DecimalType = 300.055M,
                NullType = null,
            };
            string id = userManager.Insert(userEntity);
            Console.WriteLine("returnId：" + id );
        }

        private void _Bingfa_Delete_Manager_Fun(object obj)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString, BaseSystemInfo.CenterDbType);
            UserManager userManager = new UserManager(dbHelper);
            UserEntity userEntity = new UserEntity();

            WhereClause clause = new WhereClause(userEntity.FieldNumberType, Comparison.GreaterOrEquals, 4);
            clause.AddClause(LogicOperator.And, Comparison.LessOrEquals, 6);
            userManager.Delete(clause);
        }
    }
}
