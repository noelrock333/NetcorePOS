using System.Text;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Define a CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", (builder) =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IAuthHelper,AuthHelper>();

var jwtSecret = configuration["ApplicationSettings:JWT_Secret"];
if (string.IsNullOrEmpty(jwtSecret)) {
    throw new ArgumentNullException("JWT_Secret", "JWT_Secret is not configured in the application settings.");
}

// Add authentication so we could use UseAuthentication later
builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes(jwtSecret)
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization so we could use UseAuthorization later
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Use the CORS policy
app.UseCors("AllowLocalhost3000");

// The order is important for UseAuthentication() and UseAuthorization()
app.UseAuthentication();

app.UseAuthorization();

// configuration is injected into the RegisterAuthorizationEndpoints method
app.RegisterAuthorizationEndpoints();
app.RegisterUserEndpoints(configuration);
app.RegisterProductsEndpoints();
app.RegisterCustomersEndpoint();


app.Run();

