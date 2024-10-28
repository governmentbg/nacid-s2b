using AutoMapper;
using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions.Constants;
using Infrastructure.Token;
using Integrations.SsoIntegration;
using Microsoft.AspNetCore.Http;
using Sc.Models;
using Sc.Models.Dtos.Auth;
using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Sso;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Auth;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.State;
using Sc.Models.Enums.Suppliers;
using Sc.Repositories.ApproveRegistrations;
using Sc.Repositories.Suppliers;
using Sc.Services.Companies;
using Sc.Services.Suppliers;

namespace Sc.Services.Auth
{
    public class AuthService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ScDbContext context;
        private readonly SsoIntegrationService ssoIntegrationService;
        private readonly CompanyService companyService;
        private readonly IApproveRegistrationRepository approveRegistrationRepository;
        private readonly ISupplierRepresentativeRepository supplierRepresentativeRepository;
        private readonly SupplierRepresentativeService supplierRepresentativeService;
        private readonly CompanyRepresentativeService companyRepresentativeService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly IMapper mapper;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            ScDbContext context,
            SsoIntegrationService ssoIntegrationService,
            CompanyService companyService,
            IApproveRegistrationRepository approveRegistrationRepository,
            ISupplierRepresentativeRepository supplierRepresentativeRepository,
            SupplierRepresentativeService supplierRepresentativeService,
            CompanyRepresentativeService companyRepresentativeService,
            DomainValidatorService domainValidatorService,
            IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
            this.ssoIntegrationService = ssoIntegrationService;
            this.companyService = companyService;
            this.approveRegistrationRepository = approveRegistrationRepository;
            this.supplierRepresentativeRepository = supplierRepresentativeRepository;
            this.supplierRepresentativeService = supplierRepresentativeService;
            this.companyRepresentativeService = companyRepresentativeService;
            this.domainValidatorService = domainValidatorService;
            this.mapper = mapper;
        }

        public async Task<TokenResponseDto> Login(LoginDto loginDto)
        {
            var loginContent = ConstructLoginContent(loginDto);
            var tokenResponseDto = await ssoIntegrationService.GetToken(loginContent);
            tokenResponseDto.ClientId = AppSettingsProvider.SsoConfiguration.ClientId;

            return tokenResponseDto;
        }

        public async Task<TokenResponseDto> LoginWithCode(LoginWithCodeDto loginWithCodeDto)
        {
            var loginContent = ConstructLoginAuthCodeContent(loginWithCodeDto.AuthorizationCode);
            var tokenResponseDto = await ssoIntegrationService.GetToken(loginContent);
            tokenResponseDto.ClientId = AppSettingsProvider.SsoConfiguration.ClientId;

            return tokenResponseDto;
        }

        public async Task SignUp(SignUpDto signUpDto, int? supplierId, CancellationToken cancellationToken)
        {
            using var transaction = context.BeginTransaction();

            var ssoSignUpDto = await ConstructSsoSignUpDto(signUpDto, cancellationToken);
            var ssoUserDto = await ssoIntegrationService.SignUp(ssoSignUpDto, httpContextAccessor.HttpContext);

            await transaction.CommitAsync();

            if (signUpDto.Type == SignUpType.Supplier
                && !await approveRegistrationRepository.AnyEntity(e => e.SupplierId == supplierId && e.State == ApproveRegistrationState.Approved, cancellationToken))
            {
                var supplierRepresentativeDto = mapper.Map<SupplierRepresentativeDto>(ssoUserDto);
                await supplierRepresentativeService.Create(supplierId.Value, supplierRepresentativeDto, cancellationToken);
            }
            else if (signUpDto.Type == SignUpType.Company
                && ssoSignUpDto.OrganizationalUnit.ExternalId.HasValue
                && !await companyRepresentativeService.Exists(ssoSignUpDto.OrganizationalUnit.ExternalId.Value, cancellationToken))
            {
                var companyRepresentativeDto = mapper.Map<CompanyRepresentativeDto>(ssoUserDto);
                await companyRepresentativeService.Create(ssoSignUpDto.OrganizationalUnit.ExternalId.Value, companyRepresentativeDto, cancellationToken);
            }
        }

        public async Task SignUpSupplierUpdate(SignUpDto signUpDto, int supplierId, CancellationToken cancellationToken)
        {
            var ssoSignUpDto = ConstructSsoSignUpSupplierEditDto(signUpDto, cancellationToken);
            var ssoUserDto = await ssoIntegrationService.SignUp(ssoSignUpDto, httpContextAccessor.HttpContext);

            var supplierRepresentativeDto = mapper.Map<SupplierRepresentativeDto>(ssoUserDto);
            supplierRepresentativeDto.Id = supplierId;
            var supplierRepresentativeForUpdate = await supplierRepresentativeRepository.GetById(supplierRepresentativeDto.Id, cancellationToken, supplierRepresentativeRepository.ConstructInclude(IncludeType.None));
            await supplierRepresentativeService.Update(supplierRepresentativeForUpdate, supplierRepresentativeDto, cancellationToken);
        }

        public async Task ActivateUser(SsoUserActivationDto ssoUserActivationDto)
        {
            await ssoIntegrationService.ActivateUser(ssoUserActivationDto);
        }

        public async Task GenerateRecoverCode(SsoUserEmailDto ssoUserEmailDto)
        {
            ssoUserEmailDto.RecoverLinkUrl = $"{AppSettingsProvider.Hosting}userRecover/confirm";
            await ssoIntegrationService.GenerateRecoverCode(ssoUserEmailDto);
        }

        public async Task RecoverPassword(SsoUserRecoverPasswordDto ssoUserRecoverPasswordDto)
        {
            await ssoIntegrationService.RecoverPassword(ssoUserRecoverPasswordDto);
        }

        public async Task ChangePassword(SsoChangePasswordDto ssoChangerPasswordDto)
        {
            await ssoIntegrationService.ChangePassword(ssoChangerPasswordDto, httpContextAccessor.HttpContext);
        }

        public async Task<SsoSignUpDto> ConstructSsoSignUpDto(SignUpDto signUpDto, CancellationToken cancellationToken)
        {
            var ssoSignUpDto = ConstructBaseSignUpDto(signUpDto);

            if (signUpDto.Type == SignUpType.Supplier)
            {
                if (await approveRegistrationRepository.AnyEntity(e => (signUpDto.SupplierType == SupplierType.Institution 
                    ? e.Supplier.InstitutionId == signUpDto.Institution.Id
                    : e.Supplier.ComplexId == signUpDto.Complex.Id) && e.State == ApproveRegistrationState.Approved, cancellationToken))
                {
                    var representativeNames = await supplierRepresentativeRepository.GetListByProperties(e => signUpDto.SupplierType == SupplierType.Institution
                    ? e.Supplier.InstitutionId == signUpDto.Institution.Id
                    : e.Supplier.ComplexId == signUpDto.Complex.Id, cancellationToken);

                    domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_Entity_Exists, null, representativeNames?.FirstOrDefault()?.Name);
                }

                ssoSignUpDto.OrganizationalUnit = mapper.Map<SsoOrganizationalUnitDto>(signUpDto.SupplierType == SupplierType.Institution ? signUpDto.Institution : signUpDto.Complex);
                ssoSignUpDto.RoleAlias = RoleAliasConstants.AuthorizedRepresentative;
            }
            else if (signUpDto.Type == SignUpType.Company)
            {
                var companyDto = await companyService.GetByUic(signUpDto.Company.Uic, cancellationToken);

                if (companyDto == null)
                {
                    var createdCompanyDto = await companyService.Create(signUpDto.Company, cancellationToken);
                    ssoSignUpDto.OrganizationalUnit = mapper.Map<SsoOrganizationalUnitDto>(createdCompanyDto);
                }
                else
                {
                    domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Entity_Exists);
                }

                ssoSignUpDto.OrganizationalUnit.Alias = OrganizationalUnitConstants.CompanyAlias;
                ssoSignUpDto.RoleAlias = RoleAliasConstants.CompanyRepresentative;
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(AuthErrorCode.Auth_InvalidSignUpType);
            }

            ssoSignUpDto.DeleteUnitUserWithSameRole = false;

            return ssoSignUpDto;
        }

        public SsoSignUpDto ConstructSsoSignUpTeamDto(SupplierTeamDto supplierTeamDto, SupplierDto supplierDto, CancellationToken cancellationToken)
        {
            var ssoSignUpDto = new SsoSignUpDto
            {
                User = new SsoUserDto
                {
                    UserName = supplierTeamDto.UserName,
                    PhoneNumber = supplierTeamDto.PhoneNumber,
                    UserInfo = new SsoUserInfoDto
                    {
                        FirstName = supplierTeamDto.FirstName,
                        MiddleName = supplierTeamDto.LastName,
                        LastName = supplierTeamDto.LastName,
                        FullName = supplierTeamDto.Name
                    }
                },
                ClientId = AppSettingsProvider.SsoConfiguration.ClientId,
                ActivationLinkUrl = $"{AppSettingsProvider.Hosting}userActivation",
                SystemText = "В платформата за сътрудничество между висшите училища, изследователските организации и бизнеса, поддържана от НАЦИД, е създаден нов профил на потребител:",
                RecaptchaToken = null,
                DeleteUnitUserWithSameRole = false,
                RoleAlias = RoleAliasConstants.AuthorizedRepresentativeTeam,
                OrganizationalUnit = new SsoOrganizationalUnitDto
                {
                    Alias = supplierDto.Type == SupplierType.Complex ? OrganizationalUnitConstants.ComplexAlias : null,
                    ExternalId = supplierDto.Type == SupplierType.Complex ? supplierDto.ComplexId : supplierDto.InstitutionId
                }
            };

            return ssoSignUpDto;
        }

        public SsoSignUpDto ConstructSsoSignUpSupplierEditDto(SignUpDto signUpDto, CancellationToken cancellationToken)
        {
            var ssoSignUpDto = ConstructBaseSignUpDto(signUpDto);

            if (signUpDto.Type == SignUpType.Supplier)
            {
                ssoSignUpDto.OrganizationalUnit = mapper.Map<SsoOrganizationalUnitDto>(signUpDto.SupplierType == SupplierType.Institution ? signUpDto.Institution : signUpDto.Complex);
                ssoSignUpDto.RoleAlias = RoleAliasConstants.AuthorizedRepresentative;
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(AuthErrorCode.Auth_InvalidSignUpType);
            }

            ssoSignUpDto.DeleteUnitUserWithSameRole = true;

            return ssoSignUpDto;
        }

        private SsoSignUpDto ConstructBaseSignUpDto(SignUpDto signUpDto)
        {
            var userInfo = signUpDto.User.UserInfo;
            userInfo.FullName = $"{userInfo.FirstName.Trim()} {userInfo.MiddleName?.Trim()} {userInfo.LastName.Trim()}";

            var ssoSignUpDto = new SsoSignUpDto
            {
                User = signUpDto.User,
                ClientId = AppSettingsProvider.SsoConfiguration.ClientId,
                RecaptchaToken = signUpDto.RecaptchaToken,
                SystemText = "В Платформата за сътрудничество между висшите училища, изследователските организации и бизнеса, поддържана от НАЦИД, е създаден нов профил на потребител:",
                ActivationLinkUrl = $"{AppSettingsProvider.Hosting}userActivation",
                ThrowErrorIfExists = signUpDto.Type == SignUpType.Company
            };

            return ssoSignUpDto;
        }

        private FormUrlEncodedContent ConstructLoginContent(LoginDto loginDto)
        {
            var loginKvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", AppSettingsProvider.SsoConfiguration.PasswordGrandType),
                new KeyValuePair<string, string>("client_id", AppSettingsProvider.SsoConfiguration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppSettingsProvider.SsoConfiguration.ClientSecret),
                new KeyValuePair<string, string>("username", loginDto.Username),
                new KeyValuePair<string, string>("password", loginDto.Password),
                new KeyValuePair<string, string>("recaptchaToken", loginDto.RecaptchaToken)
            };

            return new FormUrlEncodedContent(loginKvp);
        }

        private FormUrlEncodedContent ConstructLoginAuthCodeContent(string authorizationCode)
        {
            var loginKvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", AppSettingsProvider.SsoConfiguration.CodeGrandType),
                new KeyValuePair<string, string>("client_id", AppSettingsProvider.SsoConfiguration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppSettingsProvider.SsoConfiguration.ClientSecret),
                new KeyValuePair<string, string>("code", authorizationCode)
            };

            return new FormUrlEncodedContent(loginKvp);
        }
    }
}
