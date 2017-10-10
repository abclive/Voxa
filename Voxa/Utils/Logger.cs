using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        const int UPDATE_SKIP_FRAMES = 30;
        private static int skippedFrames = 0;

        private static Dictionary<string, LoggerMessage> stickyLogs = new Dictionary<string, LoggerMessage>();
        private static List<LoggerMessage> logs = new List<LoggerMessage>();

        public static void AddStickyInfo(string identifier)
        {
            stickyLogs.Add(identifier, new LoggerMessage(""));
            UpdateConsole();
        }

        public static void AddStickyInfo(string identifier, string message)
        {
            stickyLogs.Add(identifier, new LoggerMessage(message));
            UpdateConsole();
        }

        public static void AddStickyInfo(string identifier, LoggerMessage lMessage)
        {
            stickyLogs.Add(identifier, lMessage);
            UpdateConsole();
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
            UpdateConsole();
        }

        public static void Info(string message)
        {
            logs.Add(new LoggerMessage(message));
            UpdateConsole();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Warning(string message)
        {
            Type callerType = new StackTrace(1, false).GetFrame(1).GetMethod().DeclaringType;
            logs.Add(new LoggerMessage($"[{callerType.Name}::Warn]: {message}", ConsoleColor.Yellow));
            UpdateConsole();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Error(string message)
        {
            Type callerType = new StackTrace(1, false).GetFrame(1).GetMethod().DeclaringType;
            logs.Add(new LoggerMessage($"[{callerType.Name}::Error]: {message}", ConsoleColor.Red));
            UpdateConsole();
        }

        public static void UpdateStickyDisplay()
        {
            skippedFrames++;
            if (skippedFrames >= UPDATE_SKIP_FRAMES)
            {
                skippedFrames = 0;
                Console.SetCursorPosition(0, 0);
                int i = 0;
                foreach (KeyValuePair<string, LoggerMessage> stickyLog in stickyLogs)
                {
                    Console.WriteLine(new String(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, i++);
                    printLoggerMessage(stickyLog.Value);
                }
            }
        }

        public static void UpdateConsole()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            foreach (KeyValuePair<string, LoggerMessage> stickyLog in stickyLogs) {
                printLoggerMessage(stickyLog.Value);
            }
            Console.WriteLine(new String('=', Console.WindowWidth));
            int startLog = Console.CursorTop;
            int maxLogs = Console.WindowHeight - startLog - 1;
            List<LoggerMessage> displayLogs = logs.Skip((logs.Count - maxLogs < 0) ? 0 : logs.Count - maxLogs).ToList();
            foreach (LoggerMessage lMessage in displayLogs)
            {
                printLoggerMessage(lMessage);
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
