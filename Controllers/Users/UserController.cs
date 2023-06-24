using eCommerceAPI.Business.Users.Commands.Create;
using eCommerceAPI.Business.Users.Commands.Update;
using eCommerceAPI.Business.Users.Queries;
using eCommerceAPI.Business.Users.Queries.GetAllUsers;
using eCommerceAPI.Business.Users.Queries.GetUserAddresses;
using eCommerceAPI.Business.Users.Queries.GetUserDetails;
using eCommerceAPI.Business.Users.Queries.GetUserFavorites;
using eCommerceAPI.Business.Users.Queries.LoginUser;
using eCommerceAPI.Data;
using eCommerceAPI.Data.Favorites;
using eCommerceAPI.Data.Products;
using eCommerceAPI.Data.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Controllers.Users
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly CommerceDbContext _dbContext;

        public UserController(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("allUsers")]
        public async Task<List<GetAllUsersResponse>> GetUsers(CancellationToken cancellationToken)
        {
            var list = await _dbContext.Users.AsNoTracking().Select(x => new GetAllUsersResponse
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,

            }).ToListAsync(cancellationToken);
            return list;

        }

        [HttpGet("userDetail")]
        public async Task<GetUserDetailsResponse> GetUserDetails([FromQuery] GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null)
            {
                return null;
            }
            var foundUser = await _dbContext.Users.Where(x => x.Id == request.UserId).Include(x => x.Address).Select(x => new GetUserDetailsResponse
            {
                Name = x.Name,
                Email = x.Email,
                City = x.Address.City,
                Region = x.Address.Region,
                AddressLine = x.Address.AddressLine,
                PostalCode = x.Address.PostalCode,
                PhoneNumber=x.Address.PhoneNumber,
                Country = x.Address.Country

            }).FirstOrDefaultAsync(cancellationToken);
            return foundUser;
        }
        [HttpGet("userList")]
        public async Task<List<GetAllUsersListResponse>> GetAllUsers(CancellationToken cancellationToken)
        {
            var userList = await _dbContext.Users.AsNoTracking()
                .Include(x => x.Address)
                .Select(x => new GetAllUsersListResponse
                {
                    UserId = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    AccountStatus = x.IsAdmin ? "Admin" : "Client",
                    PhoneNumber = x.Address.PhoneNumber ?? "N/A",
                    City = x.Address.City ?? "N/A",
                    Region = x.Address.Region ?? "N/A",
                    Country = x.Address.Country ?? "N/A",
                    AddressLine = x.Address.AddressLine ?? "N/A",
                    PostalCode = x.Address.PostalCode ?? "N/A"

                }).ToListAsync(cancellationToken);
            return userList;
        }
        [HttpPut("update-user-details")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                return NotFound("The user does not exist");
            }
            var foundUserDetails = await _dbContext.Addresses.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            foundUserDetails.PhoneNumber = request.UserPhoneNumber ?? foundUserDetails.PhoneNumber ?? string.Empty;
            foundUserDetails.City = request.City ?? foundUserDetails.City ?? string.Empty;
            foundUserDetails.Country = request.Country ?? foundUserDetails.Country ?? string.Empty;
            foundUserDetails.Region = request.Region ?? foundUserDetails.Region ?? string.Empty;
            foundUserDetails.AddressLine = request.AddressLine ?? foundUserDetails.AddressLine ?? string.Empty;
            foundUserDetails.PostalCode = request.PostalCode ?? foundUserDetails.PostalCode ?? string.Empty;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(200);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsersByUID(Guid id, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }

        [HttpGet("loginUser")]
        public async Task<ActionResult> Login([FromQuery] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Where(x => x.Email == request.Email)
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Id);

        }
        [HttpPost("createUser")]
        public async Task<Guid> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
            };
            var userResponse = new CreateUserResponse
            {
                Id = user.Id

            };
            await _dbContext.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return userResponse.Id;
        }

        [HttpGet("userAddresses/{uid}")]
        public async Task<List<GetUserAddressesQuery>> GetAddressByUserUID(Guid uid, CancellationToken cancellationToken)
        {
            var list = _dbContext.Users.Include(x => x.Address).Where(x => x.Id == uid);

            if (list.Any())
            {
                var query = await _dbContext.Users.Include(x => x.Address).Where(x => x.Id == uid)
                    .Select(x => new GetUserAddressesQuery
                    {
                        City = x.Address.City,
                        Region = x.Address.Region,
                        AddressLine = x.Address.AddressLine,
                        PostalCode = x.Address.PostalCode

                    }).ToListAsync(cancellationToken);
                return query;
            }
            return new List<GetUserAddressesQuery>(0);
        }
        [HttpGet("favorites")]
        public async Task<List<GetUserFavoritesResponse>> GetUserFavorites([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Include(x => x.Favorites).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
            {
                return (List<GetUserFavoritesResponse>)Enumerable.Empty<Product>();
            }
            var favoriteProducts = user.Favorites.Select(item => item.Product).Select(x => new GetUserFavoritesResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Gender = x.Gender,
                Image_Url = x.Image_Url
            }).ToList();
            return favoriteProducts;
        }
        [HttpPost("addFavorite")]
        public async Task<IActionResult> AddUserFavorite([FromQuery] Guid userId, int productId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                return NotFound("User not found");
            }
            var product = await _dbContext.Products.Where(x => x.Id == productId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return NotFound("Product not found");
            }
            if (user.Favorites == null)
            {
                user.Favorites = new List<Favorite>();
            }
            if (user.Favorites.Any(x => x.ProductId == productId))
            {
                return BadRequest("Product already in favorites list");
            }
            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = productId,
            };
            await _dbContext.AddAsync(favorite, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(200);
        }

        [HttpDelete("removeFavorite")]
        public async Task<IActionResult> RemoveProductFromFavorites([FromQuery] Guid userId, int productId, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Users.Where(x => x.Id == userId)
                .Select(product => product.Favorites.FirstOrDefault(x => x.ProductId == productId)).FirstOrDefaultAsync(cancellationToken);
            if (product is null)
            {
                return NotFound();
            }

            _dbContext.Favorites.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(200);
        }
    }
}
