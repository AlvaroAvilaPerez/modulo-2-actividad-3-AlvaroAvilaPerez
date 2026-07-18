using System;
using System.Collections.Generic;


namespace ms_patients_consul_core.Application.DTOs
{
    public record PatientHistoryDto(
        Guid PatientId,
        string FullName,
        List<ConsultationEntryDto> Consultations
    );

    public record ConsultationEntryDto(
        Guid ConsultationId,
        DateTime ConsultationDate,
        bool IsInitialConsultation,
        double Weight,
        double Height,
        double Bmi,
        string Conclusion,
        string Notes,
        string ClinicalAnalysis
    );
}
