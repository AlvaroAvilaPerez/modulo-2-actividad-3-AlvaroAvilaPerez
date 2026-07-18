using System;
using System.Threading.Tasks;
using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Domain.Repositories;

namespace ms_patients_consul_core.Application.UseCases
{
    public class AssignNutritionistUseCase
    {
        private readonly IPatientRepository _patientRepository;

        public AssignNutritionistUseCase(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task ExecuteAsync(AssignNutritionistInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // 1. Recuperar el estado actual del agregado desde el repositorio del dominio
            var patient = await _patientRepository.GetByIdAsync(input.PatientId);
            if (patient == null)
            {
                throw new InvalidOperationException($"The target patient with ID {input.PatientId} was not found.");
            }

            // 2. Ejecutar el método de negocio rico de la raíz del agregado para reasignar el nutricionista
            patient.AssignNutritionist(input.NewNutritionistId);

            // 3. Persistir los cambios en la infraestructura de datos
            await _patientRepository.SaveAsync(patient);

            // 4. Limpiar los marcadores de eventos de dominio de la entidad
            patient.ClearDomainEvents();
        }
    }
}
