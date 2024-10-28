using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.FileManagementPackages.Csv;
using Infrastructure.FileManagementPackages.Excel.Services;
using Infrastructure.FileManagementPackages.FileConverters;
using Infrastructure.Helpers;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Integrations.AgencyRegixIntegration;
using Integrations.EAuth;
using Integrations.SsoIntegration;
using Logs;
using Logs.Services;
using Logs.Services.Search;
using MessageBroker.Consumer;
using MessageBroker.Consumer.Jobs;
using MessageBroker.Consumer.Services;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Middlewares;
using Newtonsoft.Json;
using OpenIddict.Validation.AspNetCore;
using ProxyKit;
using RegiXConsumer.Services;
using Sc.Jobs.Emails;
using Sc.Models;
using Sc.Reports.BgMap;
using Sc.Reports.ReceivedVouchers;
using Sc.Repositories.ApproveRegistrations;
using Sc.Repositories.Companies;
using Sc.Repositories.Nomenclatures;
using Sc.Repositories.Nomenclatures.Complexes;
using Sc.Repositories.Nomenclatures.Institutions;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Repositories.Nomenclatures.SmartSpecializations;
using Sc.Repositories.ReceivedVouchers;
using Sc.Repositories.Suppliers;
using Sc.Repositories.Suppliers.Junctions;
using Sc.Repositories.VoucherRequests;
using Sc.Services.AgencyRegix;
using Sc.Services.ApproveRegistrations;
using Sc.Services.ApproveRegistrations.Profiles;
using Sc.Services.Auth;
using Sc.Services.Auth.Profiles;
using Sc.Services.Companies;
using Sc.Services.Companies.Profiles;
using Sc.Services.Emails;
using Sc.Services.Nomenclatures.Complexes;
using Sc.Services.Nomenclatures.Complexes.Profiles;
using Sc.Services.Nomenclatures.Institutions;
using Sc.Services.Nomenclatures.Institutions.Profiles;
using Sc.Services.Nomenclatures.LawForms;
using Sc.Services.Nomenclatures.LawForms.Profiles;
using Sc.Services.Nomenclatures.Settlements;
using Sc.Services.Nomenclatures.Settlements.Profiles;
using Sc.Services.Nomenclatures.SmartSpecializations;
using Sc.Services.Nomenclatures.SmartSpecializations.Profiles;
using Sc.Services.ReceivedVouchers;
using Sc.Services.ReceivedVouchers.Permissions;
using Sc.Services.ReceivedVouchers.Profiles;
using Sc.Services.Suppliers;
using Sc.Services.Suppliers.Permissions;
using Sc.Services.Suppliers.Profiles;
using Sc.Services.VoucherRequests;
using Sc.Services.VoucherRequests.Permissions;
using Sc.Services.VoucherRequests.Profiles;
using Sc.SignalR.Hubs;
using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.SupplierOfferings.Services;
using Sc.Solr.Suppliers.Entities;
using Sc.Solr.Suppliers.Repositories;
using Sc.Solr.Suppliers.Services;
using SolrNet;

