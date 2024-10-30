using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Permissions;
using Pusula.Training.HealthCare.Shared;
using Pusula.Training.HealthCare.Titles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp;
using static Pusula.Training.HealthCare.Permissions.HealthCarePermissions;
using System.Linq.Dynamic.Core;
using MiniExcelLibs;

namespace Pusula.Training.HealthCare.Employees
{
    [RemoteService(IsEnabled = false)]
    [Authorize(HealthCarePermissions.Employees.Default)]
    public class EmployeesAppService(
        IEmployeeRepository employeeRepository,
        EmployeeManager employeeManager,
        IDistributedCache<EmployeeDownloadTokenCacheItem, string> downloadTokenCache,
        ITitleRepository titleRepository,
        IDepartmentRepository departmentRepository,
        IDistributedEventBus distributedEventBus) : HealthCareAppService, IEmployeesAppService
    {

        public virtual async Task<PagedResultDto<EmployeeWithNavigationPropertiesDto>> GetListAsync(GetEmployeesInput input)
        {
            var sortingValue = string.IsNullOrWhiteSpace(input.Sorting) ? null : (int?)Convert.ToInt32(input.Sorting);
            var totalCount = await employeeRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.DepartmentId, input.TitleId, sortingValue, input.MaxResultCount, input.SkipCount);
            var items = await employeeRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.DepartmentId, input.TitleId, sortingValue, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<EmployeeWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<EmployeeWithNavigationProperties>, List<EmployeeWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            var employee = await employeeRepository.GetWithNavigationPropertiesAsync(id);
            await distributedEventBus.PublishAsync(new EmployeeTitleAndDepartmentEto { Department = employee.Department.Name, Title = employee.Title.Name });
            return ObjectMapper.Map<EmployeeWithNavigationProperties, EmployeeWithNavigationPropertiesDto>(employee);
        }


        public virtual async Task<EmployeeDto> GetAsync(Guid id) => ObjectMapper.Map<Employee, EmployeeDto>(
                await employeeRepository.GetAsync(id));

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input)
        {
            var query = (await titleRepository.GetQueryableAsync())
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                               x => x.Name != null && x.Name.Contains(input.Filter!));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Title>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Title>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input)
        {
            var query = (await departmentRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null && x.Name.Contains(input.Filter!));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Department>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Department>, List<LookupDto<Guid>>>(lookupData)
            };
        }


        [Authorize(HealthCarePermissions.Employees.Delete)]
        public virtual async Task DeleteAsync(Guid id) => await employeeRepository.DeleteAsync(id);

        [Authorize(HealthCarePermissions.Employees.Create)]
        public virtual async Task<EmployeeDto> CreateAsync(EmployeeCreateDto input) => ObjectMapper.Map<Employee, EmployeeDto>(
            await employeeManager.CreateAsync(input.TitleId, input.DepartmentId, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDate, input.LeaveDays, input.Salary));

        [Authorize(HealthCarePermissions.Employees.Edit)]
        public virtual async Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input) => ObjectMapper.Map<Employee, EmployeeDto>(
            await employeeManager.UpdateAsync(input.Id, input.TitleId, input.DepartmentId, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDate, input.LeaveDays, input.Salary));

        [Authorize(HealthCarePermissions.Employees.Edit)]
        public virtual async Task<EmployeeDto> UpdateLeaveDaysAsync(Guid id, int leaveDays)
        {
            var employee = await employeeRepository.GetAsync(id);
            employee.LeaveDays = leaveDays;
            await employeeRepository.UpdateAsync(employee);
            return ObjectMapper.Map<Employee, EmployeeDto>(employee);
        }

        [Authorize(HealthCarePermissions.Employees.Edit)]
        public virtual async Task<EmployeeDto> UpdateSalaryAsync(Guid id, decimal salary)
        {
            var employee = await employeeRepository.GetAsync(id);
            employee.Salary = salary;
            await employeeRepository.UpdateAsync(employee);
            return ObjectMapper.Map<Employee, EmployeeDto>(employee);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input)
        {
            var downloadToken = await downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var employees = await employeeRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.DepartmentId, input.TitleId);
            var items = employees.Select(item => new EmployeeExcelDto
            {
                FirstName = item.Employee.FirstName,
                LastName = item.Employee.LastName,
                PhoneNumber = item.Employee.PhoneNumber,
                BirthDate = item.Employee.BirthDate,
                Department = item.Department?.Name ?? string.Empty,
                Title = item.Title?.Name ?? string.Empty,
                LeaveDays = item.Employee.LeaveDays, 
                Salary = item.Employee.Salary
            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Employees.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(HealthCarePermissions.Employees.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> employeeIds) => await employeeRepository.DeleteManyAsync(employeeIds);

        [Authorize(HealthCarePermissions.Employees.Delete)]
        public virtual async Task DeleteAllAsync(GetEmployeesInput input) => await employeeRepository.DeleteAllAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.DepartmentId, input.TitleId);

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await downloadTokenCache.SetAsync(
                token,
                new EmployeeDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}
