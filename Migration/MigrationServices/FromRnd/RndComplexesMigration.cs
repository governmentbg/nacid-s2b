using Microsoft.EntityFrameworkCore;
using Migration.DbContexts.Rnd;
using Migration.DbContexts.Rnd.Models;
using NacidScMigration.DbContexts.Rnd.Models;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.Enums.Complexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Migration.MigrationServices.FromRnd
{
    public class RndComplexesMigration
    {
        private List<ComplexCommit> rndComplexCommits;
        private readonly ScDbContext scDbContext;
        private readonly NacidRndContext rndContext;

        public RndComplexesMigration(
            ScDbContext scDbContext,
            NacidRndContext rndContext
            )
        {
            this.scDbContext = scDbContext;
            this.rndContext = rndContext;
        }

        public void Start()
        {
            using var scTransaction = scDbContext.Database.BeginTransaction();
            Console.WriteLine("RND Complexes migration started. Time: " + DateTime.Now.ToString());
            ComplexesMigration();
            Console.WriteLine("RND Complexes migration finished. Time: " + DateTime.Now.ToString());
            scTransaction.Commit();
        }

        public void ComplexesMigration()
        {
            rndComplexCommits = rndContext.ComplexCommits
                .AsNoTracking()
                .Include(e => e.Lot)
                .Include(e => e.ComplexBasicPart.Entity)
                .Include(e => e.ComplexCorrespondencePart.Entity)
                .Include(e => e.OrganizationParts)
                    .ThenInclude(s => s.Entity)
                .Where(e => (e.State == 3 || e.State == 4)
                    && e.ComplexBasicPart != null
                    && e.ComplexBasicPart.Entity != null)
                .OrderBy(e => e.LotId)
                .ToList();

            var ssoComplexes = new List<Complex>();

            foreach (var rndComplex in rndComplexCommits)
            {
                var complex = new Complex
                {
                    Id = rndComplex.LotId,
                    LotNumber = rndComplex.Lot.Number,
                    Alias = "complex",
                    Name = rndComplex.ComplexBasicPart?.Entity?.Name,
                    NameAlt = rndComplex.ComplexBasicPart?.Entity?.NameAlt,
                    ShortName = rndComplex.ComplexBasicPart?.Entity?.ShortName,
                    ShortNameAlt = rndComplex.ComplexBasicPart?.Entity?.ShortNameAlt,
                    CoordinatorPosition = rndComplex.ComplexBasicPart?.Entity?.CoordinatorPosition,
                    CoordinatorPositionAlt = rndComplex.ComplexBasicPart?.Entity?.CoordinatorPositionAlt,
                    Category = rndComplex.ComplexBasicPart?.Entity?.Category,
                    CategoryAlt = rndComplex.ComplexBasicPart?.Entity.CategoryAlt,
                    Description = rndComplex.ComplexBasicPart?.Entity?.Description,
                    DescriptionAlt = rndComplex.ComplexBasicPart?.Entity?.DescriptionAlt,
                    Benefits = rndComplex.ComplexBasicPart?.Entity?.Benefits,
                    BenefitsAlt = rndComplex.ComplexBasicPart?.Entity?.BenefitsAlt,
                    ScientificTeam = rndComplex.ComplexBasicPart?.Entity?.ScientificTeam,
                    ScientificTeamAlt = rndComplex.ComplexBasicPart?.Entity?.ScientificTeamAlt,
                    AreaOfActivity = rndComplex.ComplexBasicPart?.Entity?.AreaOfActivityId != null
                        ? (AreaOfActivity)rndComplex.ComplexBasicPart?.Entity?.AreaOfActivityId : null,
                    DateFrom = rndComplex.ComplexBasicPart?.Entity?.DateFrom,
                    DateTo = rndComplex.ComplexBasicPart?.Entity?.DateTo,
                    EuropeanInfrastructure = rndComplex.ComplexBasicPart?.Entity?.EuropeanInfrastructure,
                    IsForeign = rndComplex?.ComplexCorrespondencePart?.Entity?.IsForeign ?? false,
                    DistrictId = rndComplex?.ComplexCorrespondencePart?.Entity?.DistrictId,
                    MunicipalityId = rndComplex?.ComplexCorrespondencePart?.Entity?.MunicipalityId,
                    SettlementId = rndComplex?.ComplexCorrespondencePart?.Entity?.SettlementId,
                    ForeignSettlement = rndComplex?.ComplexCorrespondencePart?.Entity?.ForeignSettlement,
                    ForeignSettlementAlt = rndComplex?.ComplexCorrespondencePart?.Entity?.ForeignSettlementAlt,
                    Address = rndComplex?.ComplexCorrespondencePart?.Entity?.Address,
                    AddressAlt = rndComplex?.ComplexCorrespondencePart?.Entity?.AddressAlt,
                    WebPageUrl = rndComplex?.ComplexCorrespondencePart?.Entity?.WebPageUrl,
                    PostCode = rndComplex?.ComplexCorrespondencePart?.Entity?.PostCode,
                    Phone = rndComplex?.ComplexCorrespondencePart?.Entity?.Phone,
                    Fax = rndComplex?.ComplexCorrespondencePart?.Entity?.Fax,
                    Email = rndComplex?.ComplexCorrespondencePart?.Entity?.Email,
                    IsActive = rndComplex.State != 6
                };

                foreach (var complexOrganizationPart in rndComplex.OrganizationParts)
                {
                    var complexOrganization = new Sc.Models.Entities.Nomenclatures.Complexes.ComplexOrganization
                    {
                        Id = complexOrganizationPart.InitialPartId.Value,
                        OrganizationTypeEnum = complexOrganizationPart.Entity?.OrganizationTypeEnum,
                        FinancingOrganizationLotId = complexOrganizationPart.Entity?.FinancingOrganizationLotId,
                        NameRND = complexOrganizationPart.Entity?.NameRND,
                        NameRNDAlt = complexOrganizationPart.Entity?.NameRNDAlt,
                        OrganizationLotId = complexOrganizationPart.Entity?.OrganizationLotId
                    };

                    OrganizationCommit organizationCommit = null;
                    FinancingOrganizationCommit financingOrganizationCommit = null;

                    if (complexOrganization.OrganizationTypeEnum == OrganizationTypeEnum.Financing && complexOrganization.FinancingOrganizationLotId.HasValue)
                    {
                        financingOrganizationCommit = rndContext.FinancingOrganizationCommits
                            .AsNoTracking()
                            .Include(e => e.FinancingOrganizationBasicPart.Entity)
                            .SingleOrDefault(e => (e.State == 3 || e.State == 4)
                                && e.FinancingOrganizationBasicPart != null
                                && e.FinancingOrganizationBasicPart.Entity != null
                                && e.LotId == complexOrganization.FinancingOrganizationLotId);

                        complexOrganization.FinancingOrganizationName = financingOrganizationCommit.FinancingOrganizationBasicPart.Entity.Name;
                        complexOrganization.FinancingOrganizationNameAlt = financingOrganizationCommit.FinancingOrganizationBasicPart.Entity.NameAlt;
                        complexOrganization.FinancingOrganizationShortName = financingOrganizationCommit.FinancingOrganizationBasicPart.Entity.ShortName;
                        complexOrganization.FinancingOrganizationShortNameAlt = financingOrganizationCommit.FinancingOrganizationBasicPart.Entity.ShortNameAlt;
                    }
                    else if (complexOrganization.OrganizationTypeEnum == OrganizationTypeEnum.Scientific && complexOrganization.OrganizationLotId.HasValue)
                    {
                        organizationCommit = rndContext.OrganizationCommits
                            .AsNoTracking()
                            .Include(e => e.OrganizationBasicPart.Entity)
                            .SingleOrDefault(e => (e.State == 3 || e.State == 4)
                                && e.OrganizationBasicPart != null
                                && e.OrganizationBasicPart.Entity != null
                                && e.LotId == complexOrganization.OrganizationLotId);

                        complexOrganization.OrganizationName = organizationCommit.OrganizationBasicPart.Entity.Name;
                        complexOrganization.OrganizationNameAlt = organizationCommit.OrganizationBasicPart.Entity.NameAlt;
                        complexOrganization.OrganizationShortName = organizationCommit.OrganizationBasicPart.Entity.ShortName;
                        complexOrganization.OrganizationShortNameAlt = organizationCommit.OrganizationBasicPart.Entity.ShortNameAlt;
                    }

                    complex.ComplexOrganizations.Add(complexOrganization);
                }

                ssoComplexes.Add(complex);

                Console.WriteLine($"Institution LotId: {rndComplex.LotId} added");
            }

            scDbContext.Complexes.AddRange(ssoComplexes);
            scDbContext.SaveChanges();
        }
    }
}
