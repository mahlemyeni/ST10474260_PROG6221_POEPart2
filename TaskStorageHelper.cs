using System;
using System.Collections.Generic;
using System.Linq;

namespace Chatbot_WPF
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Load all tasks from the database
        /// </summary>
        public List<TaskItem> LoadTasks()
        {
            try
            {
                return db.Tasks.ToList();
            }
            catch
            {
                return new List<TaskItem>();
            }
        }

        /// <summary>
        /// Add a new task to the database
        /// </summary>
        public void AddTask(string title, string description, string reminder)
        {
            try
            {
                TaskItem task = new TaskItem
                {
                    Title = title,
                    Description = description,
                    Reminder = reminder,
                    IsComplete = false,
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                db.Tasks.Add(task);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error adding task: {ex.Message}");
            }
        }

        /// <summary>
        /// Mark a task as complete
        /// </summary>
        public void MarkAsComplete(int id)
        {
            try
            {
                var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
                if (task != null)
                {
                    task.IsComplete = true;
                    db.Tasks.Update(task);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error marking task complete: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a task from the database
        /// </summary>
        public void DeleteTask(int id)
        {
            try
            {
                var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error deleting task: {ex.Message}");
            }
        }

        /// <summary>
        /// Save a task (used for updates)
        /// </summary>
        public void SaveTask(TaskItem task)
        {
            try
            {
                db.Tasks.Update(task);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error saving task: {ex.Message}");
            }
        }
    }
}