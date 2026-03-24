using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stripeAPI.Data;

namespace stripeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly AppDbContext _db;
        private const string SecretKey = "MinHemmeligeNøgle123"; 

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers([FromQuery] string key)
        {
            if (key != SecretKey)
            {
                return Unauthorized("Forkert nøgle!"); 
            }

            var kunder = await _db.CustomerOrders.ToListAsync();
            return Ok(kunder);
        }
    }
}
