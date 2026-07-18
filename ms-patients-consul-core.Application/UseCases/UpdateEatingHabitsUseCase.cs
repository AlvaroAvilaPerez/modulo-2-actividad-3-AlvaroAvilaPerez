using System;
using System.Threading.Tasks;
using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Domain.Repositories;

namespace ms_patients_consul_core.Application.UseCases
{
    public class UpdateEatingHabitsUseCase
    {
        private readonly IPatientRepository _patientRepository;

        public UpdateEatingHabitsUseCase(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task ExecuteAsync(UpdateEatingHabitsInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // 1. Recuperar el estado actual del agregado desde la base de datos usando el contrato del Dominio
            var patient = await _patientRepository.GetByIdAsync(input.PatientId);
            if (patient == null)
            {
                throw new InvalidOperationException($"The target patient with ID {input.PatientId} was not found.");
            }

            // 2. Ejecutar la regla de negocio rica del agregado (limpia/reemplaza el hábito y actualiza el UpdatedAt)
            patient.UpdateEatingHabits(input.NewEatingHabitDescription);

            // 3. Persistir el nuevo estado modificado del agregado
            await _patientRepository.SaveAsync(patient);

            // 4. Limpiar los eventos de dominio marcados en el ciclo de vida
            patient.ClearDomainEvents();
        }
    }
}
