using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ms_patients_consul_core.Domain.Aggregates.PatientAggregate;
using ms_patients_consul_core.Domain.Repositories;
using ms_patients_consul_core.Infrastructure.Persistence;

namespace ms_patients_consul_core.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientsConsultationDbContext _context;

        public PatientRepository(PatientsConsultationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            // Eagerly loading historical records to guarantee business invariants function on the aggregate root
            return await _context.Patients
                .Include(p => p.ClinicalHistories)
                .Include(p => p.EatingHabits)
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task SaveAsync(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));

            var exists = await _context.Patients.AnyAsync(p => p.PatientId == patient.PatientId);

            if (!exists)
            {
                await _context.Patients.AddAsync(patient);
            }
            else
            {
                _context.Patients.Update(patient);
            }

            await _context.SaveChangesAsync();
        }
    }
}
