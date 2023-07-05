using eCommerceAPI.Business.Orders.Commands.Checkout;
using eCommerceAPI.Business.Orders.Queries.GetAllOrders;
using eCommerceAPI.Business.Orders.Queries.GetOrderById;
using eCommerceAPI.Business.Orders.Queries.GetOrdersByUserId;
using eCommerceAPI.Business.Orders.Queries.GetUserDetailsByOrderId;
using eCommerceAPI.Business.Orders.Queries.GetUsersOrders;
using eCommerceAPI.Data;
using eCommerceAPI.Data.OrderDetails;
using eCommerceAPI.Data.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("orderById")]
        public async Task<List<GetOrderByIdResponse>> GetOrderById([FromQuery] Guid orderId, CancellationToken cancellationToken)
        {
            var usersOrders = await _dbContext.OrderDetails
                .Where(x => x.OrderId == orderId)
                .Include(x => x.ProductItem)
                    .ThenInclude(x => x.Product)
                .Include(x => x.ProductItem)
                    .ThenInclude(x => x.ProductType)
                .Select(x => new GetOrderByIdResponse
                {
                    Id = x.Order.Id,
                    ProductName = x.ProductItem.Product.Name,
                    OrderDate = x.Order.OrderTime,
                    Quantity = x.Quantity,
                    ImageUrl = x.ProductItem.Product.Image_Url,
                    ProductType = x.ProductItem.ProductType.Name,
                    TotalValue = x.ItemsTotalValue
                })
                .ToListAsync(cancellationToken);

            return usersOrders;
        }
        [HttpGet("numberOfOrders")]
        public async Task<int> GetNumberOfOrders(CancellationToken cancellationToken)
        {
            var numberOfOrders = await _dbContext.Orders.CountAsync(cancellationToken);

            return numberOfOrders;
        }
        [HttpGet("totalOrdersValue")]
        public async Task<int> GetOrdersTotalValue(CancellationToken cancellationToken)
        {
            var totalValue = 0;
            var listOfOrders = await _dbContext.OrderDetails
                .Include(x => x.ProductItem)
                .ToListAsync(cancellationToken);
            foreach (var item in listOfOrders)
            {
                totalValue += item.ItemsTotalValue;
            }

            return totalValue;
        }
        [HttpGet("userDetailsByOrderId")]
        public async Task<List<GetUserDetailsByOrderIdResponse>> GetUserDetailByOrderId([FromQuery] Guid orderId, CancellationToken cancellationToken)
        {
            var usersOrders = await _dbContext.Orders.Where(x => x.Id == orderId)
                    .Include(x => x.User)
                        .ThenInclude(x => x.Address)
                .Select(x => new GetUserDetailsByOrderIdResponse
                {
                    UserName = x.User.Name,
                    UserAdressLine = x.User.Address.AddressLine,
                    UserCity = x.User.Address.City,
                    UserCountry = x.User.Address.Country,
                    UserPhone = x.User.Address.PhoneNumber,
                })
                .ToListAsync(cancellationToken);

            return usersOrders;
        }
        [HttpPost("checkout")]
        public async Task<Guid> Checkout([FromBody] CheckoutRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            var cart = await _dbContext.ShoppingCarts
                .Include(x => x.ShoppingCartItems)
                    .ThenInclude(x => x.ProductItem)
                    .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AddressId = user.Address.Id,
                IsShipped = false,
                OrderTime = DateTime.UtcNow,
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
                var productStock=await _dbContext.ProductItems.FirstOrDefaultAsync(x => x.Id == item.ProductItemId,cancellationToken);
                productStock.QtyInStock -= item.Quantity;
                order.OrderTotal += item.Quantity * item.ProductItem.Price;
            }
            order.OrderTotal += 5;

            await _dbContext.AddAsync(order, cancellationToken);
            _dbContext.ShoppingCarts.Remove(cart);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
        [HttpGet("allOrders")]
        public async Task<List<GetAllOrdersResponse>> GetOrders(CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders
                .OrderByDescending(x=>x.OrderTime)
                .Include(x => x.OrderDetails)
                .Include(x => x.User)
                .ThenInclude(x => x.Address)
                .Select(x => new GetAllOrdersResponse
                {
                    Id = x.Id,
                    UserId = x.User.Id,
                    Country = x.User.Address.Country,
                    City = x.User.Address.City,
                    PhoneNumber = x.User.Address.PhoneNumber,
                    OrderDate = x.OrderTime,
                    IsShipped=x.IsShipped,
                    TotalValue = x.OrderTotal
                })
                .ToListAsync(cancellationToken);
            return orders;
        }
        [HttpGet("userOrdersById")]
        public async Task<List<GetOrdersByUserId>> GetOrdersByUserId([FromQuery] Guid userId,CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders
                .OrderByDescending(x => x.OrderTime)
                .Include(x => x.OrderDetails)
                .Include(x => x.User)
                    .Where(x=>x.UserId == userId)
                .Include(x=>x.User)
                .ThenInclude(x => x.Address)
                .Select(x => new GetOrdersByUserId
                {
                    Id = x.Id,
                    OrderDate = x.OrderTime,
                    IsShipped = x.IsShipped,
                    TotalValue = x.OrderTotal
                })
                .ToListAsync(cancellationToken);
            return orders;
        }
        [HttpGet("mostOrderedItem")]
        public async Task<IActionResult> GetMostOrderedProductItem(CancellationToken cancellationToken)
        {
            var orders= await _dbContext.OrderDetails
                .Include(x=>x.ProductItem)
                    .ThenInclude(x=>x.Product)
                .Include(x => x.ProductItem)
                    .ThenInclude(x=>x.ProductType)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var mostOrderedProduct = orders
            .GroupBy(o => o.ProductItemId)
            .OrderByDescending(g => g.Sum(o => o.Quantity))
            .FirstOrDefault()
            .Select(x => $"{x.ProductItem.Product.Brand} {x.ProductItem.Product.Name} {x.ProductItem.ProductType.Name} {x.ProductItem.Size}")
            .Distinct();
            if(mostOrderedProduct == null)
            {
                return NoContent();
            }

            return Ok(mostOrderedProduct);
        }
        [HttpGet("itemsShipped")]
        public async Task<int> GetShippedOrders(CancellationToken cancellationToken)
        {
            var numberOfItems = await _dbContext.Orders.Where(x => x.IsShipped).CountAsync(cancellationToken);
            return numberOfItems;
        }
    }
}
