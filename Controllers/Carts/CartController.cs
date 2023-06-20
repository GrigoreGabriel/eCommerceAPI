using eCommerceAPI.Business.Carts.Commands;
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
                var isCart = await _dbContext.ShoppingCartItems.AnyAsync(x => x.ProductItemId == request.ProductItemId, cancellationToken);
                if (isCart)
                {
                    var item = await _dbContext.ShoppingCartItems.FirstOrDefaultAsync(x => x.ProductItemId == request.ProductItemId, cancellationToken);
                    item.Quantity += request.Quantity;
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Ok("Item already exists in cart, only increased quantity");

                }
                else
                {
                    var newItemEntry = new ShoppingCartItem
                    {
                        ProductItemId = request.ProductItemId,
                        Quantity = request.Quantity != 0 ? request.Quantity : 1,
                        ShoppingCartId = cart.Id
                    };

                    await _dbContext.AddAsync(newItemEntry, cancellationToken);
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
                        Id = x.Id,
                        ProductImageUrl = x.ProductItem.Product.Image_Url,
                        ProductName = $"{x.ProductItem.Product.Brand} {x.ProductItem.Product.Name}",
                        ProductItemType = x.ProductItem.ProductType.Name,
                        Quantity = x.Quantity,
                        TotalItemValue = x.ProductItem.Price * x.Quantity,
                    })
                .ToListAsync(cancellationToken);
            return items;
        }
        [HttpGet("numberOfItemsInCart")]
        public async Task<int> GetProductSizes([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart is null)
            {
                return 0;
            }
            var numberOfItems = await _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == cart.Id).CountAsync(cancellationToken);
            return numberOfItems;

        }
        [HttpDelete("removeProductFromCart")]
        public async Task<IActionResult> RemoveProductFromFavorites([FromQuery] Guid userId, int shoppingCartItemId, CancellationToken cancellationToken)
        {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart is null)
            {
                return NotFound("Cart is empty or does not exist");
            }
            var product = await _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == cart.Id).FirstOrDefaultAsync(x => x.Id == shoppingCartItemId, cancellationToken);
            _dbContext.ShoppingCartItems.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(200);
        }
        [HttpGet("cartTotalValue")]
        public async Task<int> GetCartTotalValue([FromQuery] Guid userId, CancellationToken cancellationToken)
        {

            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart is null)
            {
                return 0;
            }
            var cartItems = await _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == cart.Id).Include(x => x.ProductItem).ToListAsync(cancellationToken);
            var totalValue = 0;
            foreach (var item in cartItems)
            {
                totalValue += item.Quantity * item.ProductItem.Price;
            }
            return totalValue;

        }
    }
}
