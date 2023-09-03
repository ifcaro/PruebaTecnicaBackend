using Vehicle.API.Application.Queries;
using Vehicle.API.Extensions;
using Vehicle.API.Infrastructure;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;
using Vehicle.Infrastructure;
using Vehicle.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
});

builder.Services.AddScoped<IVehicleQueries>(sp => new VehicleQueries(builder.Configuration.GetConnectionString("VehiclesDB")!));
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VehicleContext>();
    var logger = app.Services.GetService<ILogger<VehicleContextSeed>>();

    await new VehicleContextSeed().SeedAsync(context, logger!);
}

app.Run();
