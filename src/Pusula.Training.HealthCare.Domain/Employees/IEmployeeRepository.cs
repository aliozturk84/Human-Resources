using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Pusula.Training.HealthCare.Employees
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        Task DeleteAllAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            int? leaveDaysMin = null,
            int? leaveDaysMax = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            CancellationToken cancellationToken = default);

        Task<EmployeeWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<EmployeeWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            int? leaveDaysMin = null,
            int? leaveDaysMax = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);


        Task<List<Employee>> GetListAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            int? leaveDaysMin = null,
            int? leaveDaysMax = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            int? leaveDaysMin = null,
            int? leaveDaysMax = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);
    }
}
