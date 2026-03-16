using TaskFlowAPI.DTOs;
using TaskFlowAPI.Models;
using TaskFlowAPI.Repositories;
using TaskFlowAPI.Helpers;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace TaskFlowAPI.Services
{
    
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        private readonly IMemoryCache _cache;

        
        public TaskService(ITaskRepository taskRepository,IMapper mapper,ILogger<TaskService> logger,IMemoryCache cache)
        {
            _taskRepository = taskRepository;
            _mapper=mapper;
            _logger=logger;
            _cache=cache;
        }
        
        public PagedResult<TaskResponseDTO> GetAllTasks(int page, int pageSize)
        {
            var cacheKey=$"tasks_page_{page}_size_{pageSize}";
            _logger.LogInformation("Task cached with key : {CacheKey}",cacheKey);
            if (_cache.TryGetValue(cacheKey,out PagedResult<TaskResponseDTO> cachedTasks))
            {
                _logger.LogInformation("Returning tasks from cache. Page: {Page}",page);
                return cachedTasks;
            }
            _logger.LogInformation(
            "Fetching tasks from database. Page: {Page}, PageSize: {PageSize}",
            page,
            pageSize);
            var result=_taskRepository.GetAllTasks(page,pageSize);
            var mapped=new PagedResult<TaskResponseDTO>
            {
                Page=result.Page,
                PageSize=result.PageSize,
                TotalRecords=result.TotalRecords,
                Data=_mapper.Map<List<TaskResponseDTO>>(result.Data)
            };
            var cacheOptions= new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
            _cache.Set(cacheKey,mapped,cacheOptions);
            return mapped;
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