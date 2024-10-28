using Microsoft.EntityFrameworkCore;
using Migration.DbContexts.Rnd;
using NacidScMigration.DbContexts.Rnd.Models;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Enums.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Migration.MigrationServices.FromRnd
{
    public class RndInstitutionsMigration
    {
        private List<OrganizationCommit> rndOrganizationCommits;
        private readonly ScDbContext scDbContext;
        private readonly NacidRndContext rndContext;

        public RndInstitutionsMigration(
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
            Console.WriteLine("RND Institutions migration started. Time: " + DateTime.Now.ToString());
            InstitutionsMigration();
            Console.WriteLine("RND Institutions migration finished. Time: " + DateTime.Now.ToString());
            scTransaction.Commit();
        }

        public void InstitutionsMigration()
        {
            rndOrganizationCommits = rndContext.OrganizationCommits
                .AsNoTracking()
                .Include(e => e.Lot)
                .Include(e => e.OrganizationBasicPart.Entity)
                .Include(e => e.OrganizationCorrespondencePart.Entity)
                .Include(e => e.OrganizationFinancingInformationPart.Entity)
                .Where(e => (e.State == 3 || e.State == 4)
                    && e.OrganizationBasicPart != null
                    && e.OrganizationBasicPart.Entity != null
                    && e.OrganizationBasicPart.Entity.IsActive
                    && e.OrganizationBasicPart.Entity.OrganizationTypeId.HasValue
                    && (!e.OrganizationBasicPart.Entity.OrganizationLotId.HasValue 
                        ? (e.OrganizationBasicPart.Entity.OrganizationTypeId == 25 || e.OrganizationBasicPart.Entity.OrganizationTypeId == 7
                            || e.OrganizationBasicPart.Entity.OrganizationTypeId == 33 || e.OrganizationBasicPart.Entity.OrganizationTypeId == 29
                            || e.OrganizationBasicPart.Entity.OrganizationTypeId == 32)
                        : (e.OrganizationBasicPart.Entity.OrganizationTypeId == 1 || e.OrganizationBasicPart.Entity.OrganizationTypeId == 30
                            || e.OrganizationBasicPart.Entity.OrganizationTypeId == 34 || e.OrganizationBasicPart.Entity.OrganizationTypeId == 31
                            || e.OrganizationBasicPart.Entity.OrganizationTypeId == 32 || e.OrganizationBasicPart.Entity.OrganizationTypeId == 8)))
                .OrderBy(e => e.OrganizationBasicPart.Entity.OrganizationLotId == null)
                .ThenBy(e => e.LotId)
                .ToList();

            var ssoInstitutions = new List<Institution>();

            foreach (var rndOrganizationCommit in rndOrganizationCommits)
            {

                var (parentId, rootId, level) = GetInstitutionHierarchyInformation(rndOrganizationCommit.LotId, rndOrganizationCommit.OrganizationBasicPart.Entity.OrganizationLotId);

                if (level == null || (rndOrganizationCommit.OrganizationBasicPart.Entity.OrganizationLotId.HasValue && !parentId.HasValue))
                {
                    continue;
                }

                var institution = new Institution
                {
                    Code = rndOrganizationCommit.OrganizationBasicPart.Entity.Uic,
                    DistrictId = rndOrganizationCommit.OrganizationCorrespondencePart?.Entity?.DistrictId,
                    Id = rndOrganizationCommit.LotId,
                    OwnershipType = rndOrganizationCommit.OrganizationFinancingInformationPart?.Entity?.OwnershipTypeId,
                    OrganizationType = (Sc.Models.Enums.Institutions.OrganizationType)rndOrganizationCommit.OrganizationBasicPart.Entity.OrganizationTypeId.Value,
                    IsActive = rndOrganizationCommit.OrganizationBasicPart.Entity.IsActive,
                    Level = level.Value,
                    LotNumber = rndOrganizationCommit.Lot.Number,
                    MunicipalityId = rndOrganizationCommit.OrganizationCorrespondencePart?.Entity?.MunicipalityId,
                    Name = rndOrganizationCommit.OrganizationBasicPart.Entity.Name,
                    NameAlt = rndOrganizationCommit.OrganizationBasicPart.Entity.NameAlt,
                    ParentId = parentId,
                    RootId = rootId,
                    SettlementId = rndOrganizationCommit.OrganizationCorrespondencePart?.Entity?.SettlementId,
                    Uic = rndOrganizationCommit.OrganizationBasicPart.Entity.Uic,
                    Version = 0,
                    ViewOrder = 0,
                    IsResearchUniversity = rndOrganizationCommit.OrganizationBasicPart.Entity.IsResearchUniversity,
                    Address = rndOrganizationCommit.OrganizationCorrespondencePart?.Entity?.Address,
                    AddressAlt = rndOrganizationCommit.OrganizationCorrespondencePart?.Entity?.AddressAlt,
                    WebPageUrl = rndOrganizationCommit.OrganizationCorrespondencePart?.Entity?.WebPageUrl,
                    ShortName = rndOrganizationCommit.OrganizationBasicPart.Entity.ShortName,
                    ShortNameAlt = rndOrganizationCommit.OrganizationBasicPart.Entity.ShortNameAlt
                };

                ssoInstitutions.Add(institution);

                Console.WriteLine($"Institution LotId: {rndOrganizationCommit.LotId} added");
            }

            scDbContext.Institutions.AddRange(ssoInstitutions);
            scDbContext.SaveChanges();
        }

        private (int? parentId, int rootId, Level? level) GetInstitutionHierarchyInformation(int lotId, int? organizationParentId)
        {
            int? parentId = organizationParentId;
            int rootId = lotId;
            Level? level = Level.First;

            if (organizationParentId.HasValue && organizationParentId != lotId)
            {
                while (organizationParentId.HasValue)
                {
                    rootId = organizationParentId.Value;
                    level += 1;

                    try
                    {
                        organizationParentId = rndOrganizationCommits
                        .SingleOrDefault(e => e.LotId == organizationParentId.Value)
                        .OrganizationBasicPart?.Entity?.OrganizationLotId;

                        if (organizationParentId == rootId)
                        {
                            organizationParentId = null;
                        }
                    }
                    catch (Exception)
                    {
                        level = null;
                        organizationParentId = null;
                    }
                }
            }

            return (parentId, rootId, level);
        }
    }
}
