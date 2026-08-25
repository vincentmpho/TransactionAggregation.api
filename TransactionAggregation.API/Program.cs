using TransactionAggregation.API.Middleware;
using TransactionAggregation.Application.Abstractions;
using TransactionAggregation.Application.Services;
using TransactionAggregation.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Configure Swagger with API Key Support for Auhorized Testing
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "X-Api-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your API key"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register the query service and all data sources.
builder.Services.AddScoped<IAggregationService, AggregationQueryService>();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Check the API key on every request before it reaches a controller.
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();