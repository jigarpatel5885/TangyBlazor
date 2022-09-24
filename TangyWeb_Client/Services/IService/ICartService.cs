using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Services.IService
{
    public interface ICartService
    {
        public event Action OnChange;
        public Task IncrementCart(ShoppingCart shoppingCart);

        public Task DecrementCart(ShoppingCart shoppingCart);
    }
}
