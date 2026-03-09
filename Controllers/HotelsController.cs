using HotelListing.Api.Contracts;
using HotelListing.Api.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : ControllerBase
{

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
    {
        var result = await hotelsService.GetHotelsAsync();

        return Ok(result);

    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsService.GetHotelByIdAsync(id);

        if (hotel == null)
        {
            return NotFound();
        }

        return hotel;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest();
        }
        try
        {
            await hotelsService.UpdatedHotelAsync(id, hotelDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

    }

    [HttpPost]
    public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotelDto)
    {
        var hotel = await hotelsService.CreateHotelAsync(hotelDto);

        return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        await hotelsService.DeleteHoteAsync(id);

        return NoContent();
    }


}
