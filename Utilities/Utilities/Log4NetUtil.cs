﻿using System;
using System.Data;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Collections.Generic;

namespace JSNet.Utilities
{
    /// <summary>
    /// 目前做不到多个Logger
    /// </summary>
    public class Log4NetUtil
    {
        #region 变量定义
        //定义信息的二次处理
        public static event Action<string> OutputMessage;

        private static ILog _defatulLog;

        private static Dictionary<string, ILog> _logDics = new Dictionary<string, ILog>();

        private static object locker = new object();
        #endregion

        #region 定义属性

        /// <summary>
        /// 初始化Log4net
        /// </summary>
        /// <param name="configPath">默认调用 AssemblyInfo.cs 的指定的配置文件</param>
        /// <param name="loggerNames"></param>
        public static void Init(string configPath = "", params string[] loggerNames)
        {
            if (!string.IsNullOrEmpty(configPath))
            {
                SetConfig(configPath);
            }
            foreach (string loggerName in loggerNames)
            {
                ILog log = LogManager.GetLogger(loggerName);
                if(_logDics.ContainsKey(loggerName))
                {
                    _logDics[loggerName] = log;
                }
                else
                {
                    _logDics.Add(loggerName, log);
                }
            }
            return;
        }


        /// <summary>
        /// 初始化 Log配置 的数据库连接字符串
        /// </summary>
        /// <param name="appenderName">XML文档 对应的appenderName</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void SetConnString(string appenderName, string connectionString, string loggerName = "")
        {
            AdoNetAppender targetApder = null;
            ILog log = GetLog(loggerName);

            var appenders = log.Logger.Repository.GetAppenders();
            for (int i = 0; i < appenders.Length; i++)
            {
                if (!appenderName.Equals(appenders[i].Name))
                {
                    continue;
                }
                if (appenders[i] is log4net.Appender.AdoNetAppender)
                {
                    targetApder = appenders[i] as log4net.Appender.AdoNetAppender;
                    break;
                }
            }
            if (targetApder == null)
            {
                throw new ArgumentNullException("AdoNetAppender");
            }
            targetApder.ConnectionString = connectionString;
            targetApder.ActivateOptions();
            return;
        }

        /// <summary>
        /// 设置默认的Log，如果初始化时只有一个元素，则无需设置默认LOG。
        /// </summary>
        /// <param name="loggerName"></param>
        public static void SetDefatultLog(string loggerName)
        {
            if(!_logDics.ContainsKey(loggerName))
            {
                throw new Exception("logDic not contains this log");
            }
            _defatulLog = _logDics[loggerName];
        }

        /// <summary>
        /// 获取对应的LOG对象
        /// </summary>
        /// <returns></returns>
        private static ILog GetLog(string loggerName = "")
        {
            if (_logDics.Count == 0)
            {
                throw new Exception("logDics is not init");
            }

            if (string.IsNullOrEmpty(loggerName))
            {
                if (_defatulLog == null)
                {
                    throw new ArgumentNullException("DefatulLog");
                }
                return _defatulLog;
            }

            if (!_logDics.ContainsKey(loggerName))
            {
                throw new Exception("logDic not contains this loggerName");
            }
            return _logDics[loggerName];
        }

        private static void SetConfig(string configPath)
        {
            XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(configPath)); 
        }

        #endregion

        #region 定义信息二次处理方式
        private static void HandMessage(object Msg)
        {
            if (OutputMessage != null)
            {
                OutputMessage(Msg.ToString());
            }
        }
        private static void HandMessage(object Msg, Exception ex)
        {
            if (OutputMessage != null)
            {
                OutputMessage(string.Format("{0}:{1}", Msg.ToString(), ex.ToString()));
            }
        }
        private static void HandMessage(string format, params object[] args)
        {
            if (OutputMessage != null)
            {
                OutputMessage(string.Format(format, args));
            }
        }
        #endregion

