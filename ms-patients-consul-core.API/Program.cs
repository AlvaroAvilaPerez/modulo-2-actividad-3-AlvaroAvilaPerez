using Microsoft.EntityFrameworkCore;
using ms_patients_consul_core.Application.UseCases;
using ms_patients_consul_core.Application.Queries;
using ms_patients_consul_core.Domain.Repositories;
using ms_patients_consul_core.Infrastructure.Persistence;
using ms_patients_consul_core.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- DATABASE PERSISTENCE CONFIGURATION ---
// Retrieves the runtime connection string from appsettings.json and registers EF Core 8
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PatientsConsultationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ms-patients-consul-core.Infrastructure")));

// --- APPLICATION & INFRASTRUCTURE DEPENDENCY INJECTION ---

// 1. Register Active Application Layer Use Cases and Query Services
builder.Services.AddScoped<RegisterPatientUseCase>();
builder.Services.AddScoped<ExecuteConsultationUseCase>();
builder.Services.AddScoped<UpdateEatingHabitsUseCase>();
builder.Services.AddScoped<AssignNutritionistUseCase>();
builder.Services.AddScoped<GetPatientHistoryQueryService>();

// 2. Register Active Physical Infrastructure Repositories (Fully Un-commented)
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();

// ---------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
