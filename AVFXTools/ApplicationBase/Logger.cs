using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.ApplicationBase
{
    public class Logger
    {
        public static List<LoggerItem> Items = new List<LoggerItem>();

        public static void Add(LoggerItem item)
        {
            Items.Add(item);
        }
        public static void Write(string text, LoggerType type)
        {
            Add(new LoggerItem(text, type));
        }
        public static void WriteInfo(string text)
        {
            Write(text, LoggerType.INFO);
        }
        public static void WriteWarning(string text)
        {
            Write(text, LoggerType.WARNING);
        }
        public static void WriteError(string text)
        {
            Write(text, LoggerType.ERROR);
        }
        public static void Clear()
        {
            Items = new List<LoggerItem>();
        }
    }

    public class LoggerItem
    {
        public string Text;
        public LoggerType Type;

        public LoggerItem (string text, LoggerType type)
        {
            Text = text;
            Type = type;
        }
    }

    public enum LoggerType
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2
    }
}