        #region 封装Log4net
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Debug(object message,string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message);
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Debug(object message, Exception ex, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message, ex);
            if (log.IsDebugEnabled)
            {
                log.Debug(message, ex);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, string loggerName = "", params object[] args)
        {
            ILog log = GetLog(loggerName);
            HandMessage(format, args);
            if (log.IsDebugEnabled)
            {
                log.DebugFormat(format, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Error(object message, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message);
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Error(object message, Exception ex, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message, ex);
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, string loggerName = "", params object[] args)
        {
            ILog log = GetLog(loggerName);
            HandMessage(format, args);
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat(format, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Fatal(object message, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message);
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Fatal(object message, Exception ex, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message, ex);
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, string loggerName = "", params object[] args)
        {
            ILog log = GetLog(loggerName);
            HandMessage(format, args);
            if (log.IsFatalEnabled)
            {
                log.FatalFormat(format, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Info(object message, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message);
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Info(object message, Exception ex, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message, ex);
            if (log.IsInfoEnabled)
            {
                log.Info(message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, string loggerName = "", params object[] args)
        {
            ILog log = GetLog(loggerName);
            HandMessage(format, args);
            if (log.IsInfoEnabled)
            {
                log.InfoFormat(format, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Warn(object message, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message);
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        public static void Warn(object message, Exception ex, string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            HandMessage(message, ex);
            if (log.IsWarnEnabled)
            {
                log.Warn(message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="loggerName">默认使用 default logger</param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, string loggerName = "", params object[] args)
        {
            ILog log = GetLog(loggerName);
            HandMessage(format, args);
            if (log.IsWarnEnabled)
            {
                log.WarnFormat(format, args);
            }
        }
        #endregion

        #region 手动加载配置 未测试 不使用
        /*
         *以下方法没经过测试
         */
        //public static void LoadADONetAppender()
        //{
        //    Hierarchy hier = LogManager.GetLoggerRepository() as log4net.Repository.Hierarchy.Hierarchy;
        //    if (hier != null)
        //    {
        //        log4net.Appender.AdoNetAppender appender = new log4net.Appender.AdoNetAppender();
        //        appender.Name = "AdoNetAppender";
        //        appender.CommandType = CommandType.Text;
        //        appender.BufferSize = 1;
        //        appender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        //        appender.ConnectionString = ConnectionString;
        //        appender.CommandText = @"INSERT INTO Log ([Date],[Thread],[Level],[Logger],[Message],[Exception]) VALUES (@log_date, @thread, @log_level, @logger, @message, @exception)";
        //        appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
        //        appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@thread", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) });
        //        appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
        //        appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
        //        appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message")) });
        //        appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
        //        appender.ActivateOptions();
        //        BasicConfigurator.Configure(appender);
        //    }
        //}

        //public static void LoadFileAppender()
        //{
        //    FileAppender appender = new FileAppender();
        //    appender.Name = "FileAppender";
        //    appender.File = "error.log";
        //    appender.AppendToFile = true;

        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = _ConversionPattern;
        //    patternLayout.ActivateOptions();
        //    appender.Layout = patternLayout;

        //    //选择UTF8编码，确保中文不乱码。
        //    appender.Encoding = Encoding.UTF8;

        //    appender.ActivateOptions();
        //    BasicConfigurator.Configure(appender);

        //}

        //public static void LoadRollingFileAppender()
        //{
        //    RollingFileAppender appender = new RollingFileAppender();
        //    appender.AppendToFile = true;
        //    appender.Name = "RollingFileAppender";
        //    appender.DatePattern = "yyyy-MM-dd HH'时.log'";
        //    appender.File = "Logs/";
        //    appender.AppendToFile = true;
        //    appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
        //    appender.MaximumFileSize = "500kb";
        //    appender.MaxSizeRollBackups = 10;
        //    appender.StaticLogFileName = false;


        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = _ConversionPattern;
        //    patternLayout.ActivateOptions();
        //    appender.Layout = patternLayout;

        //    //选择UTF8编码，确保中文不乱码。
        //    appender.Encoding = Encoding.UTF8;

        //    appender.ActivateOptions();
        //    BasicConfigurator.Configure(appender);
        //}

        //public static void LoadConsoleAppender()
        //{
        //    ConsoleAppender appender = new ConsoleAppender();
        //    appender.Name = "ConsoleAppender";

        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = _ConversionPattern;
        //    patternLayout.ActivateOptions();
        //    appender.Layout = patternLayout;

        //    appender.ActivateOptions();
        //    BasicConfigurator.Configure(appender);
        //}

        //public static void LoadTraceAppender()
        //{
        //    TraceAppender appender = new TraceAppender();
        //    appender.Name = "TraceAppender";

        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = _ConversionPattern;
        //    patternLayout.ActivateOptions();
        //    appender.Layout = patternLayout;

        //    appender.ActivateOptions();
        //    BasicConfigurator.Configure(appender);
        //}

        //public static void LoadEventLogAppender()
        //{
        //    EventLogAppender appender = new EventLogAppender();
        //    appender.Name = "EventLogAppender";

        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = _ConversionPattern;
        //    patternLayout.ActivateOptions();
        //    appender.Layout = patternLayout;

        //    appender.ActivateOptions();
        //    BasicConfigurator.Configure(appender);
        //}
        #endregion

        #region 定义常规应用程序中未处理的异常信息记录方式
        public static void LoadUnhandledException(string loggerName = "")
        {
            ILog log = GetLog(loggerName);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler
                 ((sender, e) =>
                 {
                     log.Fatal("未处理的异常", e.ExceptionObject as Exception);
                 });
        }
        #endregion
    }
}