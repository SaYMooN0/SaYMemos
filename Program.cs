using SaYMemos.Services.implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
Logger _logger = new();
builder.Services.AddSingleton<SaYMemos.Services.interfaces.ILogger, Logger>(provider => _logger);

string? dbConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
if (string.IsNullOrEmpty(dbConnectionString))
{
    _logger.CriticalError("DatabaseConnectionString is null or empty");
    return;
}
Database _db = new(dbConnectionString);
builder.Services.AddSingleton<SaYMemos.Services.interfaces.IDatabase, Database>(provider => _db);
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Memos/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Memos}/{action=Index}/{id?}");

app.Run();
