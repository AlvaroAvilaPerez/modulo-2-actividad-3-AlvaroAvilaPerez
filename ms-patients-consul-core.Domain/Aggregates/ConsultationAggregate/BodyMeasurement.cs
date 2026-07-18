using ms_patients_consul_core.Domain.Exceptions;


namespace ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate
{
    public record BodyMeasurement
    {
        public double Weight { get; init; }
        public double Height { get; init; }
        public double FatPercentage { get; init; }
        public double MusclePercentage { get; init; }

        public BodyMeasurement() { }

        public BodyMeasurement(double weight, double height, double fatPercentage, double musclePercentage)
        {
            if (weight <= 0)
                throw new DomainException("Weight must be greater than 0 kg.");

            if (height <= 0 || height > 3.0)
                throw new DomainException("Height must be a valid metric value in meters (e.g., 1.75).");

            Weight = weight;
            Height = height;
            FatPercentage = fatPercentage;
            MusclePercentage = musclePercentage;
        }
    }
}
