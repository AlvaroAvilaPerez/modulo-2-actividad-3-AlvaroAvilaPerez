namespace ms_patients_consul_core.Application.DTOs
{
    public record UpdateEatingHabitsInput(
        Guid PatientId,
        string NewEatingHabitDescription
    );
}
