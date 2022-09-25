using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Services.IService;

namespace TangyWeb_Client.Services
{
    public class OrderService :IOrderService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;
        private string BaseServerUrl;
        public OrderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;   
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<OrderDTO> Get(int OrderHeaderId)
        {
            var response = await _httpClient.GetAsync($"/api/order/{OrderHeaderId}");
            var content = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                var order = JsonConvert.DeserializeObject<OrderDTO>(content);
                return order;
            }
            return new OrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string ? userId=null)
        {
             var response = await _httpClient.GetAsync($"/api/order");
             var content =  await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(content);
                return orders;
            }
            return new List<OrderDTO>();
            
        }
    }
}
