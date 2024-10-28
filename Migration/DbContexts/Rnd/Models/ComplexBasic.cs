using System;

namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexBasic
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }


        public string CoordinatorPosition { get; set; }
        public string CoordinatorPositionAlt { get; set; }

        public bool IsCoordinatorRND { get; set; }
        public int? OrganizationCoordinatorLotId { get; set; }
        public string CoordinatorName { get; set; }
        public string CoordinatorNameAlt { get; set; }

        public bool IsFinancialCoordinatorRND { get; set; }
        public int? OrganizationFinancialCoordinatorLotId { get; set; }
        public string FinancialCoordinatorName { get; set; }
        public string FinancialCoordinatorNameAlt { get; set; }

        public bool IsScientificCoordinatorRND { get; set; }
        public int? OrganizationScientificCoordinatorLotId { get; set; }
        public string ScientificCoordinatorName { get; set; }
        public string ScientificCoordinatorNameAlt { get; set; }
        public string Category { get; set; }
        public string CategoryAlt { get; set; }

        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public string KeyWords { get; set; }
        public string KeyWordsAlt { get; set; }
        public string Benefits { get; set; }
        public string BenefitsAlt { get; set; }
        public string ScientificTeam { get; set; }
        public string ScientificTeamAlt { get; set; }
        public int? AreaOfActivityId { get; set; }
        public decimal? BudgetProject { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? Income { get; set; }
        public string EuropeanInfrastructure { get; set; }
    }
}
