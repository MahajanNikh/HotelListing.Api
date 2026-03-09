using HotelListing.Api.Constants;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Country;
using HotelListing.Api.DTOs.Hotel;
using HotelListing.Api.Results;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class CountriesService(HotelListingDbContext context) : ICountriesService
{
    public async Task<Result<IEnumerable<GetCountiesDto>>> GetCountriesAsync()
    {
        var countries = await context.Countries
             .Select(c => new GetCountiesDto(
                 c.CountryId,
                 c.Name,
                 c.ShortName))
             .ToListAsync();

        return Result<IEnumerable<GetCountiesDto>>.Success(countries);
    }
    public async Task<Result<GetCountryDto>> GetCountryAsync(int id)
    {
        var country = await context.Countries
              .Where(c => c.CountryId == id)
              .Select(c => new GetCountryDto(
                  c.CountryId,
                  c.Name,
                  c.ShortName,
                  c.Hotels.Select(h => new GetHotelDto(
                      h.Id,
                      h.Name,
                      h.Address,
                      h.Rating,
                      c.CountryId,
                      c.Name))
                  .ToList()
                  )).
                 FirstOrDefaultAsync();

        if (country == null)
        {
            return Result<GetCountryDto>.NotFound();
        }
        return Result<GetCountryDto>.Success(country);
    }
    public async Task<Result> UpdateCountryAsync(int id, UpdateCountryDto countryDto)
    {
        try
        {
            if (id != countryDto.Id)
            {
                return Result.BadRequest(new Error(ErrorCodes.Validation, "Id route value does not match payload Id."));
            }

            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."));
            }

            country.Name = countryDto.Name;
            country.ShortName = countryDto.ShortName;
            context.Countries.Update(country);

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {

            return Result.Failure();
        }




    }
    public async Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto countryDto)
    {
        try
        {
            var exist = await CountryExistsAsync(countryDto.Name);
            if (exist)
            {
                return Result<GetCountryDto>.Failure(new Error(ErrorCodes.Conflict, $"Country with name {countryDto.Name} already exists."));
            }

            var country = new Country
            {
                Name = countryDto.Name,
                ShortName = countryDto.ShortName,
            };
            context.Countries.Add(country);
            await context.SaveChangesAsync();

            var resultDto = new GetCountryDto(
                country.CountryId,
                country.Name,
                country.ShortName,
                []
                );

            return Result<GetCountryDto>.Success(resultDto);
        }
        catch (Exception)
        {

            return Result<GetCountryDto>.Failure();
        }

    }

    public async Task<Result> DeleteCountryAsync(int id)
    {
        try
        {
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return Result.NotFound(new Error(ErrorCodes.Failure, $"Country {id} was not found."));
            }
            context.Countries.Remove(country);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure();
        }

    }

    public async Task<bool> CountryExistsAsync(int id)
    {
        return await context.Countries.AnyAsync(e => e.CountryId == id);
    }
    public async Task<bool> CountryExistsAsync(string name)
    {
        return await context.Countries
            .AnyAsync(e => e.Name.ToLower().Trim() == name.ToLower().Trim());
    }
}
