using InboxEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// TODO: Register the PriorityScoringService using Dependency Injection
// Use AddScoped to register IPriorityScoringService with PriorityScoringService implementation
builder.Services.AddScoped<IPriorityScoringService, PriorityScoringService>();
builder.Services.AddScoped<IInboxService, InboxService>();

// TODO: Configure CORS to allow requests from your frontend
// Common frontend URLs: http://localhost:3000, http://127.0.0.1:5500, http://localhost:5500
// Make sure to allow any header and any method

var app = builder.Build();

app.UseCors("AllowReact");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Enable CORS middleware here (must be before other middleware like UseAuthorization)

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
