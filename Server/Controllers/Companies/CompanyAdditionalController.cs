using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Companies;
using Sc.Models.Enums.Common;
using Sc.Repositories.Companies;
using Sc.Services.Companies;

namespace Server.Controllers.Companies
{
    [ApiController]
    [Route("api/companies/{id:int}/additionals")]
    public class CompanyAdditionalController : ControllerBase
    {
        private readonly CompanyAdditionalService companyAdditionalService;
        private readonly ICompanyAdditionalRepository companyAdditionalRepository;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public CompanyAdditionalController(
            CompanyAdditionalService companyAdditionalService,
            ICompanyAdditionalRepository companyAdditionalRepository,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.companyAdditionalService = companyAdditionalService;
            this.companyAdditionalRepository = companyAdditionalRepository;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [Authorize, ScClient]
        [HttpGet]
        public async Task<ActionResult<CompanyAdditionalDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.CompanyAdditionalReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, id), (OrganizationalUnitConstants.NacidAlias, null), (OrganizationalUnitConstants.PniiditAlias, null) });

            return Ok(await companyAdditionalService.GetDtoById(id, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<CompanyAdditionalDto>> Create([FromRoute] int id, [FromBody] CompanyAdditionalDto companyAdditionalDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            permissionService.VerifyUnitPermissionException(PermissionConstants.CompanyAdditionalCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, id), (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await companyAdditionalService.Create(id, companyAdditionalDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut]
        public async Task<ActionResult<CompanyAdditionalDto>> Update([FromRoute] int id, [FromBody] CompanyAdditionalDto companyAdditionalDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var companyAdditionalForUpdate = await companyAdditionalRepository.GetById(companyAdditionalDto.Id, cancellationToken, companyAdditionalRepository.ConstructInclude(IncludeType.None));

            if (id != companyAdditionalForUpdate.Id)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.CompanyAdditional_Entity_InvalidId);
            }

            permissionService.VerifyUnitPermissionException(PermissionConstants.CompanyAdditionalWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, id), (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await companyAdditionalService.Update(companyAdditionalForUpdate, companyAdditionalDto, cancellationToken));
        }
    }
}
