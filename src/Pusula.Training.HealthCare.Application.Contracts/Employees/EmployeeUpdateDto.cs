﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeUpdateDto
    {
        [Required]
        public Guid Id { get; set; } = default!;
        [Required]
        [StringLength(EmployeeConsts.FirstNameMaxLength, MinimumLength = EmployeeConsts.FirstNameMinLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(EmployeeConsts.LastNameMaxLength, MinimumLength = EmployeeConsts.LastNameMinLength)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(EmployeeConsts.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }

        public Guid DepartmentId { get; set; }
        public Guid TitleId { get; set; }

        [Required] 
        [Range(0, 365)] 
        public int LeaveDays { get; set; }

        [Required] 
        [Range(0, double.MaxValue)] 
        public decimal Salary { get; set; }
    }
}
