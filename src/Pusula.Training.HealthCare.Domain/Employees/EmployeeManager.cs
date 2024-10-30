using Pusula.Training.HealthCare.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeManager(IEmployeeRepository employeeRepository) : DomainService
    {
        public virtual async Task<Employee> CreateAsync(
            Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, int leaveDays, decimal salary)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), EmployeeConsts.FirstNameMaxLength, EmployeeConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), EmployeeConsts.LastNameMaxLength, EmployeeConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), EmployeeConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));
            var employee = new Employee(
                GuidGenerator.Create(), titleId, departmentId, firstName, lastName, phoneNumber, birthDate, leaveDays, salary
                );

            return await employeeRepository.InsertAsync(employee);
        }

        public virtual async Task<Employee> UpdateAsync(
            Guid id, Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, int leaveDays, decimal salary)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), EmployeeConsts.FirstNameMaxLength, EmployeeConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), EmployeeConsts.LastNameMaxLength, EmployeeConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), EmployeeConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));

            var employee = await employeeRepository.GetAsync(id);

            employee.TitleId = titleId;
            employee.DepartmentId = departmentId;
            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.PhoneNumber = phoneNumber;
            employee.BirthDate = birthDate;
            employee.LeaveDays = leaveDays;
            employee.Salary = salary;

            return await employeeRepository.UpdateAsync(employee);
        }
    }
}
