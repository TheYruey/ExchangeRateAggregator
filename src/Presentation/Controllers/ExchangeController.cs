using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Services;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController(ExchangeService exchangeService) : ControllerBase
{
    [HttpGet("best-offer")]
    public async Task<IActionResult> GetBestOffer(
        [FromQuery] string from, 
        [FromQuery] string to, 
        [FromQuery] decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest("Amount must be positive.");
        }

        var request = new ExchangeRequest(from, to, amount);
        var offer = await exchangeService.GetBestOfferAsync(request, HttpContext.RequestAborted);

        if (offer == null)
        {
            return NotFound("No offers could be found from any provider.");
        }

        return Ok(offer);
    }
}