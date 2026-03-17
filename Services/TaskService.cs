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
        private static readonly List<string> _taskCacheKeys=new();

        private void ClearTaskCache()
        {
            foreach(var key in _taskCacheKeys)
            {
                _cache.Remove(key);
            }
            _taskCacheKeys.Clear();
            _logger.LogInformation("All task cache cleared");

        }
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

            _logger.LogInformation("Checking cache value....");
            if (_cache.TryGetValue(cacheKey,out PagedResult<TaskResponseDTO> cachedTasks))
            {
                _logger.LogInformation("Returning tasks from cache. Page: {Page}",page);
                return cachedTasks;
            }
            _logger.LogInformation("Cache miss: {CacheKey}",cacheKey);
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
            if (!_taskCacheKeys.Contains(cacheKey))
            {
                _taskCacheKeys.Add(cacheKey);
            }
            _logger.LogInformation("CACHE SET : {CacheKey}",cacheKey);
            _logger.LogInformation("Tasks cached with key: {CacheKey}",cacheKey);
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
            var created = _taskRepository.AddTask(task);
            ClearTaskCache();
            return _mapper.Map<TaskResponseDTO>(created);

        }
        public TaskResponseDTO UpdateTask(int id,TaskUpdateDTO dto)
        {   
            _logger.LogInformation("Updating task with ID: {Id}",id);
            var existingTask=_taskRepository.GetTaskById(id);
            if (existingTask==null)
                throw new Exception("Task not found");
            _mapper.Map(dto,existingTask);

            var updated=_taskRepository.UpdateTask(existingTask);
            ClearTaskCache();
            return _mapper.Map<TaskResponseDTO>(updated);

        }
        public void DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task with ID : {Id}",id);
            var existingTask=_taskRepository.GetTaskById(id);
            if (existingTask == null)
            {
                throw new Exception("Task not found");
            }
            _taskRepository.DeleteTask(existingTask);
            ClearTaskCache();

        }
        
    }
}