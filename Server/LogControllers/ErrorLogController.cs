using Infrastructure.Attributes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Logs.Entities;
using Logs.FilterDtos;
using Logs.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;

namespace Server.LogControllers
{
    [ApiController]
    [Route("api/logs/errors")]
    public class ErrorLogController : ControllerBase
    {
        private readonly ErrorLogSearchService errorLogSearchService;
        private readonly PermissionService permissionService;

        public ErrorLogController(
            ErrorLogSearchService errorLogSearchService,
            PermissionService permissionService
        )
        {
            this.errorLogSearchService = errorLogSearchService;
            this.permissionService = permissionService;
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SearchResultDto<ErrorLog>>> GetAll([FromBody] ErrorLogFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SystemLogReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await errorLogSearchService.GetAll(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost("count")]
        public async Task<ActionResult<int>> GetCount([FromBody] ErrorLogFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SystemLogReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await errorLogSearchService.GetCount(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ErrorLog>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SystemLogReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await errorLogSearchService.GetById(id, cancellationToken));
        }
    }
}
