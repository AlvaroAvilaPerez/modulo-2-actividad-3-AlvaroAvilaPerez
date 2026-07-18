using ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate;



namespace ms_patients_consul_core.Domain.Repositories
{
    public interface IConsultationRepository
    {
        Task SaveAsync(Consultation consultation);
        Task<Consultation?> GetByIdAsync(Guid consultationId);
        Task<IEnumerable<Consultation>> GetHistoryByPatientIdAsync(Guid patientId);
    }
}
