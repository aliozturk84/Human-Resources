﻿using Microsoft.EntityFrameworkCore;
using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.EntityFrameworkCore;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Pusula.Training.HealthCare.Doctors
{
    public class EfCoreDoctorRepository(IDbContextProvider<HealthCareDbContext> dbContextProvider)
        : EfCoreRepository<HealthCareDbContext, Doctor, Guid>(dbContextProvider), IDoctorRepository
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

            var ids = query.Select(x => x.Doctor.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }


        public virtual async Task<DoctorWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(doctor => new DoctorWithNavigationProperties
                {
                    Doctor = doctor,
                    Title = dbContext.Set<Title>().FirstOrDefault(c => c.Id == doctor.TitleId)!,
                    Department = dbContext.Set<Department>().FirstOrDefault(c => c.Id == doctor.DepartmentId)!
                })
                .FirstOrDefault()!;
        }

        public virtual async Task<List<DoctorWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DoctorConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<Doctor>> GetListAsync(
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
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DoctorConsts.GetDefaultSorting(false) : sorting);
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
        protected virtual IQueryable<Doctor> ApplyFilter(
            IQueryable<Doctor> query,
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

        protected virtual async Task<IQueryable<DoctorWithNavigationProperties>> GetQueryForNavigationPropertiesAsync() =>
            from doctor in (await GetDbSetAsync())
            join title in (await GetDbContextAsync()).Set<Title>() on doctor.TitleId equals title.Id into titles
            from title in titles.DefaultIfEmpty()
            join department in (await GetDbContextAsync()).Set<Department>() on doctor.DepartmentId equals department.Id into departments
            from department in departments.DefaultIfEmpty()
            select new DoctorWithNavigationProperties
            {
                Doctor = doctor,
                Title = title,
                Department = department
            };


        protected virtual IQueryable<DoctorWithNavigationProperties> ApplyFilter(
            IQueryable<DoctorWithNavigationProperties> query,
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
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Doctor.FirstName!.Contains(filterText!) || e.Doctor.LastName!.Contains(filterText!) || e.Doctor.PhoneNumber!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.Doctor.FirstName!.Contains(firstName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.Doctor.LastName!.Contains(lastName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.Doctor.PhoneNumber!.Contains(phoneNumber!))
                    .WhereIf(birthDateMin.HasValue, e => e.Doctor.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.Doctor.BirthDate <= birthDateMax!.Value)
                    .WhereIf(gender.HasValue, e => e.Doctor.Gender == gender)
                    .WhereIf(departmentId.HasValue, e => e.Department.Id == departmentId)
                    .WhereIf(titleId.HasValue, e => e.Title.Id == titleId);
        #endregion
    }
}
