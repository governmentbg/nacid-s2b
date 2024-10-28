using AbOcr.Services;
using AutoMapper;
using Hangfire;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Logs.Enums;
using Logs.Services;
using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Dtos.Base;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Suppliers;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Base;
using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.Suppliers.Repositories;
using System.Threading;

namespace Sc.Services.Suppliers
{
    public class SupplierOfferingService : ValidateService<SupplierOffering, SupplierOfferingDto>
    {
        private readonly IMapper mapper;
        private readonly ISupplierOfferingRepository supplierOfferingRepository;
        private readonly ISupplierOfferingCounterRepository supplierOfferingCounterRepository;
        private readonly ISupplierOfferingSolrIndexService soSolrIndexService;
        private readonly SupplierEquipmentService supplierEquipmentService;
        private readonly ScDbContext context;
        private readonly IOcrMetadataService ocrMetadataService;
        private readonly IOcrService ocrService;
        private readonly ErrorLogService errorLogService;
        private readonly PermissionService permissionService;

        public SupplierOfferingService(
            IMapper mapper,
            ISupplierOfferingRepository supplierOfferingRepository,
            ISupplierOfferingCounterRepository supplierOfferingCounterRepository,
            ISupplierOfferingSolrIndexService soSolrIndexService,
            SupplierEquipmentService supplierEquipmentService,
            DomainValidatorService domainValidatorService,
            ScDbContext context,
            IOcrMetadataService ocrMetadataService,
            IOcrService ocrService,
            ErrorLogService errorLogService,
            PermissionService permissionService)
            : base(domainValidatorService)
        {
            this.mapper = mapper;
            this.supplierOfferingRepository = supplierOfferingRepository;
            this.supplierOfferingCounterRepository = supplierOfferingCounterRepository;
            this.soSolrIndexService = soSolrIndexService;
            this.supplierEquipmentService = supplierEquipmentService;
            this.context = context;
            this.ocrMetadataService = ocrMetadataService;
            this.ocrService = ocrService;
            this.errorLogService = errorLogService;
            this.permissionService = permissionService;
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(SupplierOfferingFilterDto filterDto, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await supplierOfferingRepository.GetAll(filterDto, cancellationToken,
                e => e.Include(s => s.Settlement)
                    .Include(s => s.SmartSpecializations.OrderBy(e => e.Type).ThenBy(e => e.SmartSpecialization.ViewOrder).ThenBy(e => e.SmartSpecialization.Name))
                        .ThenInclude(m => m.SmartSpecialization.Root),
                e => e.OrderBy(e => e.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<SupplierOfferingDto> GetDtoById(int id, CancellationToken cancellationToken)
        {
            var supplierOffering = await supplierOfferingRepository.GetById(id, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.Collections));

            return mapper.Map<SupplierOfferingDto>(supplierOffering);
        }

        public async Task<SupplierOfferingDto> GetDtoByIdAndSupplierId(int id, int supplierId, CancellationToken cancellationToken)
        {
            var supplierOffering = await supplierOfferingRepository.GetByIdAndSupplierId(id, supplierId, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.Collections));

            return mapper.Map<SupplierOfferingDto>(supplierOffering);
        }

        public async Task<List<SupplierOfferingDto>> GetDtosBySupplier(int supplierId, SupplierDto supplierDto, CancellationToken cancellationToken)
        {
            var supplierOfferingFilterDto = new SupplierOfferingFilterDto
            {
                GetAllData = true,
                SupplierId = supplierId
            };

            supplierOfferingFilterDto.SetIsActive(permissionService, supplierDto);

            var offeringList = await supplierOfferingRepository.GetList(supplierOfferingFilterDto, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.Collections), e => e.OrderBy(e => e.Name));

            return mapper.Map<List<SupplierOfferingDto>>(offeringList);
        }

        public async Task<SupplierOfferingDto> Create(SupplierDto supplierDto, SupplierOfferingDto supplierOfferingDto, CancellationToken cancellationToken)
        {
            supplierOfferingDto.SupplierId = supplierDto.Id;
            supplierOfferingDto.ValidateProperties(domainValidatorService);
            await CreateValidation(supplierOfferingDto, cancellationToken);

            using var transaction = context.BeginTransaction();

            var newSupplierOffering = mapper.Map<SupplierOffering>(supplierOfferingDto);
            newSupplierOffering.IsActive = true;
            newSupplierOffering.Code = await GenerateCode(supplierDto.Type, supplierDto.Type == SupplierType.Institution ? supplierDto.Institution.LotNumber : supplierDto.Complex.LotNumber, supplierOfferingDto.SmartSpecializations.First(e => e.Type == SupplierOfferingSmartSpecializationType.Primary).SmartSpecialization.CodeNumber, cancellationToken);

            await supplierOfferingRepository.Create(newSupplierOffering);
            await transaction.CommitAsync();

            BackgroundJob.Enqueue(() => OcrFiles(newSupplierOffering));

            foreach (var equipment in newSupplierOffering.SupplierOfferingEquipment)
            {
                await supplierEquipmentService.UpdateSeSolr(equipment.SupplierEquipmentId, cancellationToken);
            }

            return mapper.Map<SupplierOfferingDto>(await supplierOfferingRepository.GetById(newSupplierOffering.Id, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.Collections)));
        }


