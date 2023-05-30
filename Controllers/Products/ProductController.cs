using eCommerceAPI.Business.Products.Queries.GetAll;
using eCommerceAPI.Business.Products.Queries.GetById;
using eCommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eCommerceAPI.Controllers.Products
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly CommerceDbContext _dbContext;

        public ProductController(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("productList")]
        public async Task<List<GetProductListResponse>> GetProductList(CancellationToken cancellationToken)
        {
            var list = await _dbContext.Products.AsNoTracking().Include(x => x.ProductItems).Select(x => new GetProductListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProductCategory = x.ProductCategory.Name,
                Size = x.Size,
                Image_Url = x.Image_Url,
                Price = x.ProductItems.Select(x => x.Price).FirstOrDefault(),

            }).ToListAsync(cancellationToken);
            return list;

        }
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductList(int id, CancellationToken cancellationToken)
        {
            var productCheck = await _dbContext.Products.AnyAsync(x => x.Id == id, cancellationToken);

            if (productCheck)
            {
                var product = await _dbContext.Products.AsNoTracking().Select(x => new GetProductByIdResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ProductCategory = x.ProductCategory.Name,
                    Size = x.Size,
                    Image_Url = x.Image_Url,
                    Price = x.ProductItems.Select(x => x.Price).FirstOrDefault()

                }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                return Ok(product);
            }
            return NotFound();
        }


        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
