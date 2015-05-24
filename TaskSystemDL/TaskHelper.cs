using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassShared;
using System.Transactions;

namespace TaskSystemDL
{
    public class TaskHelper
    {

        #region Instance

        private static TaskHelper instance;

        private TaskHelper() { }

        public static TaskHelper Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new TaskHelper();
                }
                return instance;
            }
        }

        #endregion

        #region Get Methods

        public List<UserProfile> GetUsers(string[] strUsers)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    return data.UserProfiles.Where(u => strUsers.Contains(u.UserName)).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Task> GetAllTasks()
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    return data.Tasks.OrderByDescending(t=>t.DateCreated).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Task> GetTasksForUserId(int userId)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    return data.Tasks.Where(t=>t.UserId == userId).OrderByDescending(t => t.DateCreated).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Task> GetTasksBySearch(int userId, int? iStatus, int? iTime, int? iImportant)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    var tasks = data.Tasks.Where(t=>t.UserId == userId).OrderByDescending(t => t.DateCreated).ToList();

                        if (iStatus.HasValue)
                        {
                            tasks = tasks.Where(k => k.Status == iStatus.Value).ToList();
                        }
                        if (iImportant.HasValue)
                        {
                            tasks = tasks.Where(k => k.Status == iImportant.Value).ToList();
                        }
                        if (iTime.HasValue)
                        {
                            switch (iTime.Value)
                            {
                                case (int)ClassShared.TaskTimeStatus.Past:
                                    {
                                        tasks = tasks.Where(k => k.TaskDate.Date < DateTime.Now.Date ).ToList();
                                        break;
                                    }
                                case (int)ClassShared.TaskTimeStatus.Today:
                                    {
                                        tasks = tasks.Where(k => k.TaskDate.Date == DateTime.Now.Date).ToList();
                                        break;
                                    }
                                case (int)ClassShared.TaskTimeStatus.Future:
                                    {
                                        tasks = tasks.Where(k => k.TaskDate.Date > DateTime.Now.Date).ToList();
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        return tasks;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task GetTaskById(int id)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    return data.Tasks.SingleOrDefault(t => t.Id == id);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region Update Methods

        public bool UpdateTaskForStatus(int taskId, int taskStatus)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    var item = data.Tasks.SingleOrDefault(e => e.Id == taskId);
                    if (item != null)
                    {
                        item.Status = taskStatus;
                        data.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public bool UpdateTask(Task task)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    var item = data.Tasks.SingleOrDefault(e => e.Id == task.Id);
                    if (item != null)
                    {
                        item.Title = task.Title;
                        item.Description = task.Description;
                        item.TaskDate = task.TaskDate;
                        item.Important = task.Important;
                        item.Status = task.Status;
                        data.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        #endregion

        #region Delete Methods

        public bool DeleteTask(int id)
        {
                using (var data = new TaskSystemEntities())
                {
                    try
                    {
                        var item = data.Tasks.SingleOrDefault(e => e.Id == id);
                        if (item != null)
                        {
                            data.Tasks.Remove(item);
                            data.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                       return false;
                    }
                    return true;
                }
        }

        #endregion

        #region Insert Methods

        public bool AddTask(Task newTask)
        {
            try
            {
                using (var data = new TaskSystemEntities())
                {
                    data.Tasks.Add(newTask);
                    data.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion

    }
}
