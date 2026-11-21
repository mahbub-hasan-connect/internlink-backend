using InternLink.Api.Data;
using InternLink.Api.Repositories;
using InternLink.Api.Services;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// IUserRepositoryUR1 চাওয়া হলে UserRepositoryUR1 ব্যবহার করতে বলা হচ্ছে (Dependency Injection)
// AddScoped মানে — প্রতিটা HTTP request-এর জন্য একটা নতুন instance তৈরি হবে, পুরো request জুড়ে সেটাই ব্যবহার হবে
builder.Services.AddScoped<IUserRepositoryUR1, UserRepositoryUR1>();

// IAuthServiceAS1 চাওয়া হলে AuthServiceAS1 ব্যবহার করতে বলা হচ্ছে (Dependency Injection)
builder.Services.AddScoped<IAuthServiceAS1, AuthServiceAS1>();

// Module 2: Student Module — Repository আর Service register করা হচ্ছে
builder.Services.AddScoped<IStudentProfileRepositorySPR1, StudentProfileRepositorySPR1>();
builder.Services.AddScoped<IStudentProfileServiceSPS1, StudentProfileServiceSPS1>();

// Module 3: Recruiter Module — Repository আর Service register করা হচ্ছে
builder.Services.AddScoped<IJobPostingRepositoryJPR1, JobPostingRepositoryJPR1>();
builder.Services.AddScoped<IJobPostingServiceJPS1, JobPostingServiceJPS1>();

// Module 4: Admin Module — Service register করা হচ্ছে (আলাদা Repository লাগবে না, existing দুটো reuse হচ্ছে)
builder.Services.AddScoped<IAdminServiceADS1, AdminServiceADS1>();

// Module 5: AI Skill-Match Engine — Service register করা হচ্ছে
builder.Services.AddScoped<ISkillMatchServiceSMS1, SkillMatchServiceSMS1>();

// Module 6: Application & Status Tracking — Repository আর Service register করা হচ্ছে
builder.Services.AddScoped<IApplicationRepositoryAPR1, ApplicationRepositoryAPR1>();
builder.Services.AddScoped<IApplicationServiceAPS1, ApplicationServiceAPS1>();

// Module 7: Notification System — Repository আর Service register করা হচ্ছে
builder.Services.AddScoped<INotificationRepositoryNTR1, NotificationRepositoryNTR1>();
builder.Services.AddScoped<INotificationServiceNTS1, NotificationServiceNTS1>();

// JWT Authentication সেটআপ — এটা bole দেয় incoming request-এর token কিভাবে verify করতে হবে
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS — frontend (Vite dev server, default port 5173) runs on a different
// origin than this API (7182), so without this the browser blocks every
// request from React with a CORS error before it even reaches a controller.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); // UseAuthentication-এর আগে থাকতে হবে

app.UseAuthentication(); // Token verify করে — এটা UseAuthorization-এর আগে থাকতে হবে, নাহলে role check কাজ করবে না

app.UseAuthorization();

app.MapControllers();

app.Run();