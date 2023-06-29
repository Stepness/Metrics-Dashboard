using AutoMapper;
using MetricsMonitoringServer.Models;

namespace MetricsMonitoringServer.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MetricsDto, DashboardDto>();
    }
}