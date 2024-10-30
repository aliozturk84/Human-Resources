using Microsoft.EntityFrameworkCore;
using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.EntityFrameworkCore;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Pusula.Training.HealthCare.Employees
{
    public class EfCoreEmployeeRepository(IDbContextProvider<HealthCareDbContext> dbContextProvider)
        : EfCoreRepository<HealthCareDbContext, Employee, Guid>(dbContextProvider), IEmployeeRepository
    {
        public virtual async Task DeleteAllAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, departmentId, titleId, leaveDaysMin, leaveDaysMax, salaryMin, salaryMax);

            var ids = query.Select(x => x.Employee.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }


        public virtual async Task<EmployeeWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(employee => new EmployeeWithNavigationProperties
                {
                    Employee = employee,
                    Title = dbContext.Set<Title>().FirstOrDefault(c => c.Id == employee.TitleId)!,
                    Department = dbContext.Set<Department>().FirstOrDefault(c => c.Id == employee.DepartmentId)!
                })
                .FirstOrDefault()!;
        }

        public virtual async Task<List<EmployeeWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, departmentId, titleId, leaveDaysMin, leaveDaysMax, salaryMin, salaryMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? EmployeeConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<Employee>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter(await GetQueryableAsync(), filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, departmentId, titleId, leaveDaysMin, leaveDaysMax, salaryMin, salaryMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? EmployeeConsts.GetDefaultSorting(false) : sorting);
            return await query.Page(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, departmentId, titleId, leaveDaysMin, leaveDaysMax, salaryMin, salaryMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        #region ApplyFilter and Queryable
        protected virtual IQueryable<Employee> ApplyFilter(
            IQueryable<Employee> query,
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            int? leaveDaysMin = null,  // Yeni parametre
            int? leaveDaysMax = null,  // Yeni parametre
            decimal? salaryMin = null,  // Yeni parametre
            decimal? salaryMax = null  // Yeni parametre
            ) =>
                query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstName!.Contains(filterText!) || e.LastName!.Contains(filterText!) || e.PhoneNumber!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName!.Contains(firstName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName!.Contains(lastName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.PhoneNumber!.Contains(phoneNumber!))
                    .WhereIf(birthDateMin.HasValue, e => e.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.BirthDate <= birthDateMax!.Value)
                    .WhereIf(departmentId.HasValue, e => e.DepartmentId == departmentId)
                    .WhereIf(titleId.HasValue, e => e.TitleId == titleId)
                    .WhereIf(leaveDaysMin.HasValue, e => e.LeaveDays >= leaveDaysMin.Value)
                    .WhereIf(leaveDaysMax.HasValue, e => e.LeaveDays <= leaveDaysMax.Value)
                    .WhereIf(salaryMin.HasValue, e => e.Salary >= salaryMin.Value)
                    .WhereIf(salaryMax.HasValue, e => e.Salary <= salaryMax.Value);

        protected virtual async Task<IQueryable<EmployeeWithNavigationProperties>> GetQueryForNavigationPropertiesAsync() =>
            from employee in (await GetDbSetAsync())
            join title in (await GetDbContextAsync()).Set<Title>() on employee.TitleId equals title.Id into titles
            from title in titles.DefaultIfEmpty()
            join department in (await GetDbContextAsync()).Set<Department>() on employee.DepartmentId equals department.Id into departments
            from department in departments.DefaultIfEmpty()
            select new EmployeeWithNavigationProperties
            {
                Employee = employee,
                Title = title,
                Department = department
            };


        protected virtual IQueryable<EmployeeWithNavigationProperties> ApplyFilter(
            IQueryable<EmployeeWithNavigationProperties> query,
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
            decimal? salaryMax = null  
            ) =>
                query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Employee.FirstName!.Contains(filterText!) || e.Employee.LastName!.Contains(filterText!) || e.Employee.PhoneNumber!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.Employee.FirstName!.Contains(firstName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.Employee.LastName!.Contains(lastName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.Employee.PhoneNumber!.Contains(phoneNumber!))
                    .WhereIf(birthDateMin.HasValue, e => e.Employee.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.Employee.BirthDate <= birthDateMax!.Value)
                    .WhereIf(departmentId.HasValue, e => e.Department.Id == departmentId)
                    .WhereIf(titleId.HasValue, e => e.Title.Id == titleId)
                    .WhereIf(leaveDaysMin.HasValue, e => e.Employee.LeaveDays >= leaveDaysMin.Value)
                    .WhereIf(leaveDaysMax.HasValue, e => e.Employee.LeaveDays <= leaveDaysMax.Value)
                    .WhereIf(salaryMin.HasValue, e => e.Employee.Salary >= salaryMin.Value)
                    .WhereIf(salaryMax.HasValue, e => e.Employee.Salary <= salaryMax.Value);
        #endregion
    }
}
