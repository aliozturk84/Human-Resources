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

namespace Pusula.Training.HealthCare.HRs
{
    public class EfCoreHRRepository(IDbContextProvider<HealthCareDbContext> dbContextProvider)
        : EfCoreRepository<HealthCareDbContext, HR, Guid>(dbContextProvider), IHRRepository
    {
        public virtual async Task DeleteAllAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender, departmentId, titleId);

            var ids = query.Select(x => x.HR.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }


        public virtual async Task<HRWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(hr => new HRWithNavigationProperties
                {
                    HR = hr,
                    Title = dbContext.Set<Title>().FirstOrDefault(c => c.Id == hr.TitleId)!,
                    Department = dbContext.Set<Department>().FirstOrDefault(c => c.Id == hr.DepartmentId)!
                })
                .FirstOrDefault()!;
        }

        public virtual async Task<List<HRWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender, departmentId, titleId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HRConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<HR>> GetListAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter(await GetQueryableAsync(), filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender, departmentId, titleId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HRConsts.GetDefaultSorting(false) : sorting);
            return await query.Page(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, phoneNumber, birthDateMin, birthDateMax, gender, departmentId, titleId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        #region ApplyFilter and Queryable
        protected virtual IQueryable<HR> ApplyFilter(
            IQueryable<HR> query,
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null) =>
                query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstName!.Contains(filterText!) || e.LastName!.Contains(filterText!) || e.PhoneNumber!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName!.Contains(firstName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName!.Contains(lastName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.PhoneNumber!.Contains(phoneNumber!))
                    .WhereIf(birthDateMin.HasValue, e => e.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.BirthDate <= birthDateMax!.Value)
                    .WhereIf(gender.HasValue, e => e.Gender == gender)
                    .WhereIf(departmentId.HasValue, e => e.DepartmentId == departmentId)
                    .WhereIf(titleId.HasValue, e => e.TitleId == titleId);

        protected virtual async Task<IQueryable<HRWithNavigationProperties>> GetQueryForNavigationPropertiesAsync() =>
            from hr in (await GetDbSetAsync())
            join title in (await GetDbContextAsync()).Set<Title>() on hr.TitleId equals title.Id into titles
            from title in titles.DefaultIfEmpty()
            join department in (await GetDbContextAsync()).Set<Department>() on hr.DepartmentId equals department.Id into departments
            from department in departments.DefaultIfEmpty()
            select new HRWithNavigationProperties
            {
                HR = hr,
                Title = title,
                Department = department
            };


        protected virtual IQueryable<HRWithNavigationProperties> ApplyFilter(
            IQueryable<HRWithNavigationProperties> query,
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null) =>
                query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.HR.FirstName!.Contains(filterText!) || e.HR.LastName!.Contains(filterText!) || e.HR.PhoneNumber!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.HR.FirstName!.Contains(firstName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.HR.LastName!.Contains(lastName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.HR.PhoneNumber!.Contains(phoneNumber!))
                    .WhereIf(birthDateMin.HasValue, e => e.HR.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.HR.BirthDate <= birthDateMax!.Value)
                    .WhereIf(gender.HasValue, e => e.HR.Gender == gender)
                    .WhereIf(departmentId.HasValue, e => e.Department.Id == departmentId)
                    .WhereIf(titleId.HasValue, e => e.Title.Id == titleId);
        #endregion
    }
}
