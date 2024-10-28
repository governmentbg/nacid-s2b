namespace Sc.Models.Dtos.Companies
{
    public class CompanyRepresentativeDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }

        public string PhoneNumber { get; set; }
    }
}
