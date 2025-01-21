var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddLogging(opts =>
{
    opts.AddConsole();
});

// Db Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});

// Identity db context
builder.Services.AddIdentityConfiguration();

// Distributed Cache
builder.Services.AddDistributedMemoryCache();

// Authorization
builder.Services.AddAuthorization();

// Scopes
builder.Services.AddLifecycle();

// Token settings
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));

// Jwt settings
builder.Services.AddJwtOptions(builder.Configuration);

// Swagger settings
builder.Services.ConfigureSwagger();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.ConfigureCustomExceptionMiddleware();




using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Database Migration Ba�lat�l�yor.");
        await db.Database.MigrateAsync();

        logger.LogInformation("Seed i�lemi Ba�lat�l�yor.");
        await AppSeed.SeedDataAsync(services);
        logger.LogInformation("Database Migration ve Seed i�lemi Ba�ar�yla Tamamland�.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database Migration veya Seed i�lemi S�ras�nda Bir Hata Olu�tu.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Multiple-Layer.Api v1.0.0");
        opts.InjectStylesheet("/swagger-css/swagger-custom-ui.css");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseStaticFiles();


app.Run();