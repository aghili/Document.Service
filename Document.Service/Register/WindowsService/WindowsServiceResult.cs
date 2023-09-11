namespace Document.Service.Register.WindowsService;

public enum WindowsServiceResult
{
    SUCCESS = 0,
    FAILURE_ACCESS_IS_DENIED = 5,
    FAILURE_DELETED_MARKED = 1072,
    FAILURE_EXIST = 1073,
    FAILURE_NOT_STARTED = 1062,
    FAILURE_INVALID_COMMANDLINE_ARGUMENT = 1639
}