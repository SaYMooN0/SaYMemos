using dotenv.net;
using Microsoft.AspNetCore.Authentication.Cookies;
using SaYMemos.Services.implementations;
using SaYMemos.Services.interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        if (!TryConfigureServices(builder))
            return;

        var app = builder.Build();

        app.UseStatusCodePagesWithRedirects("/authorization?statusCode={0}");
        ConfigureMiddleware(app);

        app.Run();
    }

    private static bool TryConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options => { options.LoginPath = "/authorization"; });

        Logger logger = new();
        builder.Services.AddSingleton<SaYMemos.Services.interfaces.ILogger, Logger>(provider => logger);

        if (!LoadEnvironmentVariables(out var envVars, logger))
            return false;

        if (!TryConfigureDatabase(envVars, logger, builder) ||
            !TryConfigureEmailService(envVars, logger, builder) ||
            !TryConfigureEncryption(envVars, logger, builder) ||
            !TryConfigureImageStorage(logger, builder))
        {
            return false;
        }

        return true;
    }

    private static bool LoadEnvironmentVariables(out IDictionary<string, string> envVars, Logger logger)
    {
        DotEnv.Load();
        envVars = DotEnv.Read();

        if (envVars.Count == 0)
        {
            logger.CriticalError("Environment variables are not loaded from .env file");
            return false;
        }

        return true;
    }

    private static bool TryConfigureDatabase(IDictionary<string, string> envVars, Logger logger, WebApplicationBuilder builder)
    {
        if (!TryGetEnvVar(envVars, "DB_CONN_STR", out string dbConnectionString))
        {
            logger.CriticalError("DatabaseConnectionString is null or empty");
            return false;
        }

        Database db = new(dbConnectionString, logger);
        builder.Services.AddSingleton<IDatabase, Database>(provider => db);
        return true;
    }

    private static bool TryConfigureEmailService(IDictionary<string, string> envVars, Logger logger, WebApplicationBuilder builder)
    {
        if (!TryGetEnvVar(envVars, "SMTP_HOST", out string smtpHost) ||
            !TryGetEnvVar(envVars, "SMTP_USER", out string smtpUser) ||
            !TryGetEnvVar(envVars, "SMTP_PASSWORD", out string smtpPassword) ||
            !TryGetEnvVar(envVars, "SMTP_PORT", out string smtpPortString) || !Int32.TryParse(smtpPortString, out int smtpPort))
        {
            logger.CriticalError("SMTP configuration is not fully set in .env file");
            return false;
        }

        EmailService emailService = new(smtpHost, smtpUser, smtpPassword, smtpPort, logger);
        builder.Services.AddSingleton<IEmailService, EmailService>(provider => emailService);
        return true;
    }

    private static bool TryConfigureEncryption(IDictionary<string, string> envVars, Logger logger, WebApplicationBuilder builder)
    {
        if (!TryGetEnvVar(envVars, "ID_ENCRYPTION", out string idEncKey) ||
            !TryGetEnvVar(envVars, "CONFIRMATION_ENCRYPTION", out string confirmationEncKey) ||
            !TryGetEnvVar(envVars, "PASSWORD_ENCRYPTION", out string passwordEncKey))
        {
            logger.CriticalError("Encryption keys are not fully set in .env file");
            return false;
        }

        Encryptor encryptor = new(idEncKey, confirmationEncKey, passwordEncKey);
        builder.Services.AddSingleton<IEncryptor, Encryptor>(provider => encryptor);

        return true;
    }

    private static bool TryConfigureImageStorage(Logger logger, WebApplicationBuilder builder)
    {
        ImageStorageService imageStorageService = new(logger);
        builder.Services.AddSingleton<IImageStorageService, ImageStorageService>(provider => imageStorageService);
        return true;
    }
    private static void ConfigureMiddleware(WebApplication app)
    {
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
    }
    private static bool TryGetEnvVar(IDictionary<string, string> envVars, string key, out string value) =>
        envVars.TryGetValue(key, out value) && !string.IsNullOrWhiteSpace(value);


}
