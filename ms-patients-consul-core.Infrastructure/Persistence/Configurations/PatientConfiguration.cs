using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ms_patients_consul_core.Domain.Aggregates.PatientAggregate;

namespace ms_patients_consul_core.Infrastructure.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasKey(p => p.PatientId);
            builder.Property(p => p.PatientId).ValueGeneratedNever();

            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.AssignedNutritionistId)
                .IsRequired();

            // Mapping the internal cumulative clinical history collection (HU-06)
            builder.OwnsMany(p => p.ClinicalHistories, ch =>
            {
                ch.ToTable("PatientClinicalHistories");
                ch.HasKey(c => c.Id);
                ch.Property(c => c.Id).ValueGeneratedNever();
                ch.Property(c => c.Description).IsRequired().HasMaxLength(1000);
                ch.Property(c => c.Type).HasMaxLength(100);
                ch.Property(c => c.RegisteredAt).IsRequired();
                ch.WithOwner().HasForeignKey("PatientId");
            });

            // Mapping the internal updating dietary habits collection (HU-07)
            builder.OwnsMany(p => p.EatingHabits, eh =>
            {
                eh.ToTable("PatientEatingHabits");
                eh.HasKey(e => e.Id);
                eh.Property(e => e.Id).ValueGeneratedNever();
                eh.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                eh.Property(e => e.UpdatedAt).IsRequired();
                eh.WithOwner().HasForeignKey("PatientId");
            });

            // Tell EF Core to ignore domain event queues during table synchronization
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
