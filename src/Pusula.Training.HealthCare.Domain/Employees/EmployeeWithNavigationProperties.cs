﻿using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.HRs;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeWithNavigationProperties
    {
        public Employee Employee { get; set; } = null!;
        public Title Title { get; set; } = null!;
        public Department Department { get; set; } = null!;
    }
}
