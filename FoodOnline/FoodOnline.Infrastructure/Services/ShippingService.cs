using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.IService;
using FoodOnline.Infrastructure.IService;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoodOnline.Infrastructure.Services
{
    public class ShippingService : IShippingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IBranchService _branchService;
        private readonly string _apiFeeUrl = "https://services.giaohangtietkiem.vn/services/shipment/fee";

        public ShippingService(IBranchService branchService, IConfiguration configuration)
        {
            _branchService = branchService ?? throw new ArgumentNullException(nameof(branchService));
            _httpClient = new HttpClient();
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var apiKey = _configuration["APIKey:GHTKApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("Token", apiKey);
            }
            else
            {
                throw new InvalidOperationException("API Key for GHTK is missing in the configuration.");
            }
        }

        public async Task<FeeDTO> GetShippingFeeAsync(UserAddressDetailGetDTO userAddress)
        {
            var branches = await _branchService.GetAllBranch();
            if (branches == null || !branches.Any())
            {
                return null; // Return early if there are no branches
            }

            var tasks = branches.Select(branch => GetShippingFeeForBranchAsync(branch, userAddress)).ToArray();
            var feeDTOs = await Task.WhenAll(tasks);

            // Return the highest fee, or null if the list is empty
            return feeDTOs.OrderBy(f => f?.Fee).FirstOrDefault();


        }

        private async Task<FeeDTO> GetShippingFeeForBranchAsync(BranchDetailGetDTO branch, UserAddressDetailGetDTO userAddress)
        {


            var requestUrl = $"{_apiFeeUrl}?weight=1000&deliver_option=xteam&address={userAddress.Detail}&province={userAddress.City}&district={userAddress.District}&pick_province={branch.City}&pick_district={branch.District}&tags%5B%5D=1&pick_address={userAddress.Detail}";


            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await _httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var feeData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var feeResponse = JsonSerializer.Deserialize<ResponseShippingFeeDTO>(feeData, options);
                feeResponse.Fee.BranchId = branch.Id;
                return feeResponse?.Success == true ? feeResponse.Fee : null;
            }

            // Log or handle the error response here as needed
            return null;
        }
    }
}
