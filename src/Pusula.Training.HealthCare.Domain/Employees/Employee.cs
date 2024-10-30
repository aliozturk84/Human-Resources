using JetBrains.Annotations;
using Pusula.Training.HealthCare.HRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;

namespace Pusula.Training.HealthCare.Employees
{
    public class Employee : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string FirstName { get; set; }
        [NotNull]
        public virtual string LastName { get; set; }
        [NotNull]
        public virtual string PhoneNumber { get; set; }
        [NotNull]
        public virtual DateTime BirthDate { get; set; }
        public virtual Guid DepartmentId { get; set; }
        public virtual Guid TitleId { get; set; }

        public virtual int LeaveDays { get; set; } 
        public virtual decimal Salary { get; set; }

        protected Employee()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            BirthDate = DateTime.Now;
            LeaveDays = 0;
            Salary = 0;
        }

        public Employee(Guid id, Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, int leaveDays, decimal salary)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), EmployeeConsts.FirstNameMaxLength, EmployeeConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), EmployeeConsts.LastNameMaxLength, EmployeeConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), EmployeeConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));


            Id = id;
            TitleId = titleId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            LeaveDays = leaveDays;
            Salary = salary;
        }
    }
}
