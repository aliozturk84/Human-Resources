using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp;
using Pusula.Training.HealthCare.HRs;

namespace Pusula.Training.HealthCare.Controllers.HRs
{
    [RemoteService]
    [Area("app")]
    [ControllerName("HRs")]
    [Route("api/app/hrs")]
    public class HRController(IHRsAppService hrsAppService) : HealthCareController, IHRsAppService
    {
        [HttpGet]
        public virtual Task<PagedResultDto<HRWithNavigationPropertiesDto>> GetListAsync(GetHRsInput input) => hrsAppService.GetListAsync(input);

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<HRWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) => hrsAppService.GetWithNavigationPropertiesAsync(id);

        [HttpGet]
        [Route("{id}")]
        public virtual Task<HRDto> GetAsync(Guid id) => hrsAppService.GetAsync(id);

        [HttpGet]
        [Route("title-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input) => hrsAppService.GetTitleLookupAsync(input);

        [HttpGet]
        [Route("department-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input) => hrsAppService.GetDepartmentLookupAsync(input);

        [HttpPost]
        public virtual Task<HRDto> CreateAsync(HRCreateDto input) => hrsAppService.CreateAsync(input);

        [HttpPut]
        public virtual Task<HRDto> UpdateAsync(HRUpdateDto input) => hrsAppService.UpdateAsync(input);

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id) => hrsAppService.DeleteAsync(id);

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(HRExcelDownloadDto input) => hrsAppService.GetListAsExcelFileAsync(input);

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync() => hrsAppService.GetDownloadTokenAsync();

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> hrIds) => hrsAppService.DeleteByIdsAsync(hrIds);

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetHRsInput input) => hrsAppService.DeleteAllAsync(input);
    }
}
