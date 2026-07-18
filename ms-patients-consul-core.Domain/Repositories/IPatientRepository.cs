using ms_patients_consul_core.Domain.Aggregates.PatientAggregate;


namespace ms_patients_consul_core.Domain.Repositories
{
    public interface IPatientRepository
    {
        Task SaveAsync(Patient patient);
        Task<Patient?> GetByIdAsync(Guid patientId);
    }
}
