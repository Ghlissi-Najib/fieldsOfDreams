using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Thefieldofdreams.API.Hubs;
using Thefieldofdreams.API.Services;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Application.Security;
using Thefieldofdreams.Application.Services;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;
using Thefieldofdreams.Infrastructure.Identity;
using Thefieldofdreams.Infrastructure.Repositories;
using Thefieldofdreams.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("JwtSettings");

// ===================== DATABASE =====================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection"),
        o => o.UseNetTopologySuite()
    )
);

// ===================== IDENTITY =====================
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ===================== JWT AUTH =====================
var jwtSecurityKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Ensure long-URI claim types (ClaimTypes.Role etc.) are restored from
    // their short JWT form so RolePermissionMatrix.HasPermission works correctly.
    options.MapInboundClaims = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = jwtSecurityKey,
        ClockSkew = TimeSpan.Zero
    };

    // Allow SignalR clients to pass the JWT token via the query string
    // (WebSocket and Server-Sent Events connections cannot set HTTP headers)
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    foreach (var policy in RbacPolicies.All)
    {
        options.AddPolicy(policy.Key, authPolicy =>
            authPolicy
                .RequireAuthenticatedUser()
                .RequireAssertion(context => RolePermissionMatrix.HasPermission(context.User, policy.Value)));
    }

    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    // Development: allow any origin (cannot be combined with AllowCredentials)
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

    // Production-ready policy for SignalR: restrict to known origins and allow credentials
    options.AddPolicy("AllowSignalR", policy =>
        policy.WithOrigins(
                  builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                  ?? new[] { "http://localhost:3000", "http://localhost:4200" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// ===================== REPOSITORIES =====================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRewardRepository, RewardRepository>();
builder.Services.AddScoped<IUserRouteCompletionRepository, UserRouteCompletionRepository>();
builder.Services.AddScoped<IUserRewardRepository, UserRewardRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Notification service (SignalR-based)
builder.Services.AddScoped<INotificationService, HubNotificationService>();

// Feature services
builder.Services.AddScoped<IPredictionService, PredictionService>();
builder.Services.AddScoped<IMissionService, MissionService>();
builder.Services.AddScoped<IUserMissionService, UserMissionService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IQRCampaignService, QRCampaignService>();
builder.Services.AddScoped<ISponsorService, SponsorService>();
builder.Services.AddScoped<IQRCodeService, QRCodeService>();
builder.Services.AddScoped<IQRScanService, QRScanService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ITourismRouteService, TourismRouteService>();
builder.Services.AddScoped<IUserRouteCompletionService, UserRouteCompletionService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IUserRewardService, UserRewardService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IReferralService, ReferralService>();


// Campaign orchestration
builder.Services.AddScoped<ICampaignOrchestrator, CampaignOrchestratorService>();

// Background services
builder.Services.AddHostedService<GpsPollingBackgroundService>();
builder.Services.AddHostedService<CampaignLifecycleBackgroundService>();

// ===================== SERVICES =====================

// Add SignalR services
builder.Services.AddSignalR();

// HTTP client for external APIs (geocoding + routing)
builder.Services.AddHttpClient("TravelEstimation", client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("MaintenanceManagement/1.0");
    client.Timeout = TimeSpan.FromSeconds(15);
});

// In-memory cache for travel estimates
builder.Services.AddMemoryCache();


//builder.Services.AddAutoMapper(typeof(MappingProfile));

// ===================== CONTROLLERS =====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ===================== SWAGGER =====================
const string swaggerSchemeId = "Bearer";

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Thefieldofdreams System API",
        Version = "v1"
    });

    // ✅ Swashbuckle v10 compatible security definition
    options.AddSecurityDefinition(swaggerSchemeId, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter your JWT token below.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",        // must be lowercase
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(swaggerSchemeId, doc)] = []
    });
});

var app = builder.Build();

// ===================== DATABASE SEEDING =====================
using (var scope = app.Services.CreateScope())
{
    try
    {
        await DataSeeder.SeedAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// ===================== PIPELINE =====================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foot-Thefieldofdreams System API v1");
    c.RoutePrefix = string.Empty;
});

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/hubs/notifications").RequireCors("AllowSignalR");
//app.MapHub<ChatHub>("/hubs/chat").RequireCors("AllowSignalR");

app.MapControllers();

app.Run();
