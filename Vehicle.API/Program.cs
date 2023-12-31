using Microsoft.OpenApi.Models;
using System.Reflection;
using Vehicle.API.Application.Queries;
using Vehicle.API.Authentication;
using Vehicle.API.Extensions;
using Vehicle.API.Infrastructure;
using Vehicle.API.RealTime;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;
using Vehicle.Infrastructure;
using Vehicle.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Description = "Basic auth added to authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "basic",
        Type = SecuritySchemeType.Http
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
            },
            new List<string>()
        }
    });
});
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
});

builder.Services.AddSignalR();

builder.Services.AddScoped<IVehicleQueries>(sp => new VehicleQueries(builder.Configuration.GetConnectionString("VehiclesDB")!));
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

//Set up basic authentication
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<VehicleEventsClientHub>("/vehicle-events");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VehicleContext>();
    var logger = app.Services.GetService<ILogger<VehicleContextSeed>>();

    await new VehicleContextSeed().SeedAsync(context, logger!);
}

app.Run();
