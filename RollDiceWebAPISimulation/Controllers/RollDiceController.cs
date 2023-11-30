using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RollDiceWebAPISimulation.Data;
using RollDiceWebAPISimulation.Models;
using RollDiceWebAPISimulation.Services;

namespace RollDiceWebAPISimulation.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class RollDiceController : ControllerBase
{
    private readonly IDiceRollerService _diceRollerService;
    private readonly ILogger<RollDiceController> _logger;
    private readonly RollDiceDbContext _context;

    public RollDiceController (IDiceRollerService diceRollerService, RollDiceDbContext context, ILogger<RollDiceController> logger)
    {
        _diceRollerService = diceRollerService;
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [Route("RollDice")]
    public async Task<IActionResult> Roll(DiceSettings settings)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var results = await _diceRollerService.RollDiceAsync(settings);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing the dice roll");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("ShowRolls")]
    public async Task<IActionResult> ShowRolls()
    {
        try
        {
            var rolls = await _context.DiceRolls!.Include(dr => dr.Results).ToListAsync();
            return Ok(rolls);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving dice rolls");
            return StatusCode(500, "Internal server error");
        }
    }
}