using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Domain.Aggregates.PatientAggregate;
using ms_patients_consul_core.Domain.Repositories;


namespace ms_patients_consul_core.Application.UseCases
{
    public class RegisterPatientUseCase
    {
        private readonly IPatientRepository _patientRepository;

        public RegisterPatientUseCase(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task ExecuteAsync(RegisterPatientInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // 1. Generate the shared unique identity (Business Responsibility f)
            Guid newPatientId = Guid.NewGuid();

            // 2. Instantiate Aggregate Root enforcing invariants through constructor
            var patient = new Patient(newPatientId, input.FullName, input.AssignedNutritionistId);

            // 3. Populate mandatory initial medical history and dietary habits (HU-01)
            patient.RegisterClinicalHistory(input.InitialClinicalHistoryDescription, input.ClinicalHistoryType);
            patient.UpdateEatingHabits(input.InitialEatingHabitDescription);

            // 4. Persist asynchronously through the infrastructure abstraction interface
            await _patientRepository.SaveAsync(patient);

            // 5. Clear domain events after successful unit of work persistence
            patient.ClearDomainEvents();
        }
    }
}
