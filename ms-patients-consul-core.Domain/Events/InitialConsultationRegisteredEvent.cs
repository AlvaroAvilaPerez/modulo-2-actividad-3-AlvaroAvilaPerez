namespace ms_patients_consul_core.Domain.Events
{    
    public record InitialConsultationRegisteredEvent(
    Guid ConsultationId,
    Guid PatientId,
    double ImcResult,
    string DiagnosisConclusion,
    DateTime OccurredOn) : IDomainEvent
    {
        public InitialConsultationRegisteredEvent(Guid consultationId, Guid patientId, double imcResult, string diagnosisConclusion)
            : this(consultationId, patientId, imcResult, diagnosisConclusion, DateTime.UtcNow) { }
    }
}
