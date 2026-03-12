using TaskFlowAPI.DTOs;
using TaskFlowAPI.Models;
using TaskFlowAPI.Repositories;
using TaskFlowAPI.Helpers;

namespace TaskFlowAPI.Services
{
    
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        
        public PagedResult<TaskResponseDTO> GetAllTasks(int page, int pageSize)
        {
            var result=_taskRepository.GetAllTasks(page,pageSize);
            return new PagedResult<TaskResponseDTO>
            {
                Page=result.Page,
                PageSize=result.PageSize,
                TotalRecords=result.TotalRecords,
                Data=result.Data.Select(t=>new TaskResponseDTO
                {
                    Id=t.Id,
                    Title=t.Title,
                    Description=t.Description,
                    IsCompleted=t.IsCompleted,
                    CreatedAt=t.CreatedAt
                }).ToList()

            };
        }

        public TaskResponseDTO CreateTask(TaskCreateDTO dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false
            };

            var newTask = _taskRepository.AddTask(task);

            return new TaskResponseDTO
            {
                Id = newTask.Id,
                Title = newTask.Title,
                Description = newTask.Description,
                IsCompleted = newTask.IsCompleted,
                CreatedAt = newTask.CreatedAt
            };
        }
    }
}