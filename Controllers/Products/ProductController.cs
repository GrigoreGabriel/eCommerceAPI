using eCommerceAPI.Business.Products.Commands.AddProduct;
using eCommerceAPI.Business.Products.Queries.GetAll;
using eCommerceAPI.Business.Products.Queries.GetById;
using eCommerceAPI.Business.Products.Queries.GetProductCategories;
using eCommerceAPI.Business.Products.Queries.GetProductDetails;
using eCommerceAPI.Business.Products.Queries.GetProductStock;
using eCommerceAPI.Business.Products.Queries.GetProductTypes;
using eCommerceAPI.Data;
using eCommerceAPI.Data.Products;
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
                    Image_Url = x.Image_Url,
                    Price = x.ProductItems.Select(x => x.Price).FirstOrDefault()

                }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                return Ok(product);
            }
            return NotFound();
        }
        [HttpGet("productDetails")]
        public async Task<List<GetProductHeaderResponse>> GetProductDetails(CancellationToken cancellationToken)
        {
            var list = await _dbContext.Products.AsNoTracking().Include(x => x.ProductItems).Select(x => new GetProductHeaderResponse
            {
                Id = x.Id,
                Name = x.Name,
                Brand = x.Brand,
                Gender = x.Gender,
                DetailsResponse = x.ProductItems.Select(x => new GetProductDetailResponse
                {
                    Id = x.Id,
                    Size = x.Size,
                    QtyInStock = x.QtyInStock,
                    Price = x.Price,

                }).ToList(),

            }).ToListAsync(cancellationToken);
            return list;

        }

        [HttpGet("itemsInStock")]
        public async Task<List<GetProductStockResponse>> GetProductStock(CancellationToken cancellationToken)
        {
            var list = await _dbContext.ProductItems.AsNoTracking().Include(x => x.Product).Select(x => new GetProductStockResponse
            {
                Brand = x.Product.Brand,
                Name = x.Product.Name,
                QtyInStock = x.QtyInStock,
                Price = x.Price,
                Size = x.Size,
            }).ToListAsync(cancellationToken);
            return list;

        }

        [HttpGet("productCategories")]
        public async Task<List<GetProductCategoriesResponse>> GetProductCategories(CancellationToken cancellationToken)
        {
            var productCategories = await _dbContext.ProductCategories.AsNoTracking().Select(x => new GetProductCategoriesResponse
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync(cancellationToken);
            return productCategories;
        }

        [HttpGet("productTypes")]
        public async Task<List<GetProductTypeResponse>> GetProductTypes(CancellationToken cancellationToken)
        {
            var productTypes = await _dbContext.ProductTypes.AsNoTracking().Select(x => new GetProductTypeResponse
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync(cancellationToken);
            return productTypes;
        }
        [HttpPost("addProduct")]
        public async Task<ActionResult> AddProduct([FromBody] AddProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.ProductCategories.FirstOrDefaultAsync(x => x.Name == request.Category, cancellationToken);
            if (category != null)
            {
                var product = new Product
                {
                    Name = request.Name,
                    Brand = request.Brand,
                    Description = request.Description,
                    Gender = request.Gender,
                    Image_Url = request.Image_Url,
                    ProductCategoryId = category.Id

                };

                await _dbContext.AddAsync(product, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Ok(product);
            }
            return BadRequest();
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
