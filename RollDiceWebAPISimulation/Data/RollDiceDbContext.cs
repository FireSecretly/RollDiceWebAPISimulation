using Microsoft.EntityFrameworkCore;
using RollDiceWebAPISimulation.Models;

namespace RollDiceWebAPISimulation.Data;

public class RollDiceDbContext : DbContext
{
    public RollDiceDbContext(DbContextOptions<RollDiceDbContext> options) : base(options) { }

    public DbSet<DiceRoll>? DiceRolls { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiceRoll>()
            .HasMany(d => d.Results)
            .WithOne(r => r.DiceRoll)
            .HasForeignKey(r => r.DiceRollId);
    }
}