using Common.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApi.Models.Authentication;
using VeekelApi.Data;
using VeekelApi.Models;

namespace VeekelApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("SendConfig")]
        public object SendConfig([FromBody] ConfigHash requestHash)
        {
            Response response = new Response();
            ConfigurationResponse configurationResponse = new ConfigurationResponse();
            var dbHashes = _context.ConfigHashes.FirstOrDefault();
            if (dbHashes== null)
            {
                configurationResponse.Changed = false;
                return Ok(configurationResponse);
            }
            if ((dbHashes.CountriesHash != requestHash.CountriesHash) || (dbHashes.CurrenciesHash != requestHash.CurrenciesHash))
            {
                configurationResponse.Changed = true;
                configurationResponse.CurrenciesHash = dbHashes.CurrenciesHash;
                configurationResponse.CountriesHash = dbHashes.CountriesHash;
                configurationResponse.Countries = _context.Countries.ToList();
                configurationResponse.Currencies = _context.Currencies.ToList();
                return Ok(configurationResponse);
            }
            configurationResponse.Changed = false;
            return Ok(configurationResponse);

        }

    }
}
