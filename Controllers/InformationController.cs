using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeekelApi.Data;

namespace VeekelApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InformationController : ControllerBase
    {

        private readonly ILogger<InformationController> _logger;
        private readonly ApplicationDbContext _context;
        public InformationController(ILogger<InformationController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        [Route("Information/Get")]
        public object Get()
        {
            return _context.Currencies.ToList(); ;

        }
    }
}