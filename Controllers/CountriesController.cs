using HotelListing.Api.Contracts;
using HotelListing.Api.DTOs.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CountriesController(ICountriesService countriesService) : BaseApiController
{
    // GET: api/Countries
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetCountiesDto>>> GetCountries()
    {
        var result = await countriesService.GetCountriesAsync();
        return ToActionResult(result);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var result = await countriesService.GetCountryAsync(id);

        return ToActionResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    {
        var result = await countriesService.UpdateCountryAsync(id, countryDto);
        return ToActionResult(result);
    }

    // POST: api/Countries
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto countryDto)
    {
        var result = await countriesService.CreateCountryAsync(countryDto);
        if (!result.IsSuccess)
        {
            return MapErrorsToResponse(result.Errors);
        }

        return CreatedAtAction(nameof(GetCountry), new { id = result.Value!.Id }, result.Value);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var result = await countriesService.DeleteCountryAsync(id);
        return ToActionResult(result);
    }
}
