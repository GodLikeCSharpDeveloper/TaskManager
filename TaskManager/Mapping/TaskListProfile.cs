using AutoMapper;
using TaskManager.Entities;
using TaskManager.Models;

namespace TaskManager.Mapping
{
    public class TaskListProfile : Profile
    {
        public TaskListProfile()
        {
            CreateMap<CreateTaskListDto, TaskListModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<TaskListModel, FindTaskListWithSharesDto>()
                .ForMember(dest => dest.SharedUserIds, opt => opt.MapFrom(src => src.Shares.Select(s => s.UserId)));
        }
    }
}