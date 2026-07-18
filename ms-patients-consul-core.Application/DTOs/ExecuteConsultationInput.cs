namespace ms_patients_consul_core.Application.DTOs
{
    public record ExecuteConsultationInput(
        Guid PatientId,
        Guid NutritionistId,
        bool IsInitialConsultation,
        double Weight,
        double Height,
        double FatPercentage,    // Cambiado de FatMass a FatPercentage
        double MusclePercentage  // Cambiado de MuscleMass a MusclePercentage
    );
}
