using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ms_patients_consul_core.Domain.Aggregates.ConsultationAggregate;
using ms_patients_consul_core.Domain.Repositories;
using ms_patients_consul_core.Infrastructure.Persistence;

namespace ms_patients_consul_core.Infrastructure.Repositories
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly PatientsConsultationDbContext _context;

        public ConsultationRepository(PatientsConsultationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Consultation?> GetByIdAsync(Guid id)
        {
            return await _context.Consultations.FirstOrDefaultAsync(c => c.ConsultationId == id);
        }

        public async Task<IEnumerable<Consultation>> GetHistoryByPatientIdAsync(Guid patientId)
        {
            // Read-optimized performance lookup bypassing tracking engine for HU-04 Query pipeline
            return await _context.Consultations
                .AsNoTracking()
                .Where(c => c.PatientId == patientId)
                .OrderByDescending(c => c.ConsultationDate)
                .ToListAsync();
        }

        public async Task SaveAsync(Consultation consultation)
        {
            if (consultation == null) throw new ArgumentNullException(nameof(consultation));

            var exists = await _context.Consultations.AnyAsync(c => c.ConsultationId == consultation.ConsultationId);

            if (!exists)
            {
                await _context.Consultations.AddAsync(consultation);
            }
            else
            {
                _context.Consultations.Update(consultation);
            }

            await _context.SaveChangesAsync();
        }
    }
}
