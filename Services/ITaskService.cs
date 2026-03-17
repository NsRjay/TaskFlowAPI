using TaskFlowAPI.DTOs;
using TaskFlowAPI.Helpers;

namespace TaskFlowAPI.Services
{
    public interface ITaskService
    {
        PagedResult<TaskResponseDTO> GetAllTasks(int page, int pageSize);
        TaskResponseDTO CreateTask(TaskCreateDTO taskDto);
        TaskResponseDTO UpdateTask(int id,TaskUpdateDTO dto);
        void DeleteTask(int id);
    }
}