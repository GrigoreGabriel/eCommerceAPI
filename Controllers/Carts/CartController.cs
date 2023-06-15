using eCommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;

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

        //POST api/<CartController>
        //[HttpPost("productToCart")]
        //public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request, CancellationToken cancellationToken)
        //{
        //    var productItem = await _dbContext.ProductItems.Include(x => x.Product).Where(x => x.ProductId == request.ProductId).FirstOrDefaultAsync(cancellationToken);
        //    //var product = await _dbContext.Products.Where(x => x.Id == request.ProductId).Include(x => x.ProductItems).FirstOrDefaultAsync(cancellationToken);
        //    //if (product == null)
        //    //{
        //    //    return NotFound("Product not found");
        //    //}
        //    //var isCart = await _dbContext.ShoppingCarts.AnyAsync(x => x.UserId == request.UserId, cancellationToken);

        //    //if (isCart)
        //    //{
        //    var cart = await _dbContext.ShoppingCarts.Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken)
        //        ?? new ShoppingCart
        //        {
        //            Id = 1,
        //            UserId = request.UserId
        //        };

        //    var shoppingCartItem = new ShoppingCartItem
        //    {
        //        Id = 1,
        //        Quantity = 1,
        //        ProductItemId = productItem.Id,
        //    };

        //    //_dbContext.ShoppingCartItems.Add(shoppingCartItem);
        //    cart.ShoppingCartItems ?? cart.ShoppingCartItems.Add(shoppingCartItem);

        //    await _dbContext.AddAsync(cart, cancellationToken);
        //    await _dbContext.SaveChangesAsync(cancellationToken);

        //    return Ok(cart);
        //}

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
