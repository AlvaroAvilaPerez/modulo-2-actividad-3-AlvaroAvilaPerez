[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/4gfZ4JAR)


# Microservice: ms-patients-consul-core
**Course:** Diplomado en Microservicios - Final Project  
**Layer:** Domain Layer Implementation (Domain-Driven Design)

This repository contains the pure Domain Layer implementation for the `ms-patients-consul-core` microservice, strictly following tactical Domain-Driven Design (DDD) patterns and Clean/Hexagonal Architecture boundaries using **C# (.NET 8)**.

## Class Diagram (Domain Model)

```mermaid
classDiagram
    class Patient {
        <<Aggregate Root>>
        +Guid PatientId
        +string FullName
        +Guid AssignedNutritionistId
        -List~ClinicalHistory~ _clinicalHistories
        -List~EatingHabit~ _eatingHabits
        -List~IDomainEvent~ _domainEvents
        +Patient(Guid id, string name, Guid docId)
        +RegisterClinicalHistory(string desc, string type)
        +UpdateEatingHabits(string desc)
        +ReassignNutritionist(Guid newDocId)
    }

    class ClinicalHistory {
        <<Entity>>
        +Guid Id
        +string Description
        +string Type
        +DateTime RegisteredAt
    }

    class EatingHabit {
        <<Entity>>
        +Guid Id
        +string Description
        +DateTime UpdatedAt
    }

    class Consultation {
        <<Aggregate Root>>
        +Guid ConsultationId
        +Guid PatientId
        +Guid NutritionistId
        +DateTime ConsultationDate
        +bool IsInitialConsultation
        +BodyMeasurement Measurement
        +NutritionalDiagnosis Diagnosis
        +Consultation(Guid id, Guid pId, Guid nId, bool isInitial, BodyMeasurement m)
        +RefineProfessionalDiagnosis(string notes, string analysis)
    }

    class BodyMeasurement {
        <<Value Object - Record>>
        +double Weight
        +double Height
        +double FatPercentage
        +double MusclePercentage
    }

    class NutritionalDiagnosis {
        <<Value Object - Record>>
        +double Bmi
        +string Conclusion
        +string Notes
        +string ClinicalAnalysis
    }

    class IDomainEvent {
        <<Interface>>
        +DateTime OccurredOn
    }

    Patient *-- ClinicalHistory : Encapsulates
    Patient *-- EatingHabit : Encapsulates
    Consultation o-- BodyMeasurement : Uses
    Consultation o-- NutritionalDiagnosis : Uses
    Patient ..> IDomainEvent : Publishes
    Consultation ..> IDomainEvent : Publishes
```

## Architectural & Tactical Design Decisions

1. **Rich Domain Model vs. Anemic Model**: Entities do not just act as simple data holders with public getters and setters. All internal state modifications are strictly encapsulated. Business logic rules (such as computing the initial BMI and automatic nutritional classification in `Consultation`) happen autonomously inside the Domain.
2. **Encapsulation & Aggregate Boundaries**: Internal lists (`_clinicalHistories`, `_eatingHabits`, `_domainEvents`) are marked as `private readonly` and exposed to the outside exclusively as `IReadOnlyCollection`. This blocks external layers from directly invoking operations like `.Add()` or `.Clear()`, enforcing the usage of valid domain behaviors.
3. **Value Objects**: Physical components (`BodyMeasurement` and `NutritionalDiagnosis`) are designed using C# structural records to guarantee native immutability and value-object equality semantics. They incorporate a Fail-Fast mechanism inside their constructor block to prevent objects from being instantiated in an invalid state.
4. **Decoupling via Domain Events**: Changes within aggregate states produce highly explicit asynchronous records (`NewPatientRegisteredEvent`, `InitialConsultationRegisteredEvent`) to stream events safely across other bounded contexts.
