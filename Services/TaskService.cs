using TaskFlowAPI.DTOs;
using TaskFlowAPI.Models;
using TaskFlowAPI.Repositories;
using TaskFlowAPI.Helpers;
using AutoMapper;

namespace TaskFlowAPI.Services
{
    
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;

        
        public TaskService(ITaskRepository taskRepository,IMapper mapper,ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _mapper=mapper;
            _logger=logger;
        }
        
        public PagedResult<TaskResponseDTO> GetAllTasks(int page, int pageSize)
        {
            _logger.LogInformation(
            "Fetching tasks from database. Page: {Page}, PageSize: {PageSize}",
            page,
            pageSize);
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
            _logger.LogInformation("Create task request received");
            /*var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false
            };*/
            var task =_mapper.Map<TaskItem>(dto);


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