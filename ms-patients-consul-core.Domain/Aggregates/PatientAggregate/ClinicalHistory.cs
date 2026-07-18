using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms_patients_consul_core.Domain.Aggregates.PatientAggregate
{
    public class ClinicalHistory
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; }
        public DateTime RegisteredAt { get; private set; }

        internal ClinicalHistory(Guid id, string description, string type, DateTime registeredAt)
        {
            Id = id;
            Description = description;
            Type = type;
            RegisteredAt = registeredAt;
        }
    }
}
