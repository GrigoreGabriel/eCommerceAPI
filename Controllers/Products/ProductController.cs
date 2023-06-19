using eCommerceAPI.Business.Products.Commands.AddProduct;
using eCommerceAPI.Business.Products.Commands.AddProductItem;
using eCommerceAPI.Business.Products.Queries.GetAll;
using eCommerceAPI.Business.Products.Queries.GetById;
using eCommerceAPI.Business.Products.Queries.GetProductCategories;
using eCommerceAPI.Business.Products.Queries.GetProductDetails;
using eCommerceAPI.Business.Products.Queries.GetProductPrice;
using eCommerceAPI.Business.Products.Queries.GetProductShortDetails;
using eCommerceAPI.Business.Products.Queries.GetProductSizes;
using eCommerceAPI.Business.Products.Queries.GetProductStock;
using eCommerceAPI.Business.Products.Queries.GetProductTypes;
using eCommerceAPI.Data;
using eCommerceAPI.Data.ProductItems;
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

        [HttpGet("productShortDetails")]
        public async Task<List<GetProductShortDetails>> GetProductShortDetails(CancellationToken cancellationToken)
        {
            var list = await _dbContext.Products.AsNoTracking().Include(x => x.ProductItems).Select(x => new GetProductShortDetails
            {
                Id = x.Id,
                Name = $"{x.Brand} {x.Name}",
                Variations = x.ProductItems.Count(),

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
                NoOfConfigs = x.ProductItems.Count,
            }).ToListAsync(cancellationToken);
            return list;

        }

        [HttpGet("itemsInStock")]
        public async Task<List<GetProductStockResponse>> GetProductStock(CancellationToken cancellationToken)
        {
            var list = await _dbContext.ProductItems.AsNoTracking().Include(x => x.Product).Include(x => x.ProductType).Select(x => new GetProductStockResponse
            {
                Brand = x.Product.Brand,
                Name = x.Product.Name,
                Type = x.ProductType.Name,
                QtyInStock = x.QtyInStock,
                Price = x.Price,
                Size = x.Size,
            }).ToListAsync(cancellationToken);
            return list;

        }
        [HttpGet("stockValue")]
        public async Task<int> GetStockValue(CancellationToken cancellationToken)
        {
            var totalValue = 0;
            var items = await _dbContext.ProductItems.AsNoTracking().ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                totalValue += (item.Price * item.QtyInStock);
            }
            return totalValue;

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

        [HttpGet("productConfigurations")]
        public async Task<List<GetProductSizesResponse>> GetProductSizes([FromQuery] int productId, CancellationToken cancellationToken)
        {
            var productSizes = await _dbContext.ProductItems
                .Where(x => x.ProductId == productId)
                .Include(x => x.ProductType)
                .AsNoTracking().Select(x => new GetProductSizesResponse
                {
                    ProductItemId = x.Id,
                    Type = x.ProductType.Name,
                    Size = x.Size,
                    Price = x.Price
                }).ToListAsync(cancellationToken);

            return productSizes;
        }
        [HttpGet("productPrice")]
        public async Task<GetProductPriceResponse> GetProductPrice([FromQuery] GetProductPriceRequest request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.ProductItems
                .Where(x => x.ProductId == request.ProductId)
                .FirstOrDefaultAsync(x => x.ProductType.Name == request.Type && x.Size == request.Size, cancellationToken);
            if (product is not null)
            {
                var response = new GetProductPriceResponse
                {
                    Price = product.Price,
                };
                return response;
            }
            return new GetProductPriceResponse { Price = 0 };
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

        [HttpPost("addProductItem")]
        public async Task<ActionResult> AddProductItem([FromBody] AddProductItemCommand request, CancellationToken cancellationToken)
        {
            var productType = await _dbContext.ProductTypes.FirstOrDefaultAsync(x => x.Name == request.TypeName, cancellationToken);
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.SelectedProductId, cancellationToken);
            if (productType is not null && product is not null)
            {
                var item = new ProductItem
                {
                    QtyInStock = request.QtyInStock,
                    Price = request.Price,
                    Size = request.Size,
                    ProductId = product.Id,
                    ProductTypeId = productType.Id
                };

                await _dbContext.AddAsync(item, cancellationToken);
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