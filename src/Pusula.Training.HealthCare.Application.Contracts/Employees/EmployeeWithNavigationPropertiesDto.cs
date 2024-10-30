using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeWithNavigationPropertiesDto
    {
        public EmployeeDto Employee { get; set; } = null!;
        public TitleDto Title { get; set; } = null!;
        public DepartmentDto Department { get; set; } = null!;
    }
}
