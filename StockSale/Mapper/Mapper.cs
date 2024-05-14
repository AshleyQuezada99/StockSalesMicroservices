using AutoMapper;
using Stock.Dtos;
using Stock.Entities;

namespace Stock.Mapper
{
    public class Mapper : Profile
    {
        public Mapper() 
        {
            CreateMap<Products, ProductsDto>().ReverseMap();

        }
    }
}
