using HotelListing.Api.DTOs.Hotel;

namespace HotelListing.Api.DTOs.Country;

public record GetCountiesDto( 
    int Id,
    string Name,
    string ShortName
);
