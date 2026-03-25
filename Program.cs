using ConfigSetting;
using Microsoft.EntityFrameworkCore;
using ProductRecordSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with PostgreSQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);



// Add services to the container

builder.Services.Configure<MapApiOptions>(builder.Configuration.GetSection("MapApi"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ASPNETWEBAPI.Services.IStudentService, ASPNETWEBAPI.Services.StudentService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:*", "http://127.0.0.1:*", "file://")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetIsOriginAllowed(origin => true) 
                  .AllowCredentials();
        });
});

var app = builder.Build();


app.UseCors("AllowLocalhost");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();