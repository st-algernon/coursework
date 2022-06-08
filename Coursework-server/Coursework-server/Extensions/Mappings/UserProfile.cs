using AutoMapper;
using Coursework_server.Data.Models;
using Coursework_server.Data.ViewModels;

namespace Coursework_server.Extensions.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserVm>();
        }
    }
}
