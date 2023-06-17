﻿using eCommerceAPI.Business.Carts.Commands;
using eCommerceAPI.Business.Carts.Queries;
using eCommerceAPI.Data;
using eCommerceAPI.Data.ShoppingCartItems;
using eCommerceAPI.Data.ShoppingCarts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Controllers.Carts
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public readonly CommerceDbContext _dbContext;

        public CartController(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<CartController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CartController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("productToCart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request, CancellationToken cancellationToken)
        {

            var productItem = await _dbContext.ProductItems.FirstOrDefaultAsync(x => x.Id == request.ProductItemId, cancellationToken);
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (cart is null)
            {
                var newShoppingCart = new ShoppingCart
                {
                    UserId = request.UserId,
                };
                newShoppingCart.ShoppingCartItems = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ProductItemId = request.ProductItemId,
                        Quantity=request.Quantity != 0 ? request.Quantity : 1,

                    }
                };
                await _dbContext.AddAsync(newShoppingCart, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Ok("Created cart and added product with specified quantity");
            }
            if (cart is not null)
            {

                if (cart.ShoppingCartItems.Any(x => x.ProductItemId == request.ProductItemId))
                {
                    var item = cart.ShoppingCartItems.FirstOrDefault(x => x.ProductItemId == request.ProductItemId).Quantity += request.Quantity;
                    await _dbContext.AddAsync(cart, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Ok("Item already exists in cart, only increased quantity");

                }
                else
                {
                    cart.ShoppingCartItems.Add(

                        new ShoppingCartItem
                        {
                            ProductItemId = request.ProductItemId,
                            Quantity = request.Quantity != 0 ? request.Quantity : 1,
                        });

                    await _dbContext.AddAsync(cart, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Ok("Cart exists but product does not, added product with specified quantity");
                }

            }
            return Ok(cart);
        }
        [HttpGet("cartContents")]
        public async Task<List<GetCartContentsResponse>> GetProductSizes([FromQuery] GetCartContentsRequest request, CancellationToken cancellationToken)
        {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            if (cart is null)
            {
                return null;
            }
            var items = await _dbContext.ShoppingCartItems
                .Where(x => x.ShoppingCartId == cart.Id)
                .Include(x => x.ProductItem)
                    .ThenInclude(x => x.Product)
                    .Select(x => new GetCartContentsResponse
                    {
                        ProductImageUrl = x.ProductItem.Product.Image_Url,
                        ProductName = $"{x.ProductItem.Product.Brand} {x.ProductItem.Product.Name}",
                        ProductItemType = x.ProductItem.ProductType.Name,
                        Quantity = x.Quantity,
                        TotalItemValue = x.ProductItem.Price * x.Quantity,
                    })
                .ToListAsync(cancellationToken);
            return items;
        }
        // PUT api/<CartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
