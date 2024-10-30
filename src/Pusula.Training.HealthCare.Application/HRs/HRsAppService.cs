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
using System.Linq.Dynamic.Core;
using MiniExcelLibs;

namespace Pusula.Training.HealthCare.HRs
{
    [RemoteService(IsEnabled = false)]
    [Authorize(HealthCarePermissions.HRs.Default)]
    public class HRsAppService(
        IHRRepository hrRepository,
        HRManager hrManager,
        IDistributedCache<HRDownloadTokenCacheItem, string> downloadTokenCache,
        ITitleRepository titleRepository,
        IDepartmentRepository departmentRepository,
        IDistributedEventBus distributedEventBus) : HealthCareAppService, IHRsAppService
    {

        public virtual async Task<PagedResultDto<HRWithNavigationPropertiesDto>> GetListAsync(GetHRsInput input)
        {
            var totalCount = await hrRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId, input.Sorting, input.MaxResultCount, input.SkipCount);
            var items = await hrRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<HRWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<HRWithNavigationProperties>, List<HRWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<HRWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            var hr = await hrRepository.GetWithNavigationPropertiesAsync(id);
            await distributedEventBus.PublishAsync(new HRTitleAndDepartmentEto { Department = hr.Department.Name, Title = hr.Title.Name });
            return ObjectMapper.Map<HRWithNavigationProperties, HRWithNavigationPropertiesDto>(hr);
        }


        public virtual async Task<HRDto> GetAsync(Guid id) => ObjectMapper.Map<HR, HRDto>(
                await hrRepository.GetAsync(id));

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


        [Authorize(HealthCarePermissions.HRs.Delete)]
        public virtual async Task DeleteAsync(Guid id) => await hrRepository.DeleteAsync(id);

        [Authorize(HealthCarePermissions.HRs.Create)]
        public virtual async Task<HRDto> CreateAsync(HRCreateDto input) => ObjectMapper.Map<HR, HRDto>(
            await hrManager.CreateAsync(input.TitleId, input.DepartmentId, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDate, input.Gender));

        [Authorize(HealthCarePermissions.HRs.Edit)]
        public virtual async Task<HRDto> UpdateAsync(HRUpdateDto input) => ObjectMapper.Map<HR, HRDto>(
            await hrManager.UpdateAsync(input.Id, input.TitleId, input.DepartmentId, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDate, input.Gender));

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(HRExcelDownloadDto input)
        {
            var downloadToken = await downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var hrs = await hrRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId);
            var items = hrs.Select(item => new HRExcelDto
            {
                FirstName = item.HR.FirstName,
                LastName = item.HR.LastName,
                PhoneNumber = item.HR.PhoneNumber,
                BirthDate = item.HR.BirthDate,
                Gender = item.HR.Gender,
                Department = item.Department?.Name ?? string.Empty,
                Title = item.Title?.Name ?? string.Empty
            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "HRs.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(HealthCarePermissions.HRs.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> hrIds) => await hrRepository.DeleteManyAsync(hrIds);

        [Authorize(HealthCarePermissions.HRs.Delete)]
        public virtual async Task DeleteAllAsync(GetHRsInput input) => await hrRepository.DeleteAllAsync(input.FilterText, input.FirstName, input.LastName, input.PhoneNumber, input.BirthDateMin, input.BirthDateMax, input.Gender, input.DepartmentId, input.TitleId);

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await downloadTokenCache.SetAsync(
                token,
                new HRDownloadTokenCacheItem { Token = token },
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
