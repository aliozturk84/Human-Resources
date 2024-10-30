using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.HRs
{
    public class HRWithNavigationProperties
    {
        public HR HR { get; set; } = null!;
        public Title Title { get; set; } = null!;
        public Department Department { get; set; } = null!;
    }
}
