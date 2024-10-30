using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Employees
{
    public class GetEmployeesInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? TitleId { get; set; }
        public int? LeaveDaysMin { get; set; }
        public int? LeaveDaysMax { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }

        public GetEmployeesInput() { }
    }
}
