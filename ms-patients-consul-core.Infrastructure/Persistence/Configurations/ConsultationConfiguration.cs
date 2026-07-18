using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate;

namespace ms_patients_consul_core.Infrastructure.Persistence.Configurations
{
    public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
    {
        public void Configure(EntityTypeBuilder<Consultation> builder)
        {
            builder.ToTable("Consultations");

            builder.HasKey(c => c.ConsultationId);
            builder.Property(c => c.ConsultationId).ValueGeneratedNever();

            builder.Property(c => c.PatientId).IsRequired();
            builder.Property(c => c.NutritionistId).IsRequired();
            builder.Property(c => c.ConsultationDate).IsRequired();
            builder.Property(c => c.IsInitialConsultation).IsRequired();

            // Mapping the immutable BodyMeasurement Value Object (init-only properties)
            builder.OwnsOne(c => c.Measurement, m =>
            {
                m.Property(p => p.Weight).HasColumnName("Weight").IsRequired();
                m.Property(p => p.Height).HasColumnName("Height").IsRequired();
                m.Property(p => p.FatPercentage).HasColumnName("FatPercentage").IsRequired();
                m.Property(p => p.MusclePercentage).HasColumnName("MusclePercentage").IsRequired();
            });

            // Mapping the immutable NutritionalDiagnosis Value Object (record properties)
            builder.OwnsOne(c => c.Diagnosis, d =>
            {
                d.Property(p => p.Bmi).HasColumnName("Bmi").IsRequired();
                d.Property(p => p.Conclusion).HasColumnName("Conclusion").IsRequired().HasMaxLength(100);
                d.Property(p => p.Notes).HasColumnName("Notes").HasMaxLength(1000);
                d.Property(p => p.ClinicalAnalysis).HasColumnName("ClinicalAnalysis").HasMaxLength(1000);
            });

            builder.Ignore(c => c.DomainEvents);
        }
    }
}
