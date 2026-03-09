using HotelListing.Api.DTOs.Country;
using HotelListing.Api.Results;

namespace HotelListing.Api.Contracts
{
    public interface ICountriesService
    {
        Task<bool> CountryExistsAsync(int id);
        Task<bool> CountryExistsAsync(string name);
        Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto countryDto);
        Task<Result> DeleteCountryAsync(int id);
        Task<Result<IEnumerable<GetCountiesDto>>> GetCountriesAsync();
        Task<Result<GetCountryDto>> GetCountryAsync(int id);
        Task<Result> UpdateCountryAsync(int id, UpdateCountryDto countryDto);
    }
}