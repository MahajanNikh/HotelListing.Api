using HotelListing.Api.Contracts;

namespace HotelListing.Api.Services;

public class ApiKeyValidatorService(IConfiguration configuration) : IApiKeyValidatorService
{
    public async Task<bool> IsValidAsync(string apiKey, CancellationToken ct = default)
    {
        return await Task.FromResult(apiKey.Equals(configuration["ApiKey"]));
    }
}
 