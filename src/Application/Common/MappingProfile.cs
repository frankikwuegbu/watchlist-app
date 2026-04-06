using Application.Movies;
using Application.Watchlists;
using AutoMapper;
using Domain.Entities;

namespace Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MoviesDto>();
    }
}
