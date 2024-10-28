using Sc.Models.Enums.Complexes;

namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexOrganization
    {
        public int Id { get; set; }

        public OrganizationTypeEnum? OrganizationTypeEnum { get; set; }
        public int? FinancingOrganizationLotId { get; set; }
        public int? OrganizationLotId { get; set; }
        public string NameRND { get; set; }
        public string NameRNDAlt { get; set; }
    }
}
