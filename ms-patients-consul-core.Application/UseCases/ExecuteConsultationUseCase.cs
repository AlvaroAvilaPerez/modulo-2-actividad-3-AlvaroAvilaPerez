using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate;
using ms_patients_consul_core.Domain.Repositories;

namespace ms_patients_consul_core.Application.UseCases
{
    public class ExecuteConsultationUseCase
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;

        public ExecuteConsultationUseCase(IConsultationRepository consultationRepository, IPatientRepository patientRepository)
        {
            _consultationRepository = consultationRepository ?? throw new ArgumentNullException(nameof(consultationRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task ExecuteAsync(ExecuteConsultationInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // 1. Verify that the targeted patient exists inside the microservice context boundaries
            var patientExists = await _patientRepository.GetByIdAsync(input.PatientId);
            if (patientExists == null)
            {
                throw new InvalidOperationException($"The patient with ID {input.PatientId} does not exist.");
            }

            // 2. Generate immutable Body Measurement structural details matching your Domain constructor
            var measurement = new BodyMeasurement(
                input.Weight,
                input.Height,
                input.FatPercentage,
                input.MusclePercentage
            );

            // 3. Instantiate rich consultation entity (triggers automated internal BMI & diagnosis)
            Guid consultationId = Guid.NewGuid();
            var consultation = new Consultation(
                consultationId,
                input.PatientId,
                input.NutritionistId,
                input.IsInitialConsultation,
                measurement
            );

            // 4. Commit state changes to persistence infrastructure
            await _consultationRepository.SaveAsync(consultation);

            // 5. Clear state-change track markers
            consultation.ClearDomainEvents();
        }
    }
}
