using AutoMapper;
using BackendApi.DTOs.Product;
using BackendApi.Models;

namespace BackendApi.Mapping
{
    /// <summary>
    /// AutoMapper profile for Product entity mappings
    /// </summary>
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            ConfigureProductMappings();
        }

        private void ConfigureProductMappings()
        {
            // Product Entity to DTOs
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Product, ProductListDto>();

            // DTOs to Product Entity
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Reverse mappings for editing scenarios
            CreateMap<Product, ProductCreateDto>();
            CreateMap<Product, ProductUpdateDto>();
        }
    }
}