        public async Task<SupplierOfferingDto> Update(SupplierOffering supplierOfferingForUpdate, SupplierOfferingDto supplierOfferingDto, CancellationToken cancellationToken)
        {
            supplierOfferingDto.ValidateProperties(domainValidatorService);
            await UpdateValidation(supplierOfferingForUpdate, supplierOfferingDto, cancellationToken);

            using var transaction = context.BeginTransaction();

            var oldFiles = supplierOfferingForUpdate.Files?.ToList();
            supplierOfferingForUpdate.Code = UpdateCode(supplierOfferingForUpdate.Code, supplierOfferingDto.SmartSpecializations.First(e => e.Type == SupplierOfferingSmartSpecializationType.Primary).SmartSpecialization.CodeNumber, cancellationToken);

            await supplierOfferingRepository.UpdateFromDto(supplierOfferingForUpdate, supplierOfferingDto);
            await transaction.CommitAsync();

            var newFiles = supplierOfferingDto.Files?.ToList();

            if (oldFiles.Count > 0 || newFiles.Count > 0)
            {
                var filesForDelete = oldFiles.Where(oldFile =>
                            !newFiles.Any(newFile => newFile.Id == oldFile.Id))
                            .ToList();

                foreach (var fileForDelete in filesForDelete)
                {
                    if (await soSolrIndexService.Exists(fileForDelete.Id))
                    {
                        await soSolrIndexService.Delete(new SolrSupplierOffering { Id = fileForDelete.Id });
                    }
                }

                BackgroundJob.Enqueue(() => OcrFiles(supplierOfferingForUpdate));
            }

            foreach (var equipment in supplierOfferingForUpdate.SupplierOfferingEquipment)
            {
                await supplierEquipmentService.UpdateSeSolr(equipment.SupplierEquipmentId, cancellationToken);
            }

            return await GetDtoById(supplierOfferingForUpdate.Id, cancellationToken);
        }

        public async Task Delete(SupplierOffering supplierOfferingForDelete, CancellationToken cancellationToken)
        {
            if (supplierOfferingForDelete.Files != null && supplierOfferingForDelete.Files.Count > 0)
            {
                foreach (var file in supplierOfferingForDelete.Files)
                {
                    if (await soSolrIndexService.Exists(file.Id))
                    {
                        await soSolrIndexService.Delete(new SolrSupplierOffering { Id = file.Id });
                    }

                    context.Set<SupplierOfferingFile>().Remove(file);
                }
            }

            await supplierOfferingRepository.Delete(supplierOfferingForDelete);

            foreach (var equipment in supplierOfferingForDelete.SupplierOfferingEquipment)
            {
                await supplierEquipmentService.UpdateSeSolr(equipment.SupplierEquipmentId, cancellationToken);
            }
        }


        public async Task<List<SolrSupplierOffering>> ForceIndex(int id, CancellationToken cancellationToken)
        {
            var supplierOffering = await supplierOfferingRepository.GetById(id, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.NavProperties));

            if (supplierOffering != null && supplierOffering.Files != null && supplierOffering.Files.Count > 0)
            {
                var indexedSolrSupplierOfferings = new List<SolrSupplierOffering>();

                foreach (var file in supplierOffering.Files)
                {
                    var solrSupplierOffering = new SolrSupplierOffering
                    {
                        Id = file.Id,
                        SupplierId = supplierOffering.SupplierId,
                        SupplierOfferingId = supplierOffering.Id
                    };

                    var ocrMetadata = await ocrMetadataService.GetByFileKey(file.Key, file.DbId);

                    if (!string.IsNullOrWhiteSpace(ocrMetadata?.Result?.Content))
                    {
                        solrSupplierOffering.Content = ocrMetadata.Result.Content;
                    }
                    else
                    {
                        var ocrMetadataResult = await ocrService.ProcessFile(ocrMetadata);
                        solrSupplierOffering.Content = ocrMetadataResult.Result.Content;
                    }

                    await soSolrIndexService.Index(solrSupplierOffering);

                    indexedSolrSupplierOfferings.Add(solrSupplierOffering);
                }

                return indexedSolrSupplierOfferings;
            }
            else
            {
                return null;
            }
        }

