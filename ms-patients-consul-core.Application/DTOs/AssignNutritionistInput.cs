using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms_patients_consul_core.Application.DTOs
{
    public record AssignNutritionistInput(
        Guid PatientId,
        Guid NewNutritionistId
    );
}
