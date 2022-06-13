using AutoMapper;
using Coursework.Core.Data.Models;
using Coursework.Core.Data.ViewModels;

namespace Coursework.Core.Extensions.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserVm>();
    }
}