        public async Task OcrFiles(SupplierOffering supplierOffering)
        {
            foreach (var file in supplierOffering.Files)
            {
                if (file != null
                && !string.IsNullOrWhiteSpace(file.Name)
                && (file.Name.Trim().ToLower().EndsWith(".docx")
                    || file.Name.Trim().ToLower().EndsWith(".xlsx")
                    || file.Name.Trim().ToLower().EndsWith(".pdf")))
                {
                    var pendingOcrMetadata = await ocrMetadataService.CreateFromFileStorage(file.Key, file.DbId);

                    try
                    {
                        var ocrMetadata = await ocrService.ProcessFile(pendingOcrMetadata);

                        var solrSupplierOffering = new SolrSupplierOffering
                        {
                            Id = file.Id,
                            SupplierId = supplierOffering.SupplierId,
                            Content = ocrMetadata.Result.Content,
                            SupplierOfferingId = supplierOffering.Id
                        };

                        await soSolrIndexService.Index(solrSupplierOffering);
                    }
                    catch (Exception exception)
                    {
                        while (exception.InnerException != null)
                        { exception = exception.InnerException; }

                        var customText = $"SupplierOfferingFileOcr.Key: {pendingOcrMetadata?.File?.Key}";
                        await errorLogService.LogError(exception, ErrorLogType.JobExceptionLog, null, null, null, customText);
                    }
                }
                else if (await soSolrIndexService.Exists(file.Id))
                {
                    await soSolrIndexService.Delete(new SolrSupplierOffering { Id = file.Id });
                }
            }
        }

        public async Task<bool> ChangeIsActive(SupplierOffering supplierOffering, IsActiveDto isActiveDto)
        {
            await supplierOfferingRepository.UpdateFromDto(supplierOffering, isActiveDto);

            return supplierOffering.IsActive;
        }

        private async Task<string> GenerateCode(SupplierType supplierType, int lotNumber, string smartSpecializationCodeNumber, CancellationToken cancellationToken)
        {
            var supplierOfferingCounter = await supplierOfferingCounterRepository.GetCounter(cancellationToken);
            supplierOfferingCounter.Counter++;
            await supplierOfferingCounterRepository.SaveEntityChanges(supplierOfferingCounter);

            var supplierNumber = $"{(supplierType == SupplierType.Institution ? "1" : "2")}{lotNumber.ToString().PadLeft(4, '0')}";
            var counter = supplierOfferingCounter.Counter.ToString().PadLeft(4, '0');
            var code = $"{supplierNumber}-{counter}-{smartSpecializationCodeNumber}";

            return code;
        }

        private string UpdateCode(string oldCode, string smartSpecializationCodeNumber, CancellationToken cancellationToken)
        {
            var code = oldCode.Remove(oldCode.Length - 3) + smartSpecializationCodeNumber;
            return code;
        }

        #region IValidationService
        protected async override Task CreateValidation(SupplierOfferingDto supplierOfferingDto, CancellationToken cancellationToken)
        {
            var hasSmartSpecializationsDublicates = supplierOfferingDto.SmartSpecializations
                .GroupBy(e => new { e.SupplierOfferingId, e.SmartSpecializationId })
                .Any(e => e.Count() > 1);

            if (hasSmartSpecializationsDublicates)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_SmartSpecializations_Exists);
            }

            await Task.CompletedTask;
        }

        protected async override Task UpdateValidation(SupplierOffering supplierOffering, SupplierOfferingDto dto, CancellationToken cancellationToken)
        {
            var hasSmartSpecializationsDublicates = dto.SmartSpecializations
                .GroupBy(e => new { e.SupplierOfferingId, e.SmartSpecializationId })
                .Any(e => e.Count() > 1);

            if (hasSmartSpecializationsDublicates)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_SmartSpecializations_Exists);
            }

            if (dto.SmartSpecializations.Any(e => e.SupplierOfferingId != supplierOffering.Id && e.SupplierOfferingId != 0))
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_SmartSpecializations_CannotChangeSupplierOfferingId);
            }

            await Task.CompletedTask;
        }
        #endregion
    }
}
