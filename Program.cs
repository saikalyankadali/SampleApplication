using PagerDutySample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<PagerDutyService>();

var app = builder.Build();

app.MapControllers();

app.Run();