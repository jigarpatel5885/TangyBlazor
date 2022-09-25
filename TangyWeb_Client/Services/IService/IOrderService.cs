using Tangy_Models;

namespace TangyWeb_Client.Services.IService
{
    public interface IOrderService  
    {
        public Task<IEnumerable<OrderDTO>> GetAll(string? userId = null);
        public Task<OrderDTO> Get(int OrderHeaderId);
    }
}
