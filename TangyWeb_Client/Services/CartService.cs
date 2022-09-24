using Blazored.LocalStorage;
using System.Runtime.CompilerServices;
using Tangy_Common;
using TangyWeb_Client.Services.IService;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Services
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;

        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            for (int i = 0; i < cart.Count; i++)
            {
                if(cart.Count == 1 || cart.Count==0)
                {
                    cart.Remove(cart[i]);
                }
                else
                {
                    cart[i].Count -= cartToDecrement.Count;
                }
            }
            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);
        }

        public async Task IncrementCart(ShoppingCart cartToAdd)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
             bool itemInCart = false;
            if(cart == null)
            {
                cart = new List<ShoppingCart>();
            }

            foreach (var obj in cart)
            {
                if(obj.ProductId==cartToAdd.ProductId && obj.ProductPriceId == cartToAdd.ProductPriceId)
                {
                    itemInCart = true;
                    obj.Count += cartToAdd.Count;
                }
            }
            if(!itemInCart)
            {
                cart.Add(new ShoppingCart()
                {
                    ProductId = cartToAdd.ProductId,
                    ProductPriceId = cartToAdd.ProductPriceId,
                    Count = cartToAdd.Count
                });
            }

            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);
        }
    }
}
