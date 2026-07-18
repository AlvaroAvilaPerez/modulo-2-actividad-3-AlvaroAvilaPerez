using ms_patients_consul_core.Domain.Events;
using ms_patients_consul_core.Domain.Exceptions;

namespace ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate
{
    public class Consultation
    {
        public Guid ConsultationId { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid NutritionistId { get; private set; }
        public DateTime ConsultationDate { get; private set; }
        public bool IsInitialConsultation { get; private set; }

        // Immutable Value Objects
        public BodyMeasurement Measurement { get; private set; } = null!;
        public NutritionalDiagnosis Diagnosis { get; private set; } = null!;

        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


        protected Consultation() { }


        public Consultation(Guid consultationId, Guid patientId, Guid nutritionistId, bool isInitialConsultation, BodyMeasurement measurement)
        {
            if (consultationId == Guid.Empty || patientId == Guid.Empty || nutritionistId == Guid.Empty)
                throw new DomainException("Consultation, Patient, and Nutritionist IDs are mandatory.");

            Measurement = measurement ?? throw new DomainException("Body measurement parameters are required.");

            ConsultationId = consultationId;
            PatientId = patientId;
            NutritionistId = nutritionistId;
            IsInitialConsultation = isInitialConsultation;
            ConsultationDate = DateTime.UtcNow;

            // Triggers rich automated behavior
            CalculateInitialDiagnosis();
        }

        private void CalculateInitialDiagnosis()
        {
            double heightInMeters = Measurement.Height;
            double bmi = Measurement.Weight / (heightInMeters * heightInMeters);

            string conclusion = bmi < 18.5 ? "Underweight" :
                                bmi < 25.0 ? "Normal Weight" :
                                bmi < 30.0 ? "Overweight" : "Obesity";

            Diagnosis = new NutritionalDiagnosis(bmi, conclusion, "Automated preliminary diagnosis", string.Empty);

            if (IsInitialConsultation)
            {
                _domainEvents.Add(new InitialConsultationRegisteredEvent(ConsultationId, PatientId, bmi, conclusion));
            }
        }

        // Allows the professional to refine diagnosis details (HU-03 / HU-06)
        public void RefineProfessionalDiagnosis(string notes, string clinicalAnalysis)
        {
            if (string.IsNullOrWhiteSpace(notes))
                throw new DomainException("Professional notes are required to refine the diagnosis.");

            // Structural replacement via immutability pattern
            Diagnosis = Diagnosis with { Notes = notes, ClinicalAnalysis = clinicalAnalysis };
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
