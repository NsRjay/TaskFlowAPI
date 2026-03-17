using AutoMapper;
using TaskFlowAPI.Models;
using TaskFlowAPI.DTOs;
namespace TaskFlowAPI.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskItem,TaskResponseDTO>();
            CreateMap<TaskCreateDTO,TaskItem>();
            CreateMap<TaskUpdateDTO,TaskItem>();
        }
    }
}