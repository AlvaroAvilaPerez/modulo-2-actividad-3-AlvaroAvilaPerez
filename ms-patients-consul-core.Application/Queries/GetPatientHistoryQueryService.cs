using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Domain.Repositories;

namespace ms_patients_consul_core.Application.Queries
{
    public class GetPatientHistoryQueryService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;

        public GetPatientHistoryQueryService(IConsultationRepository consultationRepository, IPatientRepository patientRepository)
        {
            _consultationRepository = consultationRepository ?? throw new ArgumentNullException(nameof(consultationRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task<PatientHistoryDto?> ExecuteAsync(Guid patientId)
        {
            // 1. Verify patient existence first to avoid empty/misleading states
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                return null; // Controller will handle this as a 404 Not Found
            }

            // 2. Fetch the chronological list of consultations using your repository contract
            var consultations = await _consultationRepository.GetHistoryByPatientIdAsync(patientId);

            // 3. Project directly into the thin read-optimized DTO layout
            var consultationEntries = consultations
                .OrderByDescending(c => c.ConsultationDate) // Newest consultations first
                .Select(c => new ConsultationEntryDto(
                    c.ConsultationId,
                    c.ConsultationDate,
                    c.IsInitialConsultation,
                    c.Measurement.Weight,
                    c.Measurement.Height,
                    c.Diagnosis.Bmi,
                    c.Diagnosis.Conclusion,
                    c.Diagnosis.Notes,
                    c.Diagnosis.ClinicalAnalysis
                ))
                .ToList();

            return new PatientHistoryDto(patient.PatientId, patient.FullName, consultationEntries);
        }
    }
}
