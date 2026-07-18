namespace ms_patients_consul_core.Domain.Events
{
    public record NewPatientRegisteredEvent(
    Guid PatientId,
    string FullName,
    Guid AssignedNutritionistId,
    DateTime OccurredOn) : IDomainEvent
    {
        public NewPatientRegisteredEvent(Guid patientId, string fullName, Guid assignedNutritionistId)
            : this(patientId, fullName, assignedNutritionistId, DateTime.UtcNow) { }
    }
}
