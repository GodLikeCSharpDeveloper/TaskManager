using AutoMapper;
using TaskManager.Entities;
using TaskManager.Models;

namespace TaskManager.Mapping
{
    public class TaskListShareProfile : Profile
    {
        public TaskListShareProfile()
        {
            CreateMap<CreateTaskListShareDto, TaskListShareModel>();
        }
    }
}