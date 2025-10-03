using ERP.Application;
using ERP.Infrastructure;
using ERP.WebAPI;
using ERP.WebAPI.Controllers;
using MapsterMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

builder.Services.AddSingleton(new Mapper());
builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();

builder.Services.AddControllers().AddOData(options =>
{

    options
        .EnableQueryFeatures()
        .AddRouteComponents("odata", AppODataController.GetEdmModel());

});

builder.Services.AddHttpContextAccessor();

builder.Services.AddRateLimiter(x =>

    x.AddFixedWindowLimiter("fixed", cfg =>
    {
        cfg.QueueLimit = 100;
        cfg.Window = TimeSpan.FromSeconds(1);
        cfg.PermitLimit = 100;
        cfg.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    })

);

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(x => x
    //.WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(t => true));

app.UseResponseCompression();

app.UseExceptionHandler();

app.MapOpenApi();

app.MapScalarApiReference();

app.MapControllers();
// .RequireRateLimiting("fixed");
// .RequireAuthorization();

// app.UseAuthentication();
// app.UseAuthorization();
await Helper.CreateUserAsync(app);

app.Run();