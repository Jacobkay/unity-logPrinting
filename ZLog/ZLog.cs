using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ZTools
{
    internal class ZLog : MonoBehaviour
    {
        /// <summary>
        /// 文件名称格式
        /// 请注意，不要删除时间格式，否则会造成保存不成功
        /// </summary>
        public static string LogFileName
        {
            get{ return "Log{0:_yyyy_MM_dd}.txt"; } 
        }
        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static string LogPath
        {
            get { return Application.persistentDataPath + "/LogFile"; }
        }
        /// <summary>
        /// 日志保存最近几天的内容
        /// </summary>
        public static int SaveDays = 30;

        /// <summary>
        /// 每一行的打印内容
        /// </summary>
        public string LogContent
        {
            get
            {
                return "--------------------------" + Time + "--------------------------\n{0}\n{1}";
            }
        }

        public FileLogger FileLogger;

        static string Time
        {
            get
            {
                return DateTime.Now.ToString("[HH:mm:ss.ffffff]");
            }
        }
        private void Awake()
        {
            #if !UNITY_EDITOR
            DontDestroyOnLoad(gameObject);
            FileLogger = new FileLogger();
            Application.logMessageReceivedThreaded += LogMessage;
            #endif
        }
        void LogMessage(string condition, string stackTrace, LogType type)
        {
            if (!type.Equals(LogType.Warning))
            {
                FileLogger?.Write(string.Format(LogContent, condition, stackTrace));
            }
        }
        private void OnDestroy()
        {
            FileLogger?.OnDestroy();
            FileLogger = null;
            Application.logMessageReceivedThreaded -= LogMessage;
        }
    }
}
