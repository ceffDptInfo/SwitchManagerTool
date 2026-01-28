using API.Context;
using API.Singletons;

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
        var context = services.GetRequiredService<ContextSavedState>();

        context.Database.EnsureCreated();

    }
    catch (Exception e)
    {
        Console.WriteLine($"CRITICAL ERROR ::: {e.Message}");
        return;
    }
    try
    {

        var context = services.GetRequiredService<ContextSwitch>();

        context.Database.EnsureCreated();
    }
    catch (Exception e)
    {
        Console.WriteLine($"CRITICAL ERROR ::: {e.Message}");
        return;
    }
}

Dot1qSwPortConfigSingleton dot1qSingleton = app.Services.GetRequiredService<Dot1qSwPortConfigSingleton>();
dot1qSingleton.StartPollingThread();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();;
app.UseAuthorization();
app.MapControllers();

app.Run();
