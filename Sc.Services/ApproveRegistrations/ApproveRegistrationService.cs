using AutoMapper;
using Infrastructure;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Integrations.SsoIntegration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sc.Models;
using Sc.Models.Dtos.ApproveRegistrations;
using Sc.Models.Dtos.ApproveRegistrations.Search;
using Sc.Models.Dtos.Auth;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.State;
using Sc.Models.Enums.Suppliers;
using Sc.Models.FilterDtos.ApproveRegistrations;
using Sc.Repositories.ApproveRegistrations;
using Sc.Repositories.Helpers;
using Sc.Repositories.Suppliers;
using Sc.Services.Auth;
using Sc.Services.Emails;
using Sc.Services.Suppliers;

namespace Sc.Services.ApproveRegistrations
{
    public class ApproveRegistrationService
    {
        private readonly ScDbContext context;
        private readonly IApproveRegistrationRepository approveRegistrationRepository;
        private readonly IApproveRegistrationHistoryRepository approveRegistrationHistoryRepository;
        private readonly ISupplierRepresentativeRepository supplierRepresentativeRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly SupplierService supplierService;
        private readonly SsoIntegrationService ssoIntegrationService;
        private readonly EmailGenerationService emailGenerationService;
        private readonly AuthService authService;
        private readonly UserContext userContext;
        private readonly DomainValidatorService domainValidatorService;
        private readonly IMapper mapper;

        public ApproveRegistrationService(
            ScDbContext context,
            IApproveRegistrationRepository approveRegistrationRepository,
            IApproveRegistrationHistoryRepository approveRegistrationHistoryRepository,
            ISupplierRepresentativeRepository supplierRepresentativeRepository,
            ISupplierRepository supplierRepository,
            SupplierService supplierService,
            SsoIntegrationService ssoIntegrationService,
            EmailGenerationService emailGenerationService,
            AuthService authService,
            UserContext userContext,
            DomainValidatorService domainValidatorService,
            IMapper mapper
            )
        {
            this.context = context;
            this.approveRegistrationRepository = approveRegistrationRepository;
            this.approveRegistrationHistoryRepository = approveRegistrationHistoryRepository;
            this.supplierRepresentativeRepository = supplierRepresentativeRepository;
            this.supplierRepository = supplierRepository;
            this.supplierService = supplierService;
            this.ssoIntegrationService = ssoIntegrationService;
            this.emailGenerationService = emailGenerationService;
            this.authService = authService;
            this.userContext = userContext;
            this.domainValidatorService = domainValidatorService;
            this.mapper = mapper;
        }

        public async Task<ApproveRegistrationSearchDto> GetSearchDtoById(int id, CancellationToken cancellationToken)
        {
            var approveRegistration = await approveRegistrationRepository.GetById(id, cancellationToken, approveRegistrationRepository.ConstructInclude(IncludeType.All));
            return mapper.Map<ApproveRegistrationSearchDto>(approveRegistration);
        }

        public async Task<SupplierDto> ApplyForRegistration(SignUpDeclarationDto signUpDeclarationDto, CancellationToken cancellationToken)
        {
            if (signUpDeclarationDto?.File == null)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_File_Required);
            }

            var ssoSignUpDto = await authService.ConstructSsoSignUpDto(signUpDeclarationDto.SignUp, cancellationToken);

            var userValidateInfo = await ssoIntegrationService.ValidateSignUp(ssoSignUpDto);
            signUpDeclarationDto.SignUp.SsoUserValidateSignUpInfo = userValidateInfo;
            signUpDeclarationDto.SignUp.RecaptchaToken = null;

            using var transaction = context.BeginTransaction();

            var createdSupplierDto = await supplierService.Create(mapper.Map<SupplierDto>(signUpDeclarationDto.SignUp), cancellationToken);

