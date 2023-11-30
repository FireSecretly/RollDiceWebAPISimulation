using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RollDiceWebAPISimulation.Data;
using RollDiceWebAPISimulation.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddDbContext<RollDiceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FuelPricingConnection")));
builder.Services.AddScoped<IDiceRollerService, DiceRollerService>();

// Register the Swagger generator only in development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "RollDiceSimulation API", Version = "v1" });
    });
}

var app = builder.Build();

// In development environment, enable Swagger UI and developer error page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RollDiceSimulation API v1"));
}
else // In non-development environments, use a generic error handler
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();