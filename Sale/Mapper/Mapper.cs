using AutoMapper;
using Sale.Dtos;
using Sale.Entities;

namespace Sale.Mapper
{
    public class Mapper : Profile
    {
        public Mapper() 
        {
            CreateMap<Sales, SalesDto>().ReverseMap();
            CreateMap<Sales, CreateSalesDto>().ReverseMap();
        }
    }
}
