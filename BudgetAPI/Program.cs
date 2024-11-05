using BudgetAPI.Database;
using BudgetAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
    
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();