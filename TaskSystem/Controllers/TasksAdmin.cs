using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskSystem.Helpers;
using TaskSystem.Models;
using TaskSystemDL;
using WebMatrix.WebData;

namespace TaskSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TasksAdminController : Controller
    {
        #region Public Methods

        // GET: TasksAdmin/Index
        public ActionResult Index(string message)
        {
            TaskViewModel model = new TaskViewModel();
            model.ErrorMessage = message;
            model.TaskStatusDropdown = DropdownHelper.FillTaskStatus(null);
            model.TaskTimeStatusDropdown = DropdownHelper.FillTaskTimeStatus(null);
            model.TaskImportantStatusDropdown = DropdownHelper.FillTaskImportantStatus(null);
            var tasks = TaskHelper.Instance.GetAllTasks();
            model.Tasks = MapTasksToViewModel(tasks);
            return View(model);
        }

        // GET: /TasksAdmin/GetTasks
        public ActionResult GetTasks(string status, string time, string important)
        {
            int? iStatus = null;
            int? iTime = null;
            int? iImportant = null;

            if (!String.IsNullOrEmpty(status))
            {
                int pomStatus = -1;
                if (int.TryParse(status, out pomStatus))
                {
                    iStatus = pomStatus;
                }
            }

            if (!String.IsNullOrEmpty(time))
            {
                int pomTime = -1;
                if (int.TryParse(time, out pomTime))
                {
                    iTime = pomTime;
                }
            }

            if (!String.IsNullOrEmpty(important))
            {
                int pomImportant = -1;
                if (int.TryParse(important, out pomImportant))
                {
                    iImportant = pomImportant;
                }
            }

            var tasks = TaskHelper.Instance.GetTasksBySearch(WebSecurity.GetUserId(User.Identity.Name), iStatus, iTime, iImportant);
            var model = MapTasksToViewModel(tasks);
            return PartialView("_TasksAdmin", model);
        }

        // GET: /TasksAdmin/Edit/2
        [HttpGet]
        public ActionResult Edit(int id)
        {
            TaskModel model = new TaskModel();
            var singleTask = TaskHelper.Instance.GetTaskById(id);
            if (singleTask != null)
                model = MapTaskToModel(singleTask);
            else
                return RedirectToAction("Index", new { message = ClassShared.OperationResult.Error });
            return View(model);
        }

        // POST: /TasksAdmin/Edit
        [HttpPost]
        public ActionResult Edit(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = TaskHelper.Instance.UpdateTask(MapModelToClass(model));
                if (result)
                    model.ErrorMessage = ClassShared.OperationResult.Success.ToString();
                else
                    model.ErrorMessage = ClassShared.OperationResult.Error.ToString();
            }
            else
            {
                model.ErrorMessage = ClassShared.OperationResult.Error.ToString();
            }
            model.TaskStatusDropdown = DropdownHelper.FillTaskStatus(model.TaskStatus);
            model.TaskTimeStatusDropdown = DropdownHelper.FillTaskTimeStatus(model.TaskTimeStatus);
            model.TaskImportantStatusDropdown = DropdownHelper.FillTaskImportantStatus(model.TaskImportantStatus);
            return View(model);
        }

        // POST: /TasksAdmin/Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (TaskHelper.Instance.DeleteTask(id))
                return RedirectToAction("Index", new { message = ClassShared.OperationResult.Success });
            else
                return RedirectToAction("Index", new { message = ClassShared.OperationResult.Success });
        }

        // GET: /TasksAdmin/New
        [HttpGet]
        public ActionResult New()
        {
            TaskModel model = new TaskModel();
            model.TaskDate = DateTime.Now;
            model.TaskStatusDropdown = DropdownHelper.FillTaskStatus(null);
            model.TaskTimeStatusDropdown = DropdownHelper.FillTaskTimeStatus(null);
            model.TaskImportantStatusDropdown = DropdownHelper.FillTaskImportantStatus(null);
            var users = Roles.GetUsersInRole(ClassShared.Strings.UserRoles.User);
            model.NewTasksUsersDropdown = DropdownHelper.FillUsers(users);
            return View(model);
        }

        // POST: /Admin/Event/New/model
        [HttpPost]
        public ActionResult New(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                Task newTask = MapNewModelToClass(model);
                bool result = TaskHelper.Instance.AddTask(newTask);
                if (true)
                    return RedirectToAction("Index", new { message = ClassShared.OperationResult.Success });
            }

            // If we got this far, something failed, redisplay form
            model.ErrorMessage = ClassShared.OperationResult.Success.ToString();
            model.TaskStatusDropdown = DropdownHelper.FillTaskStatus(null);
            model.TaskTimeStatusDropdown = DropdownHelper.FillTaskTimeStatus(null);
            model.TaskImportantStatusDropdown = DropdownHelper.FillTaskImportantStatus(null);
            var users = Roles.GetUsersInRole(ClassShared.Strings.UserRoles.User);
            model.NewTasksUsersDropdown = DropdownHelper.FillUsers(users);
            return View(model);
        }

        #endregion

        #region Private Methods

        private IEnumerable<TaskModel> MapTasksToViewModel(List<Task> tasks)
        {
            if (tasks == null || !tasks.Any())
                return null;

            return tasks
                .Select(singleTask => new TaskModel
                {
                    Id = singleTask.Id,
                    Title = singleTask.Title,
                    Description = singleTask.Description,
                    TaskDate = singleTask.TaskDate,
                    Important = singleTask.Important,
                    Status = singleTask.Status,
                    UserId = singleTask.UserId,
                    DateCreated = singleTask.DateCreated
                })
                .AsEnumerable();
        }

        private TaskModel MapTaskToModel(Task task)
        {
            return new TaskModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                TaskDate = task.TaskDate,
                Important = task.Important,
                Status = task.Status,
                UserId = task.UserId,
                TaskTimeStatusDropdown = DropdownHelper.FillTaskTimeStatus(GetTaskTimeStatus(task.TaskDate)),
                TaskImportantStatus = task.Important,
                TaskImportantStatusDropdown = DropdownHelper.FillTaskImportantStatus(task.Important),
                TaskStatus = task.Status,
                TaskStatusDropdown = DropdownHelper.FillTaskStatus(task.Status),
                DateCreated = task.DateCreated
            };
        }

        private Task MapModelToClass(TaskModel task)
        {
            return new Task()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                TaskDate = task.TaskDate,
                Important = task.TaskImportantStatus,
                Status = task.TaskStatus,
                UserId = task.UserId,
                DateCreated = task.DateCreated
            };
        }

        private Task MapNewModelToClass(TaskModel task)
        {
            return new Task()
            {
                Title = task.Title,
                Description = task.Description,
                TaskDate = task.TaskDate,
                Important = task.TaskImportantStatus,
                Status = task.TaskStatus,
                UserId = task.NewTaskUserId,
                DateCreated = DateTime.Now
            };
        }

        private int? GetTaskTimeStatus(DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
                return (int)ClassShared.TaskTimeStatus.Past;
            else if (date.Date == DateTime.Now.Date)
                return (int)ClassShared.TaskTimeStatus.Today;
            else
                return (int)ClassShared.TaskTimeStatus.Future;
        }

        #endregion
    }
}
