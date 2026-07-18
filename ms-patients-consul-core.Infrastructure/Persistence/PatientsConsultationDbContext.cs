using Microsoft.EntityFrameworkCore;
using ms_patients_consul_core.Domain.Aggregates.PatientAggregate;
using ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate;
using System.Reflection;

namespace ms_patients_consul_core.Infrastructure.Persistence
{
    public class PatientsConsultationDbContext : DbContext
    {
        // Constructor requerido para inyectar las opciones de configuración (ej. Connection String) desde la API
        public PatientsConsultationDbContext(DbContextOptions<PatientsConsultationDbContext> options)
            : base(options)
        {
        }

        // DbSets que exponen las Raíces de Agregado (Aggregate Roots) de tu Dominio
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Consultation> Consultations => Set<Consultation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Escanea automáticamente el ensamblado actual y aplica los mapeos de Fluent API de forma limpia
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