namespace Server.Extensions
{
    public static class InternalServicesExtensions
    {
        public static void ConfigureDbContextService(this IServiceCollection services)
        {
            services
                .AddDbContext<ScDbContext>(o =>
                {
                    o.UseNpgsql(AppSettingsProvider.MainDbConnectionString,
                    e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                })
                .AddDbContext<LogDbContext>(o =>
                {
                    o.UseNpgsql(AppSettingsProvider.LogDbConnectionString,
                        e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                });
        }

        public static void ConfigureHangfire(this IServiceCollection services)
        {
            services.AddHangfire(e =>
            {
                e.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                e.UsePostgreSqlStorage(s =>
                    s.UseNpgsqlConnection(AppSettingsProvider.MainDbConnectionString));
            });
            services.AddHangfireServer();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IInstitutionRepository, InstitutionRepository>()
                .AddScoped<IDistrictRepository, DistrictRepository>()
                .AddScoped<IMunicipalityRepository, MunicipalityRepository>()
                .AddScoped<ISettlementRepository, SettlementRepository>()
                .AddScoped<ILawFormRepository, LawFormRepository>()
                .AddScoped<IComplexRepository, ComplexRepository>()
                .AddScoped<ISmartSpecializationRepository, SmartSpecializationRepository>();

            services
                .AddScoped<ISupplierRepository, SupplierRepository>()
                .AddScoped<ISupplierOfferingRepository, SupplierOfferingRepository>()
                .AddScoped<ISupplierOfferingCounterRepository, SupplierOfferingCounterRepository>()
                .AddScoped<ISupplierTeamRepository, SupplierTeamRepository>()
                .AddScoped<ISupplierRepresentativeRepository, SupplierRepresentativeRepository>()
                .AddScoped<ISupplierEquipmentRepository, SupplierEquipmentRepository>()
                .AddScoped<ISoSmartSpecializationRepository, SoSmartSpecializationRepository>();

            services
                .AddScoped<ICompanyRepository, CompanyRepository>()
                .AddScoped<ICompanyRepresentativeRepository, CompanyRepresentativeRepository>()
                .AddScoped<ICompanyAdditionalRepository, CompanyAdditionalRepository>();

            services
                .AddScoped<IApproveRegistrationRepository, ApproveRegistrationRepository>()
                .AddScoped<IApproveRegistrationHistoryRepository, ApproveRegistrationHistoryRepository>();

            services
                .AddScoped<IVoucherRequestRepository, VoucherRequestRepository>()
                .AddScoped<IVoucherRequestCommunicationRepository, VoucherRequestCommunicationRepository>()
                .AddScoped<IVoucherRequestNotificationRepository, VoucherRequestNotificationRepository>();

            services
                .AddScoped<IReceivedVoucherRepository, ReceivedVoucherRepository>()
                .AddScoped<IReceivedVoucherHistoryRepository, ReceivedVoucherHistoryRepository>()
                .AddScoped<IReceivedVoucherCertificateRepository, ReceivedVoucherCertificateRepository>()
                .AddScoped<IReceivedVoucherCommunicationRepository, ReceivedVoucherCommunicationRepository>()
                .AddScoped<IReceivedVoucherNotificationRepository, ReceivedVoucherNotificationRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            #region Infrastructure
            services
                .AddScoped<DomainValidatorService>()
                .AddScoped<PermissionService>()
                .AddScoped<FileConverterService>()
                .AddScoped<ExcelProcessorService>()
                .AddScoped<EnumUtilityService>()
                .AddScoped<CsvProcessorService>()
                .AddScoped<CertificateService>();
            #endregion

            #region Integrations
            services
                .AddScoped<SsoIntegrationService>()
                .AddScoped<AgencyRegixIntegrationService>()
                .AddScoped<AgencyRegixService>()
                .AddScoped<IRegiXService, RegiXService>()
                .AddScoped<SamlHelperService>();
            #endregion

            #region Logs
            services
                .AddScoped<ActionLogService>()
                .AddScoped<ErrorLogService>()
                .AddScoped<ActionLogSearchService>()
                .AddScoped<ErrorLogSearchService>();
            #endregion

            #region Sc.Services
            services
                .AddScoped<AuthService>()
                .AddScoped<SupplierOfferingPermissionService>()
                .AddScoped<SupplierService>()
                .AddScoped<SupplierRepresentativeService>()
                .AddScoped<SupplierOfferingService>()
                .AddScoped<SupplierTeamService>()
                .AddScoped<SupplierEquipmentService>()
                .AddScoped<SupplierOfferingGroupService>()
                .AddScoped<SupplierSearchGroupService>()
                .AddScoped<CompanyService>()
                .AddScoped<CompanyRepresentativeService>()
                .AddScoped<CompanyAdditionalService>()
                .AddScoped<InstitutionService>()
                .AddScoped<ComplexService>()
                .AddScoped<DistrictService>()
                .AddScoped<MunicipalityService>()
                .AddScoped<SettlementService>()
                .AddScoped<LawFormService>()
                .AddScoped<SmartSpecializationService>()
                .AddScoped<ApproveRegistrationService>()
                .AddScoped<VoucherRequestPermissionService>()
                .AddScoped<VoucherRequestService>()
                .AddScoped<VoucherRequestCommunicationService>()
                .AddScoped<VoucherRequestNotificationService>()
                .AddScoped<ReceivedVoucherService>()
                .AddScoped<ReceivedVoucherHistoryService>()
                .AddScoped<ReceivedVoucherPermissionService>()
                .AddScoped<ReceivedVoucherCertificateService>()
                .AddScoped<ReceivedVoucherCertificateFileService>()
                .AddScoped<ReceivedVoucherNotificationService>()
                .AddScoped<ReceivedVoucherCommunicationService>()
                .AddScoped<EmailGenerationService>()
                .AddScoped<EmailSenderService>();
            #endregion

            #region Sc.Reports
            services
                .AddScoped<BgMapReportService>()
                .AddScoped<OfferingContractReportService>();
            #endregion

            #region UserContext
            services.AddScoped(typeof(UserContext), (provider) =>
            {
                var httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var ssoIntegrationService = provider.GetRequiredService<SsoIntegrationService>();
                var companyRepository = provider.GetRequiredService<ICompanyRepository>();
                var supplierRepository = provider.GetRequiredService<ISupplierRepository>();

                if (httpContext != null)
                {
                    string clientId = httpContext.User.FindFirst(OpenIdConnectConstants.Claims.ClientId)?.Value;
                    int? userId = int.TryParse(httpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value, out int currentUserId) ? currentUserId : null;

                    if (!string.IsNullOrWhiteSpace(clientId) && userId.HasValue)
                    {
                        var userContext = ssoIntegrationService.GetUserContext(httpContext).Result;

                        foreach (var companyOrgUnit in userContext.OrganizationalUnits
                            .Where(e => e.Alias == OrganizationalUnitConstants.CompanyAlias && e.ExternalId.HasValue))
                        {
                            var company = companyRepository.GetById(companyOrgUnit.ExternalId.Value, CancellationToken.None).Result;
                            companyOrgUnit.IsActive = company.IsActive;
                            userContext.CompanyId = company.Id;
                        }

                        foreach (var supplierOrgUnit in userContext.OrganizationalUnits
                            .Where(e => (string.IsNullOrWhiteSpace(e.Alias) && e.ExternalId.HasValue)
                                || (e.Alias == OrganizationalUnitConstants.ComplexAlias && e.ExternalId.HasValue)))
                        {
                            var supplierId = supplierRepository.GetListByProperties(e => supplierOrgUnit.Alias == OrganizationalUnitConstants.ComplexAlias
                                ? (e.ComplexId == supplierOrgUnit.ExternalId)
                                : (e.InstitutionId == supplierOrgUnit.ExternalId), CancellationToken.None).Result.FirstOrDefault()?.Id;

                            supplierOrgUnit.SupplierId = supplierId;
                        }

                        return userContext;
                    }
                    else
                    {
                        return new UserContext();
                    }
                }
                else
                {
                    return new UserContext();
                }
            });
            #endregion

            #region MessageBroker
            if (AppSettingsProvider.MessageBroker.Enable)
            {
                services.AddSingleton<ScMbConsumer>();

                services
                    .AddScoped<RndOrganizationUpdateService>()
                    .AddScoped<RndComplexUpdateService>();
            }
            #endregion
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AuthProfile());
                cfg.AddProfile(new SupplierProfile());
                cfg.AddProfile(new InstitutionProfile());
                cfg.AddProfile(new CompanyProfile());
                cfg.AddProfile(new SettlementsProfile());
                cfg.AddProfile(new LawFormsProfile());
                cfg.AddProfile(new ComplexProfiles());
                cfg.AddProfile(new SmartSpecializationsProfile());
                cfg.AddProfile(new ApproveRegistrationProfile());
                cfg.AddProfile(new VoucherRequestProfile());
                cfg.AddProfile(new ReceivedVoucherProfile());
            }).CreateMapper());
        }

        public static void ConfigureOpenIddict(this IServiceCollection services)
        {
            services.AddOpenIddict()
               .AddValidation(options =>
               {
                   options.SetIssuer(AppSettingsProvider.SsoConfiguration.SsoUri);

                   options.UseIntrospection()
                          .SetClientId(AppSettingsProvider.SsoConfiguration.ClientId)
                          .SetClientSecret(AppSettingsProvider.SsoConfiguration.ClientSecret);

                   options.UseSystemNetHttp();
                   options.UseAspNetCore();
               });

            services.AddAuthorization();
            services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }

        public static void ConfigureSolr(this IServiceCollection services)
        {
            var url = AppSettingsProvider.SolrConfiguration.Url;
            var supplierOfferingIndex = AppSettingsProvider.SolrConfiguration.SupplierOfferingIndex;
            var supplierEquipmentIndex = AppSettingsProvider.SolrConfiguration.SupplierEquipmentIndex;

            services.AddSolrNet<SolrSupplierOffering>($"{url}/{supplierOfferingIndex}");
            services.AddSolrNet<SolrSupplierEquipment>($"{url}/{supplierEquipmentIndex}");

            services
                .AddScoped<ISupplierOfferingSolrIndexService, SupplierOfferingSolrIndexService>()
                .AddScoped<ISupplierOfferingSolrSearchService, SupplierOfferingSolrSearchService>()
                .AddScoped<ISupplierEquipmentSolrIndexService, SupplierEquipmentSolrIndexService>()
                .AddScoped<ISupplierEquipmentSolrSearchService, SupplierEquipmentSolrSearchService>();
        }

        public static void ConfigureMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<RedirectionMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static void ConfigureJobs(this IServiceCollection services)
        {
            if (AppSettingsProvider.EmailConfiguration.JobEnabled)
            {
                services.AddHostedService<EmailJob>();
            }

            if (AppSettingsProvider.MessageBroker.Enable)
            {
                services
                    .AddHostedService<RndOrganizationUpdateJob>()
                    .AddHostedService<RndComplexUpdateJob>();
            }
        }

        public static void ConfigureStaticFiles(this WebApplication app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    if (context.File.Name == "index.html")
                    {
                        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                        context.Context.Response.Headers.Add("Expires", "-1");
                    }
                }
            });
        }

        public static void ConfigureSignalRHubs(this WebApplication app)
        {
            app.MapHub<NotificationHub>("/notificationHub");
            app.MapHub<VrCommunicationHub>("/vrCommunicationHub");
            app.MapHub<RvCommunicationHub>("/rvCommunicationHub");
        }

        public static void ConfigureProxy(this IApplicationBuilder app)
        {
            app.UseWhen(
                context => context.Request.Path.Value.StartsWith("/api/sso/"),
                builder =>
                {
                    var options = new RewriteOptions()
                        .AddRewrite(@"api/sso/(.*)", "api/$1", skipRemainingRules: true);
                    builder.UseRewriter(options);

                    builder.RunProxy(context => context
                        .ForwardTo(AppSettingsProvider.SsoConfiguration.SsoUri)
                        .Send());
                }
            );

            app.UseWhen(
                context => context.Request.Path.Value.StartsWith("/api/ras/ws"),
                builder =>
                {
                    var options = new RewriteOptions()
                        .AddRewrite(@"api/ras/(.*)", "api/$1", skipRemainingRules: true);
                    builder.UseRewriter(options);

                    builder.RunProxy(context => context
                        .ForwardTo(AppSettingsProvider.RasIntegration.RasUri)
                        .Send());
                }
            );
        }
    }
}
