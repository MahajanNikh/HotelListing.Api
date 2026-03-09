using HotelListing.Api.DTOs.Hotel;

namespace HotelListing.Api.Contracts
{
    public interface IHotelsService
    {
        Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto);
        Task DeleteHoteAsync(int id);
        Task<IEnumerable<GetHotelDto>> GetHotelsAsync();
        Task<GetHotelDto?> GetHotelByIdAsync(int id);
        Task<bool> HotelExists(int id);
        Task UpdatedHotelAsync(int id, UpdateHotelDto hotelDto);
    }
}