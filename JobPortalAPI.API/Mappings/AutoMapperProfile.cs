using AutoMapper;
using JobPortalAPI.API.DTOs.Application;
using JobPortalAPI.API.DTOs.Company;
using JobPortalAPI.API.DTOs.Job;
using JobPortalAPI.API.DTOs.User;
using JobPortalAPI.API.Models;

namespace JobPortalAPI.API.Mappings;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    // USER
    CreateMap<User, UserResponseDto>();


    // COMPANY
    CreateMap<Company, CompanyResponseDto>();
    CreateMap<CompanyCreateDto, Company>();


    // JOB
    CreateMap<Job, JobResponseDto>()
        .ForMember(
          dest => dest.CompanyName,
          opt => opt.MapFrom(src => src.Company.Name));

    CreateMap<JobCreateDto, Job>();


    // APPLICATION
    CreateMap<Application, ApplicationResponseDto>()
        .ForMember(
          dest => dest.JobTitle,
          opt => opt.MapFrom(src => src.Job.Title));

    CreateMap<ApplicationCreateDto, Application>();

  }
}
