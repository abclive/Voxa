using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Voxa.Utils
{
    public struct LoggerMessage
    {
        public ConsoleColor ForegroundColor;
        public ConsoleColor BackgroundColor;
        public string       Message;

        public LoggerMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Message = message;
        }

        public LoggerMessage(string message, ConsoleColor foregroundColor)
        {
            Message = message;
            ForegroundColor = foregroundColor;
            BackgroundColor = Console.BackgroundColor;
        }

        public LoggerMessage(string message)
        {
            Message = message;
            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
        }
    }

    public static class Logger
    {
        const int SLEEP_INTERVAL = 100;
        const int MAX_LOGS = 200;

        private static Dictionary<string, LoggerMessage> stickyLogs = new Dictionary<string, LoggerMessage>();
        private static List<LoggerMessage> logs = new List<LoggerMessage>();

        private static bool isDirty = false;

        static Logger()
        {
            Thread loggerThread = new Thread(() => {
                while (true) {
                    Thread.Sleep(SLEEP_INTERVAL);
                    UpdateConsole();
                }
            });
            loggerThread.IsBackground = true;
            loggerThread.Start();
        }

        public static void AddStickyInfo(string identifier)
        {
            stickyLogs.Add(identifier, new LoggerMessage(""));
        }

        public static void AddStickyInfo(string identifier, string message)
        {
            stickyLogs.Add(identifier, new LoggerMessage(message));
        }

        public static void AddStickyInfo(string identifier, LoggerMessage lMessage)
        {
            stickyLogs.Add(identifier, lMessage);
        }

        public static void UpdateStickyInfo(string identifier, string message)
        {
            LoggerMessage lMessage = stickyLogs[identifier];
            lMessage.Message = message;
            stickyLogs[identifier] = lMessage;
        }

        public static void UpdateStickyInfo(string identifier, LoggerMessage lMessage)
        {
            stickyLogs[identifier] = lMessage;
        }

        public static void RemoveStickyInfo(string identifier)
        {
            stickyLogs.Remove(identifier);
        }

        public static void Info(string message)
        {
            if (logs.Count >= MAX_LOGS) {
                logs.RemoveAt(0);
            }
            logs.Add(new LoggerMessage(message));
            isDirty = true;
        }

        public static void Success(string message)
        {
            if (logs.Count >= MAX_LOGS) {
                logs.RemoveAt(0);
            }
            logs.Add(new LoggerMessage(message, ConsoleColor.Green));
            isDirty = true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Warning(string message)
        {
            if (logs.Count >= MAX_LOGS) {
                logs.RemoveAt(0);
            }
            Type callerType = new StackTrace(1, false).GetFrame(1).GetMethod().DeclaringType;
            logs.Add(new LoggerMessage($"[{callerType.Name}::Warn]: {message}", ConsoleColor.Yellow));
            isDirty = true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Error(string message)
        {
            if (logs.Count >= MAX_LOGS) {
                logs.RemoveAt(0);
            }
            Type callerType = new StackTrace(1, false).GetFrame(1).GetMethod().DeclaringType;
            logs.Add(new LoggerMessage($"[{callerType.Name}::Error]: {message}", ConsoleColor.Red));
            isDirty = true;
        }

        public static void UpdateStickyDisplay()
        {
            Console.SetCursorPosition(0, 0);
            int i = 0;
            foreach (KeyValuePair<string, LoggerMessage> stickyLog in stickyLogs.ToList()) {
                Console.WriteLine(new String(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, i++);
                printLoggerMessage(stickyLog.Value);
            }
        }

        public static void UpdateConsole()
        {
            UpdateStickyDisplay();
            if (isDirty) {
                isDirty = false;
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                foreach (KeyValuePair<string, LoggerMessage> stickyLog in stickyLogs.ToList()) {
                    printLoggerMessage(stickyLog.Value);
                }
                Console.WriteLine(new String('=', Console.WindowWidth));
                int startLog = Console.CursorTop;
                int maxLogs = Console.WindowHeight - startLog - 1;
                List<LoggerMessage> displayLogs = logs.ToList().Skip((logs.Count - maxLogs < 0) ? 0 : logs.Count - maxLogs).ToList();
                foreach (LoggerMessage lMessage in displayLogs) {
                    printLoggerMessage(lMessage);
                }
            }
        }

        private static void printLoggerMessage(LoggerMessage lMessage)
        {
            ConsoleColor prevForeground = Console.ForegroundColor;
            ConsoleColor prevBackground = Console.BackgroundColor;
            Console.ForegroundColor = lMessage.ForegroundColor;
            Console.BackgroundColor = lMessage.BackgroundColor;
            Console.WriteLine(lMessage.Message);
            Console.ForegroundColor = prevForeground;
            Console.BackgroundColor = prevBackground;
        }
    }
}
