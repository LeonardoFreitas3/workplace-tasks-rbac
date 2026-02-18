using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkplaceTasks.API.Data;
using WorkplaceTasks.API.Services;

var builder = WebApplication.CreateBuilder(args);

// SERVICE CONFIGURATION

// Register controllers (required for API endpoints)
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });


// Register PostgreSQL DbContext
// This connects the application to the database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register built-in OpenAPI support (required for Scalar UI)
builder.Services.AddOpenApi();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(
            builder.Configuration["JwtSettings:SecretKey"]!
        );

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

    });

// Enable role-based authorization
builder.Services.AddAuthorization();

// Register custom JWT service
builder.Services.AddScoped<JwtService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();


// HTTP PIPELINE CONFIGURATION
// Enable OpenAPI JSON and Scalar UI in development
if (app.Environment.IsDevelopment())
{
    // Exposes OpenAPI document (JSON)
    app.MapOpenApi();

}

// Enable HTTPS redirection
//app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// Enable authentication middleware
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Create scope to run database migrations and seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Apply pending migrations automatically
    context.Database.Migrate();

    // Seed initial users (admin, manager, member)
    DbSeeder.Seed(context);
}

app.Run();
