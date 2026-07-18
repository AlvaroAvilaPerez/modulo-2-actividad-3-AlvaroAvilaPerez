namespace ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate
{
    public record NutritionalDiagnosis
    {
        public double Bmi { get; init; }
        public string Conclusion { get; init; } = null!;
        public string Notes { get; init; } = null!;
        public string ClinicalAnalysis { get; init; } = null!;

        protected NutritionalDiagnosis() { }
                
        public NutritionalDiagnosis(double bmi, string conclusion, string notes, string clinicalAnalysis)
        {
            Bmi = bmi;
            Conclusion = conclusion;
            Notes = notes;
            ClinicalAnalysis = clinicalAnalysis;
        }
    }
}
