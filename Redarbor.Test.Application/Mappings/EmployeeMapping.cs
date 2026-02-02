using AutoMapper;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Domain.Entities;

namespace Redarbor.Test.Application.Mappings
{
    public class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<EmployeeDto, Employee>().ReverseMap();
        }
    }
}
