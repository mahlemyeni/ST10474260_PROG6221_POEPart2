using System;
using System.Collections.Generic;

namespace Chatbot_WPF
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager(ActivityLogger logger = null)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        /// <summary>
        /// Add a new task and log the action
        /// </summary>
        public string AddTask(string title, string description, string reminder)
        {
            try
            {
                _storage.AddTask(title, description, reminder);

                string logMessage = string.IsNullOrEmpty(reminder)
                    ? $"Task added: '{title}'"
                    : $"Task added: '{title}' (Reminder set for {reminder})";

                _logger?.Log(logMessage);

                return $"Task added: '{title}'. Would you like a reminder?";
            }
            catch
            {
                return "Error adding task. Please try again.";
            }
        }

        /// <summary>
        /// Get all tasks from the database
        /// </summary>
        public List<TaskItem> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        /// <summary>
        /// Mark a task as complete and log it
        /// </summary>
        public string MarkAsComplete(int id)
        {
            try
            {
                var tasks = _storage.LoadTasks();
                var task = tasks.Find(t => t.Id == id);

                if (task != null)
                {
                    _storage.MarkAsComplete(id);
                    _logger?.Log($"Task marked complete: '{task.Title}'");
                    return $"Task '{task.Title}' marked as complete!";
                }

                return "Task not found.";
            }
            catch
            {
                return "Error marking task complete.";
            }
        }

        /// <summary>
        /// Delete a task and log it
        /// </summary>
        public string DeleteTask(int id)
        {
            try
            {
                var tasks = _storage.LoadTasks();
                var task = tasks.Find(t => t.Id == id);

                if (task != null)
                {
                    _storage.DeleteTask(id);
                    _logger?.Log($"Task deleted: '{task.Title}'");
                    return $"Task '{task.Title}' deleted.";
                }

                return "Task not found.";
            }
            catch
            {
                return "Error deleting task.";
            }
        }
    }
}