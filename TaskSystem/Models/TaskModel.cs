
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskSystem.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Task Date")]
        public DateTime TaskDate { get; set; }

        public string StrTaskDate { get { return TaskDate.ToShortDateString(); } }

        public int Important { get; set; }

        public int Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public string StrDateCreated { get { return DateCreated.ToShortDateString(); } }

        public int TaskTimeStatus { get {
            if (TaskDate.Date < DateTime.Now.Date)
                return (int)ClassShared.TaskTimeStatus.Past;
            else if (TaskDate.Date == DateTime.Now.Date)
                return (int)ClassShared.TaskTimeStatus.Today;
            else
                return (int)ClassShared.TaskTimeStatus.Future;
        } }

        [Display(Name = "Time Status")]
        public IEnumerable<SelectListItem> TaskTimeStatusDropdown { get; set; }

        public int TaskStatus { get; set; }

        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> TaskStatusDropdown { get; set; }

        public int TaskImportantStatus { get; set; }

        [Display(Name = "Important Status")]
        public IEnumerable<SelectListItem> TaskImportantStatusDropdown { get; set; }

        public int NewTaskUserId { get; set; }

        [Display(Name = "Users")]
        public IEnumerable<SelectListItem> NewTasksUsersDropdown { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class TaskViewModel
    {
        public IEnumerable<TaskModel> Tasks { get; set; }
        public IEnumerable<SelectListItem> TaskStatusDropdown { get; set; }
        public IEnumerable<SelectListItem> TaskTimeStatusDropdown { get; set; }
        public IEnumerable<SelectListItem> TaskImportantStatusDropdown { get; set; }
        public string ErrorMessage { get; set; }
    }
}