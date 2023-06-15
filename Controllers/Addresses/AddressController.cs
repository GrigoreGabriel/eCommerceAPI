using eCommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eCommerceAPI.Controllers.Addresses
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        public readonly CommerceDbContext _dbContext;

        public AddressController(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<AddressController>
        [HttpGet("userAddressByUID/{uid}")]
        //public async Task<List<Address>> GetAdressByUserUID(Guid uid,CancellationToken cancellationToken)
        //{
        //    await _dbContext.Addresses.AsNoTracking()
        //        .Include(ad => ad.User).Select()
        //}

        // GET api/<AddressController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        //public async Task<ActionResult> AddUserAddress([FromBody] Guid userId, CreateAddressRequest request)
        //{
        //    var user = await _dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        //    if (user is not null)
        //    {
        //        _dbContext.Addresses.Include(x => x.User).Where(x => x.UserId == userId).Select(x => new Address
        //        {
        //            City = request.City,
        //            Region = request.Region,
        //            AddressLine = request.AddressLine,
        //            PostalCode = request.PostalCode
        //        });
        //        return Ok("User Address added successfully");
        //    }
        //    return NotFound();
        //}

        // PUT api/<AddressController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
