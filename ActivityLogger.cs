using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chatbot_WPF
{
    public class ActivityLogger
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Log an action to the database
        /// </summary>
        public void Log(string action)
        {
            try
            {
                LogEntry log = new LogEntry
                {
                    Description = action,
                    CreatedAt = DateTime.Now.ToString("[HH:mm:ss]")
                };

                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch
            {
                // Silently fail if logging fails
            }
        }

        /// <summary>
        /// Get the last 'count' entries from the log
        /// </summary>
        public string GetRecentLog(int count = 10)
        {
            try
            {
                var logs = db.Logs.ToList().Skip(Math.Max(0, db.Logs.Count() - count)).ToList();

                if (logs.Count == 0)
                    return "No activity logged yet.";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Here's a summary of recent actions:");

                int index = 1;
                foreach (var log in logs)
                {
                    sb.AppendLine($"{index}. {log.Description} {log.CreatedAt}");
                    index++;
                }

                return sb.ToString();
            }
            catch
            {
                return "Error retrieving activity log.";
            }
        }

        /// <summary>
        /// Get all log entries
        /// </summary>
        public string GetFullLog()
        {
            try
            {
                var logs = db.Logs.ToList();

                if (logs.Count == 0)
                    return "No activity logged yet.";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Complete Activity Log:");

                int index = 1;
                foreach (var log in logs)
                {
                    sb.AppendLine($"{index}. {log.Description} {log.CreatedAt}");
                    index++;
                }

                return sb.ToString();
            }
            catch
            {
                return "Error retrieving activity log.";
            }
        }

        /// <summary>
        /// Get the total count of log entries
        /// </summary>
        public int GetCount()
        {
            try
            {
                return db.Logs.Count();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Clear all logs
        /// </summary>
        public void ClearLogs()
        {
            try
            {
                var allLogs = db.Logs.ToList();
                foreach (var log in allLogs)
                {
                    db.Logs.Remove(log);
                }
                db.SaveChanges();
            }
            catch
            {
                // Silently fail
            }
        }
    }
}