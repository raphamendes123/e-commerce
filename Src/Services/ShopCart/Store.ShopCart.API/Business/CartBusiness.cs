using Core.ApiConfigurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.ShopCart.API.Business.Interfaces;
using Store.ShopCart.API.Domain.Data.Contexts;
using Store.ShopCart.API.Domain.Data.Entitys;

namespace Store.ShopCart.API.Business
{
    public class CartBusiness : ICartBusiness 
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly CartDbContext _context;

        private ICollection<string> _errors = new List<string>();

        public CartBusiness(CartDbContext context, IAspNetUser aspNetUser)
        {
            _context = context;
            _aspNetUser = aspNetUser;
        }

        public async Task<Cart> GetCart()
        {
            return await GetCartByUser() ?? new Cart();
        }

        public async Task<ICollection<string>> AddItem(CartItem item)
        {
            Cart? cart = await GetCartByUser();

            if (cart == null)
                ManageNewCart(item);
            else
                ManageCart(cart, item);

            if (_errors.Any()) return _errors;

            await Persist();
            return _errors;
        }

        public async Task<ICollection<string>> UpdateItem(Guid idProduct, CartItem item)
        {
            Cart? cart = await GetCartByUser();
            var cartItem = await GetValidItem(idProduct, cart, item);
            if (cartItem == null) return _errors;

            cart.UpdateUnit(cartItem, item.Quantity);

            ValidateCart(cart);
            if (_errors.Any()) return _errors;

            _context.CartItems.Update(cartItem);
            _context.Carts.Update(cart);

            await Persist();
            return _errors;
        }

        public async Task<ICollection<string>> RemoveItem(Guid idProduct)
        {
            var cart = await GetCartByUser();

            var item = await GetValidItem(idProduct, cart);
            if (item == null) return _errors;

            ValidateCart(cart);
            if (_errors.Any()) return _errors;

            cart.RemoveItem(item);

            _context.CartItems.Remove(item);
            _context.Carts.Update(cart);

            await Persist();
            return _errors;
        }

        public async Task<ICollection<string>> ApplyVoucher(Voucher voucher)
        {
            var cart = await GetCartByUser();

            cart.ApplyVoucher(voucher);

            _context.Carts.Update(cart);

            await Persist();
            return _errors;
        }


        async Task<Cart> GetCartByUser()
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.IdCustomer == _aspNetUser.GetUserId());
        }

        void ManageNewCart(CartItem item)
        {
            Cart? cart = new Cart(_aspNetUser.GetUserId());
            cart.AddItem(item);

            if(ValidateCart(cart))
            { 
                _context.Carts.Add(cart);
            }
        }

        void ManageCart(Cart cart, CartItem item)
        {
            bool savedItem = cart.HasItem(item);

            cart.AddItem(item);
            ValidateCart(cart);

            if (savedItem)
            {
                _context.CartItems.Update(cart.GetProductById(item.IdProduct));
            }
            else
            {
                _context.CartItems.Add(item);
            }

            _context.Carts.Update(cart);
        }

        async Task<CartItem> GetValidItem(Guid idProduct, Cart cart, CartItem item = null)
        {
            if (item != null && idProduct != item.IdProduct)
            {
                AddError("Current item is not the same sent item");
                return null;
            }

            if (cart == null)
            {
                AddError("Shopping cart not found");
                return null;
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(i => i.IdCart == cart.Id && i.IdProduct == idProduct);

            if (cartItem == null || !cart.HasItem(cartItem))
            {
                AddError("The item is not in cart");
                return null;
            }

            return cartItem;
        }

        async Task Persist()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddError("Error saving data");
        }

        bool ValidateCart(Cart cart)
        {
            if (cart.IsValid()) return true;

            cart.ValidationResult.Errors.ToList().ForEach(e => AddError(e.ErrorMessage));
            return false;
        }

        void AddError(string error)
        {
            _errors.Add(error);
        }         
    }
}
