using FilesStorageNetCore.FormDataHelpers;
using FileStorageNetCore;
using FileStorageNetCore.Api;
using FileStorageNetCore.Models;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.FileStorageControllers
{
    [Route("api/FileStorage")]
    public class NacidScFileStorage : FileStorageController
    {
        private readonly DomainValidatorService domainValidatorService;

        public NacidScFileStorage(
            BlobStorageService blobStorageService,
            DomainValidatorService domainValidatorService
            )
            : base(blobStorageService)
        {
            this.domainValidatorService = domainValidatorService;
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        public override async Task<AttachedFile> PostFile(int? dbId)
        {
            return await base.PostFile(dbId);
        }

        [HttpGet]
        public override async Task<IActionResult> Get(Guid key, int dbId, string fileName = null, string mimeType = null)
        {
            return await base.Get(key, dbId, fileName, mimeType);
        }

        [Authorize, ScClient]
        [HttpDelete]
        public override IActionResult DeleteFile(Guid key, int? dbId)
        {
            domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);

            return Ok(base.DeleteFile(key, dbId.Value));
        }
    }
}
