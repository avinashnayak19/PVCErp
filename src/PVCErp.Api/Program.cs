using PVCErp.Application;
using PVCErp.Application.Abstractions;
using PVCErp.Application.Services;
using PVCErp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://127.0.0.1:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
//builder.Services.AddScoped<ISocketingService, SocketingService>();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT
app.UseCors("AngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();