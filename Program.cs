using DinkToPdf;
using DinkToPdf.Contracts;
using LearnAPI.AppDbContext;
using LearnAPI.ModelDTO;
using LearnAPI.Repositories.IRepository;
using LearnAPI.Repositories.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//});

//// Add services to the container.
//builder.Services.AddDistributedMemoryCache(); // Required for session
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//// Configure Serilog
//// Define ColumnOptions separately
//var columnOptions = new ColumnOptions();
//columnOptions.AdditionalColumns = new Collection<SqlColumn>
//{
//    new SqlColumn("ApplicationName", SqlDbType.NVarChar) { DataLength = 100 }
//};



//Log.Logger = new LoggerConfiguration()
//    .Enrich.WithProperty("ApplicationName", "TheBookNook")
//    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
//    .WriteTo.MSSqlServer(
//        connectionString: builder.Configuration.GetConnectionString("DefaultDb"),
//        sinkOptions: new MSSqlServerSinkOptions
//        {
//            TableName = "Logs_TheBookNook",
//            AutoCreateSqlTable = true
//        },
//        restrictedToMinimumLevel: LogEventLevel.Information // Only logs Information or higher
//    )
//    .Filter.ByIncludingOnly(logEvent =>
//        logEvent.Properties.ContainsKey("SourceContext") &&
//        logEvent.Properties["SourceContext"].ToString().Contains("LearnAPI")
//    )
//    .CreateLogger();


//builder.Host.UseSerilog();

//// Register filters
//builder.Services.AddScoped<LoggingFilter>();
//builder.Services.AddScoped<ValidateModelFilter>();
//builder.Services.AddScoped<GlobalExceptionFilter>();
//builder.Services.AddScoped<CustomAuthorizationFilter>();
//builder.Services.AddScoped<ResourceFilter>();
//builder.Services.AddScoped<ActionLoggingFilter>();
//builder.Services.AddScoped<CustomResultFilter>();

//// Register controllers and add filters
//builder.Services.AddControllers(
//    options =>
//{
//    options.Filters.AddService<LoggingFilter>();
//    options.Filters.AddService<ValidateModelFilter>();
//    options.Filters.AddService<GlobalExceptionFilter>();
//    options.Filters.AddService<CustomAuthorizationFilter>();
//    options.Filters.AddService<ResourceFilter>();
//    options.Filters.AddService<ActionLoggingFilter>();
//    options.Filters.AddService<CustomResultFilter>();
//}
//);
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
////Database Connection
//builder.Services.AddDbContext<BookNookDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));
//// Dependencies --
//builder.Services.AddSingleton<IStateRepository, StateRepository>();

//// Add JWT Authentication
////var key = "this_is_a_super_secret_key_for_jwt"; // Replace with your secret key
//var key = builder.Configuration["JwtSettings:SecretKey"];
//if (string.IsNullOrEmpty(key))
//    throw new Exception("JWT Secret Key is missing in configuration");

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = "https://localhost:7005",
//            ValidAudience = "https://localhost:7005",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
//        };

//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine("Authentication failed: " + context.Exception.Message);
//                return Task.CompletedTask;
//            }
//        };
//    });


////SwaggerGen
//builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter 'Bearer' followed by your token."
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});


//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//var context = new CustomAssemblyLoadContext();

////context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
//context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));


////Build application wkhtmltox
//var app = builder.Build();
//app.UseCors("AllowAll"); // ? Enable CORS globally

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    //app.UseSwaggerUI();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
//    });
//}

//app.UseHttpsRedirection();

////app.UseAuthorization();

//// Use Authentication and Authorization
//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();
//builder.Services.AddDbContext<BookNookDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));
//app.Run();

var builder = WebApplication.CreateBuilder(args);

