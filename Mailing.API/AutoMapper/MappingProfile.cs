using AutoMapper;
using Mailing.API.Dto;
using Mailing.API.Models;

namespace Mailing.API.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SendMailDto, Letter>();
        CreateMap<SendMailDto, Letter>().ReverseMap();
    }
}