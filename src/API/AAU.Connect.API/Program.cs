using AAU.Connect.Modules.Auth.Api;
using AAU.Connect.Modules.Timeline.Api;
using AAU.Connect.Modules.Groups.Api;
using AAU.Connect.Modules.Messaging.Api;
using AAU.Connect.Modules.Messaging.Infrastructure.Persistence;
using Carter;
using DotNetEnv;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using AAU.Connect.BuildingBlocks.Infrastructure.BackgroundJobs;
using AAU.Connect.BuildingBlocks.Infrastructure.Messaging;
using AAU.Connect.Modules.Auth.Infrastructure.EventHandlers;
using AAU.Connect.Modules.Timeline.Infrastructure.EventHandlers;
using AAU.Connect.Modules.Groups.Infrastructure.EventHandlers;
using AAU.Connect.Modules.Messaging.Infrastructure.EventHandlers;
using Quartz;

var currentDir = Directory.GetCurrentDirectory();
var envPath = Path.Combine(currentDir, "..", "..", "..", ".env");
if (!File.Exists(envPath))
{
    envPath = Path.Combine(currentDir, "..", "..", "..", "..", ".env");
}
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
    var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "railway";
    var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
    
    connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";
}

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Configuration["ConnectionStrings:AuthDb"] = connectionString;
}

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY")))
{
    builder.Configuration["Keycloak:Authority"] = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY");
}
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE")))
{
    builder.Configuration["Keycloak:Audience"] = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE");
}
if (bool.TryParse(Environment.GetEnvironmentVariable("KEYCLOAK_REQUIRE_HTTPS_METADATA"), out var requireHttps))
{
    builder.Configuration["Keycloak:RequireHttpsMetadata"] = requireHttps.ToString();
}

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RABBITMQ_HOST")))
{
    builder.Configuration["RabbitMQ:Host"] = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
}
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")))
{
    builder.Configuration["RabbitMQ:Username"] = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
}
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")))
{
    builder.Configuration["RabbitMQ:Password"] = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(AAU.Connect.Modules.Auth.Application.Handlers.RegisterUserCommandHandler).Assembly,
    typeof(AAU.Connect.Modules.Timeline.Application.Handlers.CreatePostCommandHandler).Assembly,
    typeof(AAU.Connect.Modules.Timeline.Infrastructure.Handlers.AddCommentCommandHandler).Assembly,
    typeof(AAU.Connect.Modules.Groups.Application.Handlers.CreateGroupCommandHandler).Assembly,
    typeof(AAU.Connect.Modules.Messaging.Infrastructure.Handlers.GetMessagesQueryHandler).Assembly
));

builder.Services.AddEventBus(builder.Configuration,
    typeof(UserRegisteredConsumer),
    typeof(PostCreatedConsumer),
    typeof(GroupCreatedConsumer),
    typeof(MessageSentConsumer));

builder.Services.AddAuthModule(builder.Configuration);
builder.Services.AddTimelineModule(builder.Configuration);
builder.Services.AddGroupsModule(builder.Configuration);
builder.Services.AddMessagingModule(builder.Configuration);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Keycloak:RequireHttpsMetadata"] ?? "false");
    });

builder.Services.AddAuthorization();

builder.Services.AddCarter();

builder.Services.AddQuartz(configure =>
{
    var jobKeyAuth = new JobKey("AuthOutboxJob");
    configure.AddJob<ProcessOutboxMessagesJob<AAU.Connect.Modules.Auth.Infrastructure.Persistence.AuthDbContext>>(opts => opts.WithIdentity(jobKeyAuth).StoreDurably())
             .AddTrigger(trigger => trigger.ForJob(jobKeyAuth).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));

    var jobKeyTimeline = new JobKey("TimelineOutboxJob");
    configure.AddJob<ProcessOutboxMessagesJob<AAU.Connect.Modules.Timeline.Infrastructure.Persistence.TimelineDbContext>>(opts => opts.WithIdentity(jobKeyTimeline).StoreDurably())
             .AddTrigger(trigger => trigger.ForJob(jobKeyTimeline).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));

    var jobKeyGroups = new JobKey("GroupsOutboxJob");
    configure.AddJob<ProcessOutboxMessagesJob<AAU.Connect.Modules.Groups.Infrastructure.Persistence.GroupsDbContext>>(opts => opts.WithIdentity(jobKeyGroups).StoreDurably())
             .AddTrigger(trigger => trigger.ForJob(jobKeyGroups).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));

    var jobKeyMessaging = new JobKey("MessagingOutboxJob");
    configure.AddJob<ProcessOutboxMessagesJob<AAU.Connect.Modules.Messaging.Infrastructure.Persistence.MessagingDbContext>>(opts => opts.WithIdentity(jobKeyMessaging).StoreDurably())
             .AddTrigger(trigger => trigger.ForJob(jobKeyMessaging).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var authContext = scope.ServiceProvider.GetRequiredService<AAU.Connect.Modules.Auth.Infrastructure.Persistence.AuthDbContext>();
    var timelineContext = scope.ServiceProvider.GetRequiredService<AAU.Connect.Modules.Timeline.Infrastructure.Persistence.TimelineDbContext>();
    var groupsContext = scope.ServiceProvider.GetRequiredService<AAU.Connect.Modules.Groups.Infrastructure.Persistence.GroupsDbContext>();
    var messagingContext = scope.ServiceProvider.GetRequiredService<AAU.Connect.Modules.Messaging.Infrastructure.Persistence.MessagingDbContext>();

    try
    {
        await authContext.Database.MigrateAsync();
        await timelineContext.Database.MigrateAsync();
        await groupsContext.Database.MigrateAsync();
        await messagingContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed (likely already initialized): {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();