            var newApproveRegistration = new ApproveRegistration
            {
                CreateDate = DateTime.Now,
                SupplierId = createdSupplierDto.Id,
                State = ApproveRegistrationState.Draft,
                JsonSignUpDto = JsonConvert.SerializeObject(signUpDeclarationDto.SignUp, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                })
            };

            if (signUpDeclarationDto.File != null)
            {
                newApproveRegistration.File = new ApproveRegistrationFile
                {
                    DbId = signUpDeclarationDto.File.DbId,
                    Hash = signUpDeclarationDto.File.Hash,
                    Key = signUpDeclarationDto.File.Key,
                    MimeType = signUpDeclarationDto.File.MimeType,
                    Name = signUpDeclarationDto.File.Name,
                    Size = signUpDeclarationDto.File.Size
                };
            }

            await approveRegistrationRepository.Create(newApproveRegistration);
            await emailGenerationService.SupplierRegistrationComposeEmail(signUpDeclarationDto.SignUp);

            await transaction.CommitAsync();

            return createdSupplierDto;
        }

        public async Task<SearchResultDto<ApproveRegistrationSearchDto>> GetSearchResultDto(ApproveRegistrationFilterDto filterDto, IncludeType includeType, CancellationToken cancellationToken)
        {
            filterDto.GetAllData = true;

            var result = await approveRegistrationRepository.GetList(filterDto, cancellationToken, approveRegistrationRepository.ConstructInclude(includeType), e => e.OrderByDescending(e => e.CreateDate));

            var deserializedResult = mapper.Map<List<ApproveRegistrationSearchDto>>(result);

            if (!string.IsNullOrWhiteSpace(filterDto.AuthorizedRepresentativeUsername))
            {
                deserializedResult = deserializedResult
                    .Where(e => e.SignUpDto.User.UserName.Trim().ToLower() == filterDto.AuthorizedRepresentativeUsername.Trim().ToLower()
                        || e.ApproveRegistrationHistories.Any(s => s.SignUpDto.User.UserName.Trim().ToLower() == filterDto.AuthorizedRepresentativeUsername.Trim().ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(filterDto.AuthorizedRepresentativeFullname))
            {
                deserializedResult = deserializedResult
                    .Where(e => e.SignUpDto.User.UserInfo.FullName.Trim().ToLower().Contains(filterDto.AuthorizedRepresentativeFullname.Trim().ToLower())
                        || e.ApproveRegistrationHistories.Any(s => s.SignUpDto.User.UserInfo.FullName.Trim().ToLower().Contains(filterDto.AuthorizedRepresentativeFullname.Trim().ToLower())))
                    .ToList();
            }

            if (filterDto.InstitutionId.HasValue)
            {
                deserializedResult = deserializedResult.Where(e => e.SignUpDto.Institution != null && e.SignUpDto.Institution.Id == filterDto.InstitutionId).ToList();
            }

            if (filterDto.ComplexId.HasValue)
            {
                deserializedResult = deserializedResult.Where(e => e.SignUpDto.Complex != null && e.SignUpDto.Complex.Id == filterDto.ComplexId).ToList();
            }

            var searchResult = new SearchResultDto<ApproveRegistrationSearchDto>
            {
                Result = deserializedResult,
                TotalCount = deserializedResult.Count
            };

            return searchResult;
        }

        public async Task<SupplierOfferingCountDto> GetSupplierOfferingCount(ApproveRegistrationFilterDto filter, CancellationToken cancellationToken)
        {
            filter.GetAllData = true;

            var filteredRegistrations = await approveRegistrationRepository.GetList(
                filter,
                cancellationToken,
                approveRegistrationRepository.ConstructInclude(IncludeType.NavProperties)
            );

            var deserializedResult = mapper.Map<List<ApproveRegistrationSearchDto>>(filteredRegistrations);

            if (!string.IsNullOrWhiteSpace(filter.AuthorizedRepresentativeUsername))
            {
                deserializedResult = deserializedResult
                    .Where(e => e.SignUpDto.User.UserName.Trim().ToLower() == filter.AuthorizedRepresentativeUsername.Trim().ToLower()
                        || e.ApproveRegistrationHistories.Any(s => s.SignUpDto.User.UserName.Trim().ToLower() == filter.AuthorizedRepresentativeUsername.Trim().ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.AuthorizedRepresentativeFullname))
            {
                deserializedResult = deserializedResult
                    .Where(e => e.SignUpDto.User.UserInfo.FullName.Trim().ToLower().Contains(filter.AuthorizedRepresentativeFullname.Trim().ToLower())
                        || e.ApproveRegistrationHistories.Any(s => s.SignUpDto.User.UserInfo.FullName.Trim().ToLower().Contains(filter.AuthorizedRepresentativeFullname.Trim().ToLower())))
                    .ToList();
            }

            if (filter.InstitutionId.HasValue)
            {
                deserializedResult = deserializedResult.Where(e => e.SignUpDto.Institution != null && e.SignUpDto.Institution.Id == filter.InstitutionId).ToList();
            }

            if (filter.ComplexId.HasValue)
            {
                deserializedResult = deserializedResult.Where(e => e.SignUpDto.Complex != null && e.SignUpDto.Complex.Id == filter.ComplexId).ToList();
            }

            var suppliers = deserializedResult
                .Select(r => r.Supplier)
                .Distinct()
                .ToList();

            var supplierOfferingCount = suppliers
                .Where(e => e != null)
                .SelectMany(i => i.SupplierOfferings)
                .Distinct()
                .Count();

            return new SupplierOfferingCountDto
            {
                SupplierCount = suppliers.Count,
                SupplierOfferingCount = supplierOfferingCount
            };
        }

        public async Task<ApproveRegistrationSearchDto> DeclineRegistration(DeclineRegistrationDto declineRegistrationDto, CancellationToken cancellationToken)
        {
            var registration = await approveRegistrationRepository.GetById(declineRegistrationDto.RegistrationId, cancellationToken);

            if (registration.State != ApproveRegistrationState.Draft)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_WrongState);
            }

            using var transaction = context.BeginTransaction();

            registration.FinishDate = DateTime.Now;
            registration.State = ApproveRegistrationState.Declined;
            registration.AdministratedUserId = userContext.UserId;
            registration.AdministratedUsername = userContext.UserName;
            registration.DeclinedNote = declineRegistrationDto.DeclinedNote;

            await approveRegistrationRepository.SaveEntityChanges(registration);

            var supplierForDelete = await supplierRepository.GetById(registration.SupplierId.Value, cancellationToken, supplierRepository.ConstructInclude(IncludeType.All));

            await supplierService.Delete(supplierForDelete);

            await transaction.CommitAsync();

            return await GetSearchDtoById(registration.Id, cancellationToken);
        }

        public async Task<ApproveRegistrationSearchDto> ApproveRegistration(ApproveRegistrationDto approveRegistrationDto, CancellationToken cancellationToken)
        {
            var registration = await approveRegistrationRepository.GetById(approveRegistrationDto.RegistrationId, cancellationToken);

            if (registration.State != ApproveRegistrationState.Draft)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_WrongState);
            }

            var signUpDto = JsonConvert.DeserializeObject<SignUpDto>(registration.JsonSignUpDto);
            await authService.SignUp(signUpDto, registration.SupplierId, cancellationToken);

            registration.FinishDate = DateTime.Now;
            registration.State = ApproveRegistrationState.Approved;
            registration.AdministratedUserId = userContext.UserId;
            registration.AdministratedUsername = userContext.UserName;

            await approveRegistrationRepository.SaveEntityChanges(registration);

            return await GetSearchDtoById(registration.Id, cancellationToken);
        }

        public async Task SignUpSupplier(ApproveRegistrationSearchDto approveRegistrationDto, CancellationToken cancellationToken)
        {
            if (approveRegistrationDto?.File == null)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_File_Required);
            }

            using var transaction = context.BeginTransaction();

            var registration = await approveRegistrationRepository.GetById(approveRegistrationDto.Id, cancellationToken, approveRegistrationRepository.ConstructInclude(IncludeType.NavProperties));

            await authService.SignUpSupplierUpdate(approveRegistrationDto.SignUpDto, registration.SupplierId.Value, cancellationToken);

            var newHistory = new ApproveRegistrationHistory
            {
                ApproveRegistrationId = approveRegistrationDto.Id
            };
            EntityHelper.CloneProperties(registration, newHistory);
            await approveRegistrationHistoryRepository.Create(newHistory);

            if (approveRegistrationDto.File != null)
            {
                if (registration.File != null)
                {
                    registration.File.DbId = approveRegistrationDto.File.DbId;
                    registration.File.Hash = approveRegistrationDto.File.Hash;
                    registration.File.Key = approveRegistrationDto.File.Key;
                    registration.File.MimeType = approveRegistrationDto.File.MimeType;
                    registration.File.Name = approveRegistrationDto.File.Name;
                    registration.File.Size = approveRegistrationDto.File.Size;

                    context.Entry(registration.File).State = EntityState.Modified;
                }
                else
                {
                    registration.File = new ApproveRegistrationFile
                    {
                        DbId = approveRegistrationDto.File.DbId,
                        Hash = approveRegistrationDto.File.Hash,
                        Key = approveRegistrationDto.File.Key,
                        MimeType = approveRegistrationDto.File.MimeType,
                        Name = approveRegistrationDto.File.Name,
                        Size = approveRegistrationDto.File.Size
                    };

                    context.Entry(registration.File).State = EntityState.Added;
                }
            }

            registration.CreateDate = DateTime.Now;
            registration.FinishDate = DateTime.Now;
            registration.State = ApproveRegistrationState.Approved;
            registration.AdministratedUserId = userContext.UserId;
            registration.AdministratedUsername = userContext.UserName;
            registration.JsonSignUpDto = JsonConvert.SerializeObject(approveRegistrationDto.SignUpDto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
    
            await approveRegistrationRepository.SaveEntityChanges(registration);

            await transaction.CommitAsync();
        }

        public async Task UpdateRepresentativeInfo(ApproveRegistrationSearchDto approveRegistrationDto, CancellationToken cancellationToken)
        {
            if (approveRegistrationDto?.File == null)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_File_Required);
            }

            using var transaction = context.BeginTransaction();

            var registration = await approveRegistrationRepository.GetById(approveRegistrationDto.Id, cancellationToken, approveRegistrationRepository.ConstructInclude(IncludeType.NavProperties));

            var newHistory = new ApproveRegistrationHistory
            {
                ApproveRegistrationId = approveRegistrationDto.Id
            };
            EntityHelper.CloneProperties(registration, newHistory);
            await approveRegistrationHistoryRepository.Create(newHistory);

            var userInfo = approveRegistrationDto.SignUpDto.User.UserInfo;
            userInfo.FullName = $"{userInfo.FirstName.Trim()} {userInfo.MiddleName?.Trim()} {userInfo.LastName.Trim()}";

            if (approveRegistrationDto.File != null)
            {
                if (registration.File != null)
                {
                    registration.File.DbId = approveRegistrationDto.File.DbId;
                    registration.File.Hash = approveRegistrationDto.File.Hash;
                    registration.File.Key = approveRegistrationDto.File.Key;
                    registration.File.MimeType = approveRegistrationDto.File.MimeType;
                    registration.File.Name = approveRegistrationDto.File.Name;
                    registration.File.Size = approveRegistrationDto.File.Size;

                    context.Entry(registration.File).State = EntityState.Modified;
                }
                else
                {
                    registration.File = new ApproveRegistrationFile
                    {
                        DbId = approveRegistrationDto.File.DbId,
                        Hash = approveRegistrationDto.File.Hash,
                        Key = approveRegistrationDto.File.Key,
                        MimeType = approveRegistrationDto.File.MimeType,
                        Name = approveRegistrationDto.File.Name,
                        Size = approveRegistrationDto.File.Size
                    };

                    context.Entry(registration.File).State = EntityState.Added;
                }
            }

            registration.CreateDate = DateTime.Now;
            registration.FinishDate = DateTime.Now;
            registration.State = ApproveRegistrationState.Approved;
            registration.AdministratedUserId = userContext.UserId;
            registration.AdministratedUsername = userContext.UserName;
            registration.JsonSignUpDto = JsonConvert.SerializeObject(approveRegistrationDto.SignUpDto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await approveRegistrationRepository.SaveEntityChanges(registration);

            var supplierRepresentativeForUpdate = await supplierRepresentativeRepository.GetById(registration.SupplierId.Value, cancellationToken, supplierRepresentativeRepository.ConstructInclude(IncludeType.None));
            supplierRepresentativeForUpdate.PhoneNumber = approveRegistrationDto.SignUpDto.User.PhoneNumber;
            supplierRepresentativeForUpdate.Name = approveRegistrationDto.SignUpDto.User.UserInfo.FullName;
            await supplierRepresentativeRepository.SaveEntityChanges(supplierRepresentativeForUpdate);

            await transaction.CommitAsync();
        }

        public async Task<bool> ExistsNonDeclined(int externalId, SupplierType supplierType, CancellationToken cancellationToken)
        {
            return await approveRegistrationRepository.AnyEntity(e => (supplierType == SupplierType.Institution 
                    ? e.Supplier.InstitutionId == externalId 
                    : e.Supplier.ComplexId == externalId) 
                && e.State != ApproveRegistrationState.Declined, cancellationToken);
        }
    }
}
