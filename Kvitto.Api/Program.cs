using Kvitto.Api.Extensions;
using Kvitto.Core.Repositories;
using Kvitto.Data;
using Kvitto.Data.Data;
using Kvitto.Data.Data.Repositories;
using Kvitto.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<KvittoApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KvittoApiContext") ?? throw new InvalidOperationException("Connection string 'KvittoApiContext' not found.")));

var _configuration = builder.Configuration;
builder.Services.AddSingleton(_configuration.GetSection("IdentityUser").Get<PasswordSettings>());
builder.Services.Configure<PasswordSettings>(_configuration.GetSection("IdentityUser").Bind);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

// Add services to the container.
builder.Services.AddScoped<IUoW, UoW>();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    //Dimitris tycker RequireConfirmedAccount är "onödigt" för detta projekt, men såg det som en utmaning att följa flödet för att göra confirm och få seeds som är confirmed också
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<KvittoApiContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver =
           new CamelCasePropertyNamesContractResolver();
    })
    .AddXmlDataContractSerializerFormatters()
    .ConfigureApiBehaviorOptions(setupAction =>
    {
        setupAction.InvalidModelStateResponseFactory = context =>
        {
            // create a problem details object
            var problemDetailsFactory = context.HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                    context.HttpContext,
                    context.ModelState);

            // add additional info not added by default
            problemDetails.Detail = "See the errors field for details.";
            problemDetails.Instance = context.HttpContext.Request.Path;

            // find out which status code to use
            var actionExecutingContext =
                  context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

            // if there are modelstate errors & all keys were correctly
            // found/parsed we're dealing with validation errors
            if ((context.ModelState.ErrorCount > 0) &&
                (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
            {
                problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "One or more validation errors occurred.";

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            }

            // if one of the keys wasn't correctly found / couldn't be parsed
            // we're dealing with null/unparsable input
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "One or more errors on input occurred.";
            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        };
    });

builder.Services.AddAutoMapper(typeof(KvittoMappings));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

app.SeedDataAsync().GetAwaiter().GetResult();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
        });
    });

}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
