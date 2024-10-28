using Infrastructure.Attributes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Companies;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Companies;
using Sc.Repositories.Companies;
using Sc.Services.Companies;

namespace Server.Controllers.Companies
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService companyService;
        private readonly ICompanyRepository companyRepository;
        private readonly PermissionService permissionService;

        public CompanyController(
            CompanyService companyService,
            ICompanyRepository companyRepository,
            PermissionService permissionService
            )
        {
            this.companyService = companyService;
            this.companyRepository = companyRepository;
            this.permissionService = permissionService;
        }

        [Authorize, ScClient]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            //permissionService.VerifyUnitPermissionException(null, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, id), (OrganizationalUnitConstants.NacidAlias, null) });
            
            return Ok(await companyService.GetById(id, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SearchResultDto<CompanyDto>>> GetSearchResultDto([FromBody] CompanyFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(null, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null), (OrganizationalUnitConstants.PniiditAlias, null) });

            return Ok(await companyService.GetSearchResultDto<CompanyDto>(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut("isActive")]
        public async Task<ActionResult<bool>> ChangeIsActive([FromBody] IsActiveDto isActiveDto, CancellationToken cancellationToken)
        {
            var companyForUpdate = await companyRepository.GetById(isActiveDto.Id, cancellationToken, companyRepository.ConstructInclude(IncludeType.None));

            permissionService.VerifyUnitPermissionException(PermissionConstants.CompanyAdditionalWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, companyForUpdate.Id), (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await companyService.ChangeIsActive(companyForUpdate, isActiveDto));
        }
    }
}
