using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class HotelsService(HotelListingDbContext context, IMapper mapper) : IHotelsService
{
    public async Task<IEnumerable<GetHotelDto>> GetHotelsAsync()
    {
        var result = await context.Hotels
       .Select(h => new GetHotelDto(
           h.Id,
           h.Name,
           h.Address,
           h.Rating,
           h.CountryId,
           h.Country!.Name
           ))
       .ToListAsync();
        return result;
    }

    public async Task<GetHotelDto?> GetHotelByIdAsync(int id)
    {
        var hotel = await context.Hotels
        .Where(h => h.Id == id)
        .Select(h => new GetHotelDto(
            h.Id,
            h.Name,
            h.Address,
            h.Rating,
            h.CountryId,
            h.Country!.Name
            ))
        .FirstOrDefaultAsync();

        //var hotel = await context.Hotels
        //    .Where(h => h.Id == id)
        //    .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
        //    .FirstOrDefaultAsync();

        return hotel ?? null;
    }

    public async Task UpdatedHotelAsync(int id, UpdateHotelDto hotelDto)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException("Hotel not found");
        }

        hotel.Name = hotelDto.Name;
        hotel.Address = hotelDto.Address;
        hotel.Rating = hotelDto.Rating;
        hotel.CountryId = hotelDto.CountryId;

        await context.SaveChangesAsync();

    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        //var hotel = new Hotel
        //{
        //    Name = hotelDto.Name,
        //    Address = hotelDto.Address,
        //    Rating = hotelDto.Rating,
        //    CountryId = hotelDto.CountryId,
        //};
        var hotel = mapper.Map<Hotel>(hotelDto);
        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();


        var resultDto = await context.Hotels.
            Where(h => h.Id == hotel.Id).
            Select(h => new GetHotelDto(
            h.Id,
            h.Name,
            h.Address,
            h.Rating,
            h.CountryId,
            h.Country!.Name
            )).
            FirstOrDefaultAsync();

        return resultDto;
    }

    public async Task DeleteHoteAsync(int id)
    {
        //var hotel = await context.Hotels.FindAsync(id);
        //if (hotel == null)
        //{
        //    throw new KeyNotFoundException("Hotel not found");
        //}

        //context.Hotels.Remove(hotel);
        //await context.SaveChangesAsync();

        // Better Approch 
        var rows = await context.Hotels.
            Where(c => c.Id == id).
            ExecuteDeleteAsync();

        if (rows == 0)
        {
            throw new KeyNotFoundException("Hotel not found");
        }
    }
    public async Task<bool> HotelExists(int id)
    {
        return await context.Hotels.AnyAsync(e => e.Id == id);
    }

}