using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.HRs
{
    public class HRWithNavigationPropertiesDto
    {
        public HRDto HR { get; set; } = null!;
        public TitleDto Title { get; set; } = null!;
        public DepartmentDto Department { get; set; } = null!;
    }
}
