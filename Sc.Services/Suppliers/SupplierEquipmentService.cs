using AbOcr.Services;
using AutoMapper;
using Hangfire;
using Infrastructure.DomainValidation;
using Logs.Enums;
using Logs.Services;
using Sc.Models;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Solr.Suppliers.Entities;
using Sc.Solr.Suppliers.Repositories;
using System.Data.Entity;

namespace Sc.Services.Suppliers
{
    public class SupplierEquipmentService
    {
        private readonly IMapper mapper;
        private readonly ISupplierEquipmentRepository supplierEquipmentRepository;
        private readonly ISupplierEquipmentSolrIndexService seSolrIndexService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly IOcrMetadataService ocrMetadataService;
        private readonly IOcrService ocrService;
        private readonly ErrorLogService errorLogService;

        public SupplierEquipmentService(
            IMapper mapper,
            ISupplierEquipmentRepository supplierEquipmentRepository,
            ISupplierEquipmentSolrIndexService seSolrIndexService,
            DomainValidatorService domainValidatorService,
            IOcrMetadataService ocrMetadataService,
            IOcrService ocrService,
            ErrorLogService errorLogService)
        {
            this.mapper = mapper;
            this.supplierEquipmentRepository = supplierEquipmentRepository;
            this.seSolrIndexService = seSolrIndexService;
            this.domainValidatorService = domainValidatorService;
            this.ocrMetadataService = ocrMetadataService;
            this.ocrService = ocrService;
            this.errorLogService = errorLogService;
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(SupplierEquipmentFilterDto filterDto, IncludeType includeType, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await supplierEquipmentRepository.GetAll(filterDto, cancellationToken, supplierEquipmentRepository.ConstructInclude(includeType), e => e.OrderBy(e => e.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<SupplierEquipmentDto> GetDtoById(int id, CancellationToken cancellationToken)
        {
            var supplierEquipment = await supplierEquipmentRepository.GetById(id, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.All));

            return mapper.Map<SupplierEquipmentDto>(supplierEquipment);
        }

        public async Task<List<SupplierEquipmentDto>> GetBySupplierDtos(int supplierId, CancellationToken cancellationToken)
        {
            var supplierEquipmentFilterDto = new SupplierEquipmentFilterDto
            {
                GetAllData = true,
                SupplierId = supplierId
            };

            var equipmentList = await supplierEquipmentRepository.GetList(supplierEquipmentFilterDto, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.Collections), e => e.OrderBy(e => e.Name));

            return mapper.Map<List<SupplierEquipmentDto>>(equipmentList);
        }

        public async Task<SupplierEquipmentDto> Create(int supplierId, SupplierEquipmentDto supplierEquipmentDto, CancellationToken cancellationToken)
        {
            supplierEquipmentDto.SupplierId = supplierId;
            supplierEquipmentDto.ValidateProperties(domainValidatorService);

            var newSupplierEquipment = mapper.Map<SupplierEquipment>(supplierEquipmentDto);

            await supplierEquipmentRepository.Create(newSupplierEquipment);

            BackgroundJob.Enqueue(() => OcrFile(newSupplierEquipment));

            return mapper.Map<SupplierEquipmentDto>(await supplierEquipmentRepository.GetById(newSupplierEquipment.Id, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.NavProperties)));
        }

        public async Task<SupplierEquipmentDto> Update(SupplierEquipment supplierEquipmentForUpdate, SupplierEquipmentDto supplierEquipmentDto, CancellationToken cancellationToken)
        {
            supplierEquipmentDto.ValidateProperties(domainValidatorService);

            var oldFile = supplierEquipmentForUpdate.File;
            var newFile = supplierEquipmentDto.File;

            await supplierEquipmentRepository.UpdateFromDto(supplierEquipmentForUpdate, supplierEquipmentDto);

            if (oldFile != null && newFile == null && await seSolrIndexService.Exists(supplierEquipmentForUpdate.Id))
            {
                await seSolrIndexService.Delete(new SolrSupplierEquipment { Id = supplierEquipmentForUpdate.Id });
            }
            else
            {
                BackgroundJob.Enqueue(() => OcrFile(supplierEquipmentForUpdate));
            }

            return await GetDtoById(supplierEquipmentForUpdate.Id, cancellationToken);
        }

        public async Task Delete(SupplierEquipment supplierEquipmentForDelete, CancellationToken cancellationToken)
        {
            if (supplierEquipmentForDelete.File != null && await seSolrIndexService.Exists(supplierEquipmentForDelete.Id)) {
                    await seSolrIndexService.Delete(new SolrSupplierEquipment { Id = supplierEquipmentForDelete.Id });
            }

            await supplierEquipmentRepository.Delete(supplierEquipmentForDelete);
        }

        public async Task<SolrSupplierEquipment> ForceIndex(int id, CancellationToken cancellationToken)
        {
            var supplierEquipment = await supplierEquipmentRepository.GetById(id, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.Collections));

            if (supplierEquipment != null && supplierEquipment.File != null)
            {
                var solrSupplierEquipment = new SolrSupplierEquipment
                {
                    Id = supplierEquipment.Id,
                    SupplierId = supplierEquipment.SupplierId,
                    SupplierOfferingIds = supplierEquipment.SupplierOfferingEquipment.Select(s => s.SupplierOfferingId).ToList()
                };

                var ocrMetadata = await ocrMetadataService.GetByFileKey(supplierEquipment.File.Key, supplierEquipment.File.DbId);

                if (!string.IsNullOrWhiteSpace(ocrMetadata?.Result?.Content))
                {
                    solrSupplierEquipment.Content = ocrMetadata.Result.Content;
                }
                else
                {
                    var ocrMetadataResult = await ocrService.ProcessFile(ocrMetadata);
                    solrSupplierEquipment.Content = ocrMetadataResult.Result.Content;
                }

                await seSolrIndexService.Index(solrSupplierEquipment);

                return solrSupplierEquipment;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateSeSolr(int supplierEquipmentId, CancellationToken cancellationToken)
        {
            var supplierEquipment = await supplierEquipmentRepository.GetById(supplierEquipmentId, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.Collections));

            if (supplierEquipment.File != null
            && !string.IsNullOrWhiteSpace(supplierEquipment.File.Name)
            && (supplierEquipment.File.Name.Trim().ToLower().EndsWith(".docx")
                || supplierEquipment.File.Name.Trim().ToLower().EndsWith(".xlsx")
                || supplierEquipment.File.Name.Trim().ToLower().EndsWith(".pdf")))
            {
                var ocrMetadata = await ocrMetadataService.GetByFileKey(supplierEquipment.File.Key, supplierEquipment.File.DbId);

                if (ocrMetadata != null)
                {
                    var solrSupplierEquipment = new SolrSupplierEquipment
                    {
                        Id = supplierEquipment.Id,
                        SupplierId = supplierEquipment.SupplierId,
                        Content = ocrMetadata.Result.Content,
                        SupplierOfferingIds = supplierEquipment.SupplierOfferingEquipment.Select(e => e.SupplierOfferingId).ToList()
                    };

                    await seSolrIndexService.Index(solrSupplierEquipment);
                }
            }
        }

        public async Task OcrFile(SupplierEquipment supplierEquipment)
        {
            if (supplierEquipment.File != null
            && !string.IsNullOrWhiteSpace(supplierEquipment.File.Name)
            && (supplierEquipment.File.Name.Trim().ToLower().EndsWith(".docx")
                || supplierEquipment.File.Name.Trim().ToLower().EndsWith(".xlsx")
                || supplierEquipment.File.Name.Trim().ToLower().EndsWith(".pdf")))
            {
                var pendingOcrMetadata = await ocrMetadataService.CreateFromFileStorage(supplierEquipment.File.Key, supplierEquipment.File.DbId);

                try
                {
                    var ocrMetadata = await ocrService.ProcessFile(pendingOcrMetadata);

                    var solrSupplierEquipment = new SolrSupplierEquipment
                    {
                        Id = supplierEquipment.Id,
                        SupplierId = supplierEquipment.SupplierId,
                        Content = ocrMetadata.Result.Content,
                        SupplierOfferingIds = supplierEquipment.SupplierOfferingEquipment.Select(e => e.SupplierOfferingId).ToList()
                    };

                    await seSolrIndexService.Index(solrSupplierEquipment);
                }
                catch (Exception exception)
                {
                    while (exception.InnerException != null)
                    { exception = exception.InnerException; }

                    var customText = $"SupplierEquipmentFileOcr.Key: {pendingOcrMetadata?.File?.Key}";
                    await errorLogService.LogError(exception, ErrorLogType.JobExceptionLog, null, null, null, customText);
                }
            }
            else if (await seSolrIndexService.Exists(supplierEquipment.Id))
            {
                await seSolrIndexService.Delete(new SolrSupplierEquipment { Id = supplierEquipment.Id });
            }
        }
    }
}
