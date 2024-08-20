using Microsoft.EntityFrameworkCore;
using Ticket.Service.Interfaces;
using Ticket.Service.Repositories;
using Ticket.Data.Models.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Ticket.logging;
using Ticket.Service.HealthChecks;
using Microsoft.OpenApi.Models;
using Ticket.Integration.Services;
using Refit;
using Ticket.Integration.Proxy;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Ticket.JobScheduler.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddFileLogger(options =>
    {
        options.FolderPath = "logs";
        options.FilePath = "log-{date}.txt";
    });
});

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please Insert Token Here",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            new List<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IViewTeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<ITicketRepository, ViewTicketRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITicketClassRepository, TicketClassRepository>();
builder.Services.AddScoped<IManagerDashboardRepository, ManagerDashboardRepository>(); 
builder.Services.AddScoped<IHealthCheck, HealthCheckRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();



builder.Services.AddHttpClient();

builder.Services.AddHealthChecks()
    .AddSqlServer(
    connectionString: connectionString,
    healthQuery: "SELECT 1",
    name: "SqlServer Check",
    failureStatus: HealthStatus.Unhealthy,
    tags: new[] { "sql", "SqlServer", "healthcheks" })
    .AddCheck<HealthCheckRepository>("Our Custom Health Checks");


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var openAiSettings = builder.Configuration.GetSection("OpenAI");

builder.Services.AddRefitClient<IChatGPTProxy>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(openAiSettings["BaseUrl"]);
        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiSettings["ApiKey"]);
    });

builder.Services.AddScoped<ChatGPTService>();


var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("*")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("DailyJob");
    q.AddJob<DailyJob>(opts => opts.WithIdentity(jobKey));

    var cronExpression = builder.Configuration["JobScheduler:Jobs:DailyJob:CronExpression"] ?? "0 0 * * * ?";
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DailyJob-trigger")
        .WithCronSchedule(cronExpression));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket"));




app.UseHttpsRedirection();


app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
