using UnityEngine;

using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

namespace QuestSLAM.Utils
{   
    public class QueuedLogger
    {

        /// <summary>
        /// Defines the supported log levels for the logger
        /// </summary>
        public enum Levels
        {
            /// <summary>Informational messages</summary>
            INFO,

            /// <summary>Warning messages</summary>
            WARNING,

            /// <summary>Error messages</summary>
            ERROR
        }

        public class LogEntry
        {
            /// <summary>log message content</summary>
            public string Message { get; private set; }
    
            /// <summary>
            /// Unix timestamp in milliseconds when log was created
            /// </summary>
            public long Timestamp { get; set; }
    
            /// <summary>
            /// The filename where the log was called from
            /// </summary>
            public string CallingFileName { get; private set; }
    
            /// <summary>
            /// Number of times this identical message was logged
            /// </summary>
            public int Count { get; set; }
    
    
            /// <summary>
            /// The log level of this entry
            /// </summary>
            public Levels Level { get; private set; }
    
            /// <summary>
            /// Associated exception if this is an exception log
            /// </summary>
            public Exception Exception { get; private set; }
    
            /// <summary>
            /// Creates a new log entry
            /// </summary>
            /// <param name="message">The log message</param>
            /// <param name="level">The log level</param>
            /// <param name="callingFileName">The filename where the log was called from</param>
            /// <param name="exception">Optional exception associated with this log entry</param>
            public LogEntry(string message, Levels level, [CallerFilePath] string callingFileName = "", Exception exception = null)
            {
                Message = message;
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Level = level;
                CallingFileName = callingFileName;
                Exception = exception;
                Count = 1;
            }
    
            public override string ToString()
            {
                string prefix = string.IsNullOrEmpty(CallingFileName)
                    ? ""
                    : $"[{CallingFileName}] ";
                string messageWithPrefix = $"{prefix}{Message}";
                return Count > 1
                    ? $"{messageWithPrefix} (repeated {Count} times)"
                    : messageWithPrefix;
            }
        }

        /// <summary>
        /// Thread context to always log in main thread
        /// </summary>
        private static SynchronizationContext mainThreadContext;

        /// <summary>
        /// Queue to hold log entries before they are flushed
        /// </summary>
        private static readonly Queue<LogEntry> logQueue = new Queue<LogEntry>();

        /// <summary>
        /// Lock object for thread-safe access to logQueue and lastEntry
        /// </summary>
        private static readonly object logLock = new object();

        /// <summary>
        /// Cache the last entry to support deduplication of identical messages.
        /// Access must be synchronized with logLock.
        /// </summary>
        private static LogEntry lastEntry = null;

        #region Public Methods
        public void Init()
        {
            mainThreadContext = SynchronizationContext.Current;
        }

        public static void Log(string message, Levels level = Levels.INFO, [CallerFilePath] string callerFilePath = "")
        {
            string callingFileName = GetFileNameFromPath(callerFilePath);

            lock (logLock)
            {
                if (
                    lastEntry != null
                    && lastEntry.Message == message
                    && lastEntry.Level == level
                    && lastEntry.CallingFileName == callingFileName
                    && lastEntry.Exception == null
                )
                {
                    lastEntry.Count++;
                }
                else
                {
                    lastEntry = new LogEntry(message, level, callingFileName);
                    logQueue.Enqueue(lastEntry);

                    
                    while (logQueue.Count > 1000)
                    {
                        logQueue.Dequeue();
                    }
                }
            }
        }

        public static void LogWarning(string message, [CallerFilePath] string callerFilePath = "")
        {
            Log(message, Levels.WARNING, callerFilePath);
        }

        public static void LogError(string message, [CallerFilePath] string callerFilePath = "")
        {
            Log(message, Levels.ERROR, callerFilePath);
        }

        public static void LogException(Exception exception, [CallerFilePath] string callerFilePath = "")
        {
            LogException(exception.Message, exception, callerFilePath);
        }

        public static void LogException(string message, Exception exception, [CallerFilePath] string callerFilePath = "")
        {
            string callingFileName = GetFileNameFromPath(callerFilePath);

            lock (logLock)
            {
                if (
                    lastEntry != null
                    && lastEntry.Level == Levels.ERROR
                    && lastEntry.Exception != null
                    && lastEntry.Message == message
                    && lastEntry.CallingFileName == callingFileName
                )
                {
                    lastEntry.Count++;
                }
                else
                {
                    lastEntry = new LogEntry(message, Levels.ERROR, callingFileName, exception);
                    logQueue.Enqueue(lastEntry);

                    // Maintain circular buffer - keep only the most recent logs
                    while (logQueue.Count > 1000)
                    {
                        logQueue.Dequeue();
                    }
                }
            }
        }

        public static void Flush()
        {
            // Copy entries under lock, then log outside lock to avoid holding lock during Debug calls
            List<LogEntry> entriesToFlush;

            lock (logLock)
            {
                entriesToFlush = new List<LogEntry>(logQueue);
                logQueue.Clear();
                lastEntry = null;
            }

            // Only flush on main thread
            invokeOnMainThread(() =>
            {
                foreach (LogEntry entry in entriesToFlush)
                {
                    switch (entry.Level)
                    {
                        case Levels.WARNING:
                            Debug.LogWarning(entry.ToString());
                            break;
                        case Levels.ERROR:
                            if (entry.Exception != null)
                            {
                                // Log the custom message with prefix and count
                                Debug.LogError(entry.ToString());
                                Debug.LogException(entry.Exception);
                            }
                            else
                            {
                                Debug.LogError(entry.ToString());
                            }
                            break;
                        default:
                            Debug.Log(entry.ToString());
                            break;
                    }
                }
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Extracts the filename from a full file path for cleaner log output.
        /// </summary>
        /// <param name="filePath">The full file path</param>
        /// <returns>Just the filename portion of the path</returns>
        private static string GetFileNameFromPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return "";

            // Windows paths compiled with [CallerFilePath] use backslashes,
            // but Path.GetFileName on Unix doesn't recognize \ as a separator
            string normalizedPath = filePath.Replace('\\', '/');
            return Path.GetFileName(normalizedPath);
        }

        private static void invokeOnMainThread(Action action)
        {
            if (mainThreadContext == null)
            {
                action();
            }
            else
            {
                mainThreadContext.Post(_ => action(), null);
            }
        }
        #endregion
    }
}