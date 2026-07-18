using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms_patients_consul_core.Domain.Aggregates.PatientAggregate
{
    public class EatingHabit
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        internal EatingHabit(Guid id, string description, DateTime updatedAt)
        {
            Id = id;
            Description = description;
            UpdatedAt = updatedAt;
        }
    }
}
