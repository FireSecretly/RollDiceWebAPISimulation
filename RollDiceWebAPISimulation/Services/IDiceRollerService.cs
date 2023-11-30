using RollDiceWebAPISimulation.Models;
namespace RollDiceWebAPISimulation.Services;

public interface IDiceRollerService
{
    Task<List<DiceRoll>> RollDiceAsync(DiceSettings settings);
}