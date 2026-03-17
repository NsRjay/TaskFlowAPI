using TaskFlowAPI.Helpers;
using TaskFlowAPI.Models;
namespace TaskFlowAPI.Repositories
{
    
    public interface ITaskRepository
    {
       
            PagedResult<TaskItem> GetAllTasks(int page,int pageSize);

            TaskItem AddTask(TaskItem task);
            TaskItem UpdateTask(TaskItem task);
            TaskItem DeleteTask(TaskItem task);
            TaskItem GetTaskById(int id);
        
    }
}