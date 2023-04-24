using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Behaviours;
using Parking.FindingSlotManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Register the IServiceCollection
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "ParkZ API V1");
});

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();

app.UseMiddleware<LogMiddleware>();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
