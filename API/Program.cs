using API.Context;
using API.Singletons;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<API.Context.ContextSavedState>();
builder.Services.AddDbContext<ContextSwitch>();

builder.Services.AddSingleton<BearerTokenSingleton>();
builder.Services.AddSingleton<Dot1qSwPortConfigSingleton>();

builder.Services.AddMvc().AddControllersAsServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context1 = services.GetRequiredService<ContextSavedState>();
        var context2 = services.GetRequiredService<ContextSwitch>();

        context1.Database.Migrate();
        context2.Database.Migrate();
    }
    catch { }
}

Dot1qSwPortConfigSingleton dot1qSingleton = app.Services.GetRequiredService<Dot1qSwPortConfigSingleton>();
dot1qSingleton.StartPollingThread();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection()f;
app.UseAuthorization();
app.MapControllers();

app.Run();
