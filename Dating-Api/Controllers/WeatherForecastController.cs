using Dating_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.Controllers
{
    //[Authorize]
    //[Route("[controller]/[action]/{id?}")]
    //[ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly DataContext _context;

        public WeatherForecastController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeathers()
        {
            var weathers =await _context.WeatherForecasts.ToListAsync();
            return Ok(weathers);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetWeatherById(int Id)
        {
            var weather =await _context.WeatherForecasts.FirstOrDefaultAsync(w => w.ID == Id);
            return Ok(weather);
        }

    }
}
