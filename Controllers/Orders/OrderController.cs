using eCommerceAPI.Business.Orders.Queries.GetOrderByUserId;
using eCommerceAPI.Data;
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

        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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

        //[HttpGet("order/products/{orderId}")]
        //public async Task<List<GetOrderDetailByOrderIdResponse>> GetProductsFromOrder(int orderId, CancellationToken cancellationToken)
        //{
        //    var list = _dbContext.OrderDetails
        //                .Include(x => x.Order).Where(x => x.Id == orderId)
        //                    .ThenInclude(x => x.Product)
        //                    .Select(x => new GetOrderDetailByOrderIdResponse
        //                    {
        //                        ProductName = x.OrderDetails.Select(x => x.Product.Name).FirstOrDefault(),
        //                        Quantity = x.OrderDetails.Select(x => x.Quantity).FirstOrDefault(),
        //                        Gender = x.OrderDetails.Select(x => x.Product.Gender).FirstOrDefault(),
        //                    })
        //                .ToListAsync(cancellationToken).Result;

        //    return list;
        //}

        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
