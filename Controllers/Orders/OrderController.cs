using eCommerceAPI.Business.Orders.Commands.Checkout;
using eCommerceAPI.Business.Orders.Queries.GetOrderByUserId;
using eCommerceAPI.Data;
using eCommerceAPI.Data.OrderDetails;
using eCommerceAPI.Data.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eCommerceAPI.Controllers.Orders
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly CommerceDbContext _dbContext;

        public OrderController(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("orders/{userId}")]
        public async Task<List<GetOrderByUserIdResponse>> GetAllOrders(Guid userId, CancellationToken cancellationToken)
        {
            var list = _dbContext.Orders
                .Include(x => x.User)
                    .Where(x => x.User.Id == userId)
                .Include(x => x.OrderDetails)
                .Include(x => x.Address)
                .Select(x => new GetOrderByUserIdResponse
                {
                    Id = x.Id,
                    UserName = x.User.Name,
                    AdressName = x.Address.AddressLine,
                    AdressPostalCode = x.Address.PostalCode,
                    OrderTotal = x.OrderTotal
                })
                .ToListAsync(cancellationToken).Result;

            return list;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            var cart = await _dbContext.ShoppingCarts.Include(x => x.ShoppingCartItems).ThenInclude(x => x.ProductItem).FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (cart is null)
            {
                return NotFound();
            }
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AddressId = user.Address.Id
            };
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in cart.ShoppingCartItems)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.Id,
                    ProductItem = item.ProductItem,
                    Quantity = item.Quantity,
                    ItemsTotalValue = item.Quantity * item.ProductItem.Price
                });
                order.OrderTotal += item.Quantity * item.ProductItem.Price;
            }
            //shipping
            order.OrderTotal += 5;

            await _dbContext.AddAsync(order, cancellationToken);
            _dbContext.ShoppingCarts.Remove(cart);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok("order confirmed");
        }
    }
}
