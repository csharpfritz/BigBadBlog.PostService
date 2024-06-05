using BigBadBlog.PostService.Api;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMongoDBClient("posts-database");
//builder.Services.AddDbContext<PostDbContext>(options => options.UseSqlite());
builder.Services.AddScoped<PostDbContext>(svc =>
{
	var scope = svc.CreateScope();
	return PostDbContext.Create(scope.ServiceProvider.GetRequiredService<IMongoDatabase>());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPostEndpoints();

app.Run();
