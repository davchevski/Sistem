using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassShared
{

    public enum OperationResult
    {
        Error = 0,
        Success = 1,
        AlreadyExist = 2,
        NotExists = 3
    }

    public enum UserRolesInt
    {
        User = 1,
        Admin = 2,
    }

    public enum TaskTimeStatus
    {
        Past = 0,
        Today = 1,
        Future = 2
    }

    public enum TaskStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Complete = 2
    }

    public enum TaskImportantStatus
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class Strings
    {
        public class UserRoles
        {
            public const String User = "User";
            public const String Admin = "Admin";
        }

        public class TaskTimeStatus
        {
            public const String Past = "Past";
            public const String Today = "Today";
            public const String Future = "Future";
        }

        public class TaskStatus
        {
            public const String NotStarted = "Not Started";
            public const String InProgress = "In Progress";
            public const String Complete = "Complete";
        }

        public class TaskImportantStatus
        {
            public const String Low = "Low";
            public const String Medium = "Medium";
            public const String High = "High";
        }
    }
}