// Enable CORS globally
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Serilog
var columnOptions = new ColumnOptions();
columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn("ApplicationName", SqlDbType.NVarChar) { DataLength = 100 }
};

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("ApplicationName", "TheBookNook")
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultDb"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs_TheBookNook",
            AutoCreateSqlTable = true
        },
        restrictedToMinimumLevel: LogEventLevel.Information
    )
    .Filter.ByIncludingOnly(logEvent =>
        logEvent.Properties.ContainsKey("SourceContext") &&
        logEvent.Properties["SourceContext"].ToString().Contains("LearnAPI")
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Register filters
//builder.Services.AddScoped<LoggingFilter>();
//builder.Services.AddScoped<ValidateModelFilter>();
//builder.Services.AddScoped<GlobalExceptionFilter>();
//builder.Services.AddScoped<CustomAuthorizationFilter>();
//builder.Services.AddScoped<ResourceFilter>();
//builder.Services.AddScoped<ActionLoggingFilter>();
//builder.Services.AddScoped<CustomResultFilter>();

//// Register controllers and add filters
//builder.Services.AddControllers(options =>
//{
//    options.Filters.AddService<LoggingFilter>();
//    options.Filters.AddService<ValidateModelFilter>();
//    options.Filters.AddService<GlobalExceptionFilter>();
//    options.Filters.AddService<CustomAuthorizationFilter>();
//    options.Filters.AddService<ResourceFilter>();
//    options.Filters.AddService<ActionLoggingFilter>();
//    options.Filters.AddService<CustomResultFilter>();
//});

//use when you have to stop the filters
builder.Services.AddControllers();

// Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "This is an API for The Book Nook project."
    });

    // Add JWT Authentication support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database Connection
builder.Services.AddDbContext<BookNookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));

// Dependencies
builder.Services.AddSingleton<IStateRepository, StateRepository>();

// Add JWT Authentication
var key = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(key))
    throw new Exception("JWT Secret Key is missing in configuration");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7005",
            ValidAudience = "https://localhost:7005",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

// Build application
//var app = builder.Build();

//app.UseCors("AllowAll"); // Enable CORS globally

//// Enable Swagger for all environments (not just Development)
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
//});

//// Configure the HTTP request pipeline
//app.UseHttpsRedirection();

//// Enable authentication and authorization
//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
try
{
    var app = builder.Build();
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application startup error: {ex.Message}");
    throw;
}



//using LearnAPI.AppDbContext;
//using LearnAPI.Repositories.IRepository;
//using LearnAPI.Repositories.Repository;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Serilog;
//using Serilog.Events;
//using Serilog.Sinks.MSSqlServer;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// CORS Configuration
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//});

//// Session Configuration
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//// Serilog Configuration
//var columnOptions = new ColumnOptions();
//columnOptions.AdditionalColumns = new Collection<SqlColumn>
//{
//    new SqlColumn("ApplicationName", SqlDbType.NVarChar) { DataLength = 100 }
//};

//Log.Logger = new LoggerConfiguration()
//    .Enrich.WithProperty("ApplicationName", "TheBookNook")
//    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
//    .WriteTo.MSSqlServer(
//        connectionString: builder.Configuration.GetConnectionString("DefaultDb"),
//        sinkOptions: new MSSqlServerSinkOptions
//        {
//            TableName = "Logs_TheBookNook",
//            AutoCreateSqlTable = true
//        },
//        restrictedToMinimumLevel: LogEventLevel.Information
//    )
//    .Filter.ByIncludingOnly(logEvent =>
//        logEvent.Properties.ContainsKey("SourceContext") &&
//        logEvent.Properties["SourceContext"].ToString().Contains("LearnAPI")
//    )
//    .CreateLogger();

//builder.Host.UseSerilog();

//// Add services
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

//// Database Connection
//builder.Services.AddDbContext<BookNookDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));

//// Dependency Injection
//builder.Services.AddSingleton<IStateRepository, StateRepository>();

//// JWT Authentication Configuration
//var key = builder.Configuration["JwtSettings:SecretKey"];
//if (string.IsNullOrEmpty(key))
//    throw new Exception("JWT Secret Key is missing in configuration");

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = "https://localhost:7005",
//            ValidAudience = "https://localhost:7005",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
//        };
//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine("Authentication failed: " + context.Exception.Message);
//                return Task.CompletedTask;
//            }
//        };
//    });

//// Swagger Security Setup
//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter 'Bearer' followed by your token."
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

//// Build the app
//var app = builder.Build();

//// Optional: Check DB Connection
//try
//{
//    using var scope = app.Services.CreateScope();
//    var db = scope.ServiceProvider.GetRequiredService<BookNookDbContext>();
//    db.Database.EnsureCreated();
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Database Error: {ex.Message}");
//}

//// Enable CORS globally
//app.UseCors("AllowAll");

//// Middleware pipeline
//app.UseHttpsRedirection();
//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

//// Enable Swagger in all environments
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheBookNook API V1");
//    c.RoutePrefix = string.Empty; // Swagger runs at root
//});

//app.MapControllers();
//app.Run();

