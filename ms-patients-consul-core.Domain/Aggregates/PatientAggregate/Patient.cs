using ms_patients_consul_core.Domain.Events;
using ms_patients_consul_core.Domain.Exceptions;

namespace ms_patients_consul_core.Domain.Aggregates.PatientAggregate
{
    public class Patient
    {
        // Unique and immutable identifier (HU-01 & Responsibility f)
        public Guid PatientId { get; private set; }
        public string FullName { get; private set; } = null!;
        public Guid AssignedNutritionistId { get; private set; }

        // Encapsulated collections to prevent external direct manipulation
        private readonly List<ClinicalHistory> _clinicalHistories = [];
        public IReadOnlyCollection<ClinicalHistory> ClinicalHistories => _clinicalHistories.AsReadOnly();

        private readonly List<EatingHabit> _eatingHabits = [];
        public IReadOnlyCollection<EatingHabit> EatingHabits => _eatingHabits.AsReadOnly();

        // Internal collection to track domain events
        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // Rich Constructor: Ensures the entity starts in a valid state
        public Patient(Guid patientId, string fullName, Guid assignedNutritionistId)
        {
            if (patientId == Guid.Empty) throw new DomainException("Patient ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(fullName)) throw new DomainException("Full name is required.");
            if (assignedNutritionistId == Guid.Empty) throw new DomainException("An initial nutritionist must be assigned.");

            PatientId = patientId;
            FullName = fullName;
            AssignedNutritionistId = assignedNutritionistId;

            // Register domain event
            _domainEvents.Add(new NewPatientRegisteredEvent(patientId, fullName, assignedNutritionistId));
        }

        // Cumulative Business Rule (HU-06) - Rich Domain behavior
        public void RegisterClinicalHistory(string description, string type)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Clinical history description cannot be empty.");

            var entry = new ClinicalHistory(Guid.NewGuid(), description, type, DateTime.UtcNow);
            _clinicalHistories.Add(entry);
        }

        // Evolutionary Update Business Rule (HU-07)
        public void UpdateEatingHabits(string newHabitsDescription)
        {
            if (string.IsNullOrWhiteSpace(newHabitsDescription))
                throw new DomainException("Eating habits description must contain valid details.");

            var habit = new EatingHabit(Guid.NewGuid(), newHabitsDescription, DateTime.UtcNow);
            _eatingHabits.Add(habit);
        }

        // Team management assignment business logic (HU-05)
        public void AssignNutritionist(Guid newNutritionistId)
        {
            if (newNutritionistId == Guid.Empty)
                throw new DomainException("Invalid nutritionist ID.");

            AssignedNutritionistId = newNutritionistId;
        }


        // Clears events once they are dispatched by the Application Layer
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
