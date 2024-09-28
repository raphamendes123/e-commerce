using FluentValidation;
using FluentValidation.Results; 

namespace Store.ShopCart.API.Domain.Data.Entitys
{
    public class Cart
    {
        internal const int MAX_ITEMS = 5;

        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public decimal Total { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();       
 
        public ValidationResult ValidationResult { get; set; }

        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; } = 0;

        public Voucher Voucher { get; set; }  

        public Cart(Guid idCustomer)
        {
            Id = Guid.NewGuid();
            IdCustomer = idCustomer;
        }

        public Cart() { }

        public void ApplyVoucher(Voucher voucher)
        {
            if(voucher is not null)
            { 
                Voucher = voucher;
                HasVoucher = true;
            }
            else
            {
                Voucher = new Voucher(); 
            }
            CalculateTotalPrice();
        }

        internal void CalculateTotalPrice()
        {
            Total = Items.Sum(p => p.CalculatePrice());
            CalculateDiscountPrice();
        }

        private void CalculateDiscountPrice()
        {
            if (!HasVoucher) return;

            decimal discount = 0;
            var price = Total;

            if (Voucher.DiscountType == DiscountType.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (price * Voucher.Percentage.Value) / 100;
                    price -= discount;
                }
            }
            else
            {
                if (Voucher.Discount.HasValue)
                {
                    discount = Voucher.Discount.Value;
                    price -= discount;
                }
            }

            Total = price < 0 ? 0 : price;
            Discount = discount;
        }

        internal bool HasItem(CartItem item)
        {
            return Items.Any(p => p.IdProduct == item.IdProduct);
        }

        internal CartItem GetProductById(Guid productId)
        {
            return Items.FirstOrDefault(p => p.IdProduct == productId);
        }

        internal void AddItem(CartItem item)
        {
            item.SetIdCart(Id);

            if (HasItem(item))
            {
                var itemRef = GetProductById(item.IdProduct);
                itemRef.AddUnit(item.Quantity);

                item = itemRef;
                Items.Remove(itemRef);
            }

            Items.Add(item);
            CalculateTotalPrice();
        }

        internal void UpdateItem(CartItem item)
        {
            item.SetIdCart(Id);

            var itemExistente = GetProductById(item.IdProduct);

            Items.Remove(itemExistente);
            Items.Add(item);

            CalculateTotalPrice();
        }

        internal void UpdateUnit(CartItem item, int unities)
        {
            item.UpdateUnit(unities);
            UpdateItem(item);
        }

        internal void RemoveItem(CartItem item)
        {
            Items.Remove(GetProductById(item.IdProduct));
            CalculateTotalPrice();
        }

        internal bool IsValid()
        {
            List<ValidationFailure>? errors = Items.SelectMany(i => new CartItem.ShoppingCartItemValidation().Validate(i).Errors).ToList();
            errors.AddRange(new CartValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }

        public class CartValidation : AbstractValidator<Cart>
        {
            public CartValidation()
            {
                RuleFor(c => c.IdCustomer)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Customer not found");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("The shopping cart does not have any items");

                RuleFor(c => c.Total)
                    .GreaterThan(0)
                    .WithMessage("The shopping cart total amount should be greater than 0");
            }
        }
    }
}


