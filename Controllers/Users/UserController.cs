using eCommerceAPI.Business.Users.Commands.Create;
using eCommerceAPI.Business.Users.Commands.Update;
using eCommerceAPI.Business.Users.Queries;
using eCommerceAPI.Business.Users.Queries.GetUserAddresses;
using eCommerceAPI.Data;
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

        [HttpPost("createUser")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
            };
            await _dbContext.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(200);
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
        [HttpPut("updateUserInfo/{id}")]
        public async Task<ActionResult> UpdateUserInfo(Guid id, UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(x => x.Id == id).AnyAsync(cancellationToken);
            if (user)
            {
                var userInfo = await _dbContext.Users.Where(x => x.Id == id).Include(x => x.Address).FirstOrDefaultAsync(cancellationToken);
                userInfo.Name = request.Name;
                userInfo.Email = request.Email;
                userInfo.Address.City = request.City;
                userInfo.Address.Region = request.Region;
                userInfo.Address.AddressLine = request.AddressLine;
                userInfo.Address.PostalCode = request.PostalCode;

            }

            //_mapper.Map<UpdateUserInfoRequest>(userInfo);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(200);
        }
    }
}